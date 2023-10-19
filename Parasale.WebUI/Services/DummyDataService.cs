using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Services
{
    public class DummyDataService
    {
        private readonly IConfigurationRoot _config;
        private readonly IRepositoryWrapper _repository;
        private readonly UserManager<AppUser> _userManager;
        public DummyDataService(IRepositoryWrapper repository, UserManager<AppUser> userManager, IConfigurationRoot config)
        {
            _repository = repository;
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> AddDummyData(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            ////Random random = new Random();
            ////var rand = random.Next(0, 999);
            var addUser = new AppUser()
            {
                UserName = "dummyUser_" + user.UserName,
                NormalizedUserName = "DUMMY_" + user.UserName,
                Email = "userdummy_" + user.UserName + "@yopmail.com",
                NormalizedEmail = "USERDUMMY_" + user.UserName + "@YOPMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAEAACcQAAAAEEk/sxHYeHdPnlE7tlItWaM3IBqvKopw0+XjHt1G3wydYeLdLes2+uM9VNN20PHw0g==",
                SecurityStamp = "ZRVX4PBBCA5C4Z422YQUTIHADC77MBXY",
                ConcurrencyStamp = "0dad1004-6983-4713-b757-e2a586496152",
                LockoutEnabled = true,
                IsManager = false,
                IsCompanyAdmin = false,
                AssginedAdmin = user.Id,
                voiceOnboarding = false,
                Name = "Dummy User_" + user.UserName,

            };

            await _repository.ParasaleRepository.Add(addUser);
            await _repository.ParasaleRepository.Save();
            var collection = new Collection()
            {
                CollectionName = "Dummy First Collection_"+user.UserName,
                appUser = user,
                AssignedAdmin = user.Id,
                isDummy = true
            };
            await _repository.ParasaleRepository.Add(collection);
            await _repository.ParasaleRepository.Save();
            var objection = new Objection()
            {
                InitialObjection = "Dummy First Objection_"+user.UserName,
                PitchKeyword = "Okay",
                ValidPitchResponse = "good job",
                BadPitchResponse = "try again",
                collection = collection,
                AssignedAdmin = user.Id,
                User = user,
                isDummy = true
            };
            await _repository.ParasaleRepository.Add(objection);
            await _repository.ParasaleRepository.Save();
            #region Commented Code
            //var qCollection = new QuickCollection()
            //{
            //    CollectionName = "Dummy Quick Collection_"+user.UserName,
            //    QuickStart = true,
            //    AssignedAdmin = user.Id,
            //    isDummy = true
            //};
            //await _repository.ParasaleRepository.Add(qCollection);
            //await _repository.ParasaleRepository.Save();
            //var qObjections = new QuickStartObjections()
            //{
            //    InitialObjection = "Dummy First Quick Objection",
            //    PitchKeyword = "Okay",
            //    ValidPitchResponse = "good job",
            //    BadPitchResponse = "try again",
            //    collection = qCollection,
            //    AssignedAdmin = user.Id,
            //    User = user,
            //    isDummy = true
            //};
            //await _repository.ParasaleRepository.Add(qObjections);
            //await _repository.ParasaleRepository.Save();
            #endregion
            for (int i = 1; i < 6; i++)
            {
                //bool completed;
                if (i % 2 == 0)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        var objectionLog = new ObjectionLog()
                        {
                            IsCompleted = true,
                            Objection = objection,
                            AppUser = addUser,
                            ObjectionTime = DateTime.Now.AddDays(-i),
                            AssignedAdmin = user.Id,
                            isDummy = true
                        };
                        await _repository.ParasaleRepository.Add(objectionLog);

                    }
                }
                else
                {
                    for (int k = 0; k < 5; k++)
                    {
                        var objectionLog = new ObjectionLog()
                        {
                            IsCompleted = false,
                            Objection = objection,
                            AppUser = addUser,
                            ObjectionTime = DateTime.Now.AddDays(-i),
                            AssignedAdmin = user.Id,
                            isDummy = true
                        };
                        await _repository.ParasaleRepository.Add(objectionLog);
                    }
                }
               
               
                var Ccs = new CCS()
                {
                    TotalScore = 781+i,
                    WPMTarget = 140,
                    WPMMeasure = 72+i,
                    WPMScore = 51 + i,
                    WPRTarget = 6,
                    WPRMeasure = 5+i,
                    WPRScore = 100,
                    RPATarget = 6,
                    RPA = 5+i,
                    RPAScore = 83+i,
                    TimeStamp = DateTime.Now.AddDays(-i),
                    appUser = addUser,
                    objection = objection,
                    isDummy = true
                };
              
                // await _repository.ParasaleRepository.Save();
                await _repository.ParasaleRepository.Add(Ccs);
                await _repository.ParasaleRepository.Save();
            }
            return "a";
        }


        public async Task<bool> DeleteAdminData(string userId)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var qcollection = _repository.QuickCollectionRepository.GetAllCollectionbyAdmin(userId);
            var colIds = qcollection.Select(x => x.Id).ToList();
            var underUsers = _repository.UserRepository.GetUsersByAdmin(userId);

            if (qcollection.Count() != 0)
            {

                var qObjection = _repository.QuickObjectionRepository.GetAllObjectionbyCollectionIds(colIds);
                foreach (var col in qcollection)
                {

                    if (qObjection.Count() > 0)
                    {
                        foreach (var qobj in qObjection)
                        {
                            _repository.ParasaleRepository.Delete(qobj);
                        }
                        await _repository.ParasaleRepository.Save();
                    }


                    _repository.ParasaleRepository.Delete(col);
                    try
                    {
                        await _repository.ParasaleRepository.Save();
                    }
                    catch (Exception ex)
                    {
                        throw ex.GetBaseException();
                    }
                }

            }

            var collection = _repository.CollectionRepository.GetAdminManagerCollections(userId);//.Where(x => x.isDummy == true);
            if (collection.Count != 0)
            {
                foreach (var col in collection)
                {
                    var Objection = _repository.ObjectionRepository.GetAllObjectionsByAdmin(userId);
                    try
                    {
                        if (Objection.Count() > 0)
                        {
                            foreach (var obj in Objection)
                            {
                                try
                                {
                                    var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogs(obj.Id);
                                    var ccs = _repository.CCSRepository.GetScorebyObjectionId(obj.Id);
                                    var sessions = _repository.SessionRepository.GetSessionObjection(obj.Id);
                                    var assignedCol = _repository.CollectionRepository.GetAllAdminCollections(userId).ToList();
                                    if (assignedCol.Count() > 0)
                                    {
                                        foreach (var aCol in assignedCol)
                                        {
                                            _repository.ParasaleRepository.Delete(aCol);
                                        }
                                        await _repository.ParasaleRepository.Save();
                                    }
                                    if (objectionLog.Count() > 0)
                                    {
                                        foreach (var logs in objectionLog)
                                        {

                                            _repository.ParasaleRepository.Delete(logs);
                                        }
                                        await _repository.ParasaleRepository.Save();
                                    }
                                    if (ccs.Count() > 0)
                                    {
                                        foreach (var ccss in ccs)
                                        {
                                            _repository.ParasaleRepository.Delete(ccss);

                                        }
                                        await _repository.ParasaleRepository.Save();
                                    }
                                    if (sessions.Count() > 0)
                                    {
                                        foreach (var ses in sessions)
                                        {
                                            _repository.ParasaleRepository.Delete(ses);
                                        }
                                        await _repository.ParasaleRepository.Save();
                                    }

                                    //_repository.ParasaleRepository.Delete(obj);
                                    _repository.ParasaleRepository.Delete(obj);
                                    await _repository.ParasaleRepository.Save();
                                }
                                catch (Exception ex)
                                {
                                    throw ex.GetBaseException();
                                }
                            }

                            await _repository.ParasaleRepository.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex.GetBaseException();
                    }

                    _repository.ParasaleRepository.Delete(col);
                    await _repository.ParasaleRepository.Save();
                }


            }
            var auditLog = _repository.AuditLogRepository.GetAuditLogsbyAdmin(userId);
            if (auditLog.Count > 0)
            {
                foreach (var log in auditLog)
                {
                    _repository.ParasaleRepository.Delete(log);
                }
                await _repository.ParasaleRepository.Save();
            }
            foreach (var usr in underUsers)
            {
                _repository.ParasaleRepository.Delete(usr);
            }
            await _repository.ParasaleRepository.Save();
            return true;

        }
        //public async Task<string> DeleteDummyData(string userName)
        //{
        // retrun null;
        //}
    }
}
