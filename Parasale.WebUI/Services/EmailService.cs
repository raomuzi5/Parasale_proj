using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Parasale.Models;
using Parasale.Repository;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Services
{
    public class EmailService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _config;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly IRepositoryWrapper _repository;
        private readonly UserManager<AppUser> _userManager;
        private Timer _timer;

        public EmailService(ILogger<EmailService> logger, IConfigurationRoot config,
            IServiceScopeFactory serviceScopeFactory,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _config = config;
            _env = env;

            var scope = _serviceScopeFactory.CreateScope();
            _repository = scope.ServiceProvider.GetService<IRepositoryWrapper>();
            _userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                _logger.LogInformation("Timed Background Service is working.");
                var users = _repository.UserRepository.GetUsers();
                foreach (var user in users)
                {
                    var objectionsCount = _repository.ObjectionLogRepository.GetObjectionLogsCount(user.Id, false);
                    if (objectionsCount != 0)
                    {
                        if (user.LastNotifiedOn < DateTime.Today && user.LastObjectionCount < objectionsCount)
                        {
                            var objectionLogs = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id, false);
                            user.LastNotifiedOn = DateTime.Today;
                            user.LastObjectionCount = objectionsCount;

                            _userManager.UpdateAsync(user);

                            SendMail(user.Email, "Missed Objection Notification", CreateMailBody(user.Name, objectionsCount, objectionLogs, NotificationType.Missed));
                        }
                    }
                    if (_repository.ObjectionLogRepository.AllObjectionsCompleted(user.Id))
                    {
                        var objectionLogs = _repository.ObjectionLogRepository.AllObjectionsCompleted(user.Id, DateTime.Today.AddDays(-1).Date);
                        SendMail(user.Email, "Congratulations!", CreateMailBody(user.Name, objectionLogs.Count, objectionLogs, NotificationType.Completed));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
        }

        public void SendMail(string to, string subject, string body)
        {
            try {
                var apiKey = _config.GetSection("apiSendgridKey").Value;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("concierge@parasale.co", "Parasale Co");
                List<EmailAddress> tos = new List<EmailAddress>
          {
              new EmailAddress(to, to)
                        };
                var subjects = subject;
                var htmlContent = body;
                var displayRecipients = false; // set this to true if you want recipients to see each others mail id
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subjects, "", htmlContent, false);
                var response = client.SendEmailAsync(msg);
            }
            catch(Exception ex)
            {
                
            }
            }
        //public void SendMail(string to, string subject, string body)
        //{
        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient(_config["EmailCongurations:smtpClient"]);

        //    mail.From = new MailAddress(_config["EmailCongurations:EmailAddress"]);
        //    mail.To.Add(to);
        //    mail.IsBodyHtml = true;
        //    mail.Subject = subject;
        //    mail.Body = body;

        //    SmtpServer.Port = Convert.ToInt32(_config["EmailCongurations:Port"]);
        //    SmtpServer.Credentials = new System.Net.NetworkCredential(_config["EmailCongurations:EmailAddress"], _config["EmailCongurations:Password"]);
        //    SmtpServer.EnableSsl = true;
        //    try
        //    {
        //        SmtpServer.Send(mail);
        //    }
        //    catch
        //    {

        //    }
        //}

        public string CreateMailBody(string Username, int objectionCount, List<ObjectionLog> objections, NotificationType type)
        {
            string filePath = Path.Combine(_env.ContentRootPath, "Views\\Shared\\ObjectionMailTemplate.html");
            StreamReader str = new StreamReader(filePath);
            string MailText = str.ReadToEnd();
            MailText = MailText.Replace("{Name}", Username);
            string trs = null;
            foreach (var item in objections)
            {
                trs = CreateTableRow(trs, item);
            }
            switch (type)
            {
                case NotificationType.Missed:
                    MailText = MailText.Replace("{ObjectionsCount}", $"You have missed total {Convert.ToString(objectionCount)} objection(s). Here is detail");
                    break;
                case NotificationType.Completed:
                    MailText = MailText.Replace("{ObjectionsCount}", $"You have <b>Completed</b> total {Convert.ToString(objectionCount)} objection(s) on date {DateTime.Today.AddDays(-1).Date.ToString("dd/MM/yyyy")}. Here is detail");
                    break;
                default:
                    break;
            }

            MailText = MailText.Replace("{DynamicDataTr}", trs);
            return MailText;
        }

        private static string CreateTableRow(string trs, ObjectionLog item)
        {
            trs += "<tr>";
            trs += "<td style='font-size: 18px; text-align: left; padding: 8px;line-height: 1.42857;vertical-align: top;border-top: 1px solid #e7ecf1'>";
            trs += $"{item.Objection.InitialObjection}<br>";
            trs += "</td>";
            trs += "<td style='font-size: 18px; text-align: left; padding: 8px;line-height: 1.42857;vertical-align: top;border-top: 1px solid #e7ecf1'>";
            trs += $"{item.Objection.PitchKeyword}<br>";
            trs += "</td>";
            trs += "<td style='font-size: 18px; text-align: left; padding: 8px;line-height: 1.42857;vertical-align: top;border-top: 1px solid #e7ecf1'>";
            trs += $"{item.ObjectionTime}<br>";
            trs += "</td>";
            trs += "</tr>";
            return trs;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
