using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using Parasale.WebUI.Models;
using Parasale.WebUI.Services;
using Vereyon.Web;

namespace Parasale.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private IRepositoryWrapper _repository;
        private IFlashMessage _flashMessage;
        private EmailService _emailServices;
        private UserManager<AppUser> _userManager;
        private DummyDataService _dummyDataService;
        private Random rnd = new Random();
        public DashboardController(IRepositoryWrapper repository, IFlashMessage flashMessage, EmailService emailService, UserManager<AppUser> userManager, DummyDataService dummyDataService)
        {
            _repository = repository;
            _flashMessage = flashMessage;
            _emailServices = emailService;
            _userManager = userManager;
            _dummyDataService = dummyDataService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dashboardModel = new DashboardIndexViewModel();
            var collections = _repository.CollectionRepository.GetAdminManagerCollections(user.Id);
            foreach (var item in collections)
            {
                dashboardModel.collections.Add(new CollectionListViewModel()
                {
                    Id = item.Id,
                    CollectionName = item.CollectionName

                });
            }
            var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id);

            var totalObjectionLogsCount = objectionLog.Count();//_repository.ObjectionLogRepository.GetObjectionLogs(user.Id).Count;
            dashboardModel.TotalActiveUsers = objectionLog.Select(p => p.AppUser).Distinct().Count(); //_repository.ObjectionLogRepository.GetObjectionLogs().Where(x => x.AssignedAdmin == user.Id).Select(p => p.AppUser).Distinct().Count();
            var ms = objectionLog.Where(x => x.IsCompleted == false).Count();
            var cm = objectionLog.Where(x => x.IsCompleted == true).Count();
            var missed = (double)ms / totalObjectionLogsCount * 100; //(double)_repository.ObjectionLogRepository.GetObjectionLogsCountbyAdmin(user.Id,false) / totalObjectionLogsCount * 100;
            var completed = (double)cm / totalObjectionLogsCount * 100;//_repository.ObjectionLogRepository.GetObjectionLogsCountbyAdmin(user.Id,true) / totalObjectionLogsCount * 100;

            var missedObjectionPercentage = double.IsNaN(missed) ? 0 : missed;
            var completedObjectionPercentage = double.IsNaN(completed) ? 0 : completed;

            var usersCount = _repository.UserRepository.GetUsersCount(user.Id);
            dashboardModel.Users = Convert.ToInt32(usersCount);
            //dashboardModel.TotalObjections = _repository.ObjectionRepository.GetObjectionsCount();
            dashboardModel.MissedObjections = missedObjectionPercentage;
            dashboardModel.CompletedObjections = completedObjectionPercentage;
            dashboardModel.voiceBoarding = user.voiceOnboarding;
            dashboardModel.userName = user.UserName;
            dashboardModel.currentStep = user.currentStep;
            dashboardModel.StepLevel = user.StepLevel;
            dashboardModel.path = user.path;

            return View(dashboardModel);
        }
        public async Task<IActionResult> ManageCollections(string after)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var collectionModel = new ManageCollectionsViewModel();
            var collections = _repository.CollectionRepository.GetAdminManagerCollections(user.Id);
            collectionModel.voiceBoarding = user.voiceOnboarding;
            collectionModel.userName = user.UserName;
            collectionModel.currentStep = user.currentStep;
            collectionModel.StepLevel = user.StepLevel;
            collectionModel.path = user.path;
            foreach (var item in collections)
            {
                collectionModel.collections.Add(new CollectionListViewModel()
                {
                    Id = item.Id,
                    CollectionName = item.CollectionName

                });
            }
            var qcollections = _repository.CollectionRepository.GetAllCollections(user.Id).Select(o => o.CollectionName).ToList();
            collectionModel.QuickCollection = _repository.QuickCollectionRepository.GetAllCollections().Where(x => x.QuickStart == true && !qcollections.Contains(x.CollectionName)).ToList();
            if (after != null)
            {
                return PartialView(collectionModel);
            }
            else
            {
                return View(collectionModel);
            }
        }
        public async Task<IActionResult> ManageTeamsAndUsers()
        {
            InvitesViewModel model = new InvitesViewModel();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.voiceboarding = user.voiceOnboarding;
            // Manage Team
            var users = _repository.UserRepository.GetUsersByAdmin(user.Id);
            var userTeam = users.Select(p => new Teams()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                IsManager = p.IsManager,
                ManagerUserId = p.AssignedManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true
            }).ToList();
            model.teams = userTeam;
            //User List
            var usrsList = users.Where(x => x.IsCompanyAdmin == false).Select(p => new Teams()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                IsManager = p.IsManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true
            }).ToList();
            model.userList = usrsList;
            return View(model);
        }
        public async Task<IActionResult> ResetAccount()
        {
            return View();
        }
        public async Task<IActionResult> AddObjection(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return PartialView("~/Areas/Admin/Views/Dashboard/_AddObjection.cshtml", new ObjectionAddViewModel()
            {
                collectionId = id,
                collections = _repository.CollectionRepository.GetAdminManagerCollections(user.Id).Select(p => new SelectListItem()
                {
                    Text = p.CollectionName,
                    Value = p.Id.ToString()
                }).ToList()
            });
        }
        public IActionResult GetObjectionsByCollection(int id)
        {
            return ViewComponent("AObjectionsbyCollection", new { id = id });
        }
        public async Task<IActionResult> GetAllObjections()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var objections = _repository.ObjectionRepository.GetAllObjectionsByAdmin(user.Id);
            var dashboardModel = new DashboardIndexViewModel();
            foreach (var item in objections)
            {
                dashboardModel.Objections.Add(new ObjectionViewModel()
                {
                    Id = item.Id,
                    ObjectionName = item.InitialObjection,
                    ResponseKeyword = item.PitchKeyword,
                    ManagerName = item.User == null ? "Admin" : item.User.Name
                });
            }
            return View(dashboardModel);
        }
        [HttpPost]
        //public async Task<IActionResult> AddObjection(ObjectionAddViewModel model)
        public async Task<IActionResult> AddObjection(string objectionName, string ResponseKeyword, int collectionId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                string keywordResponse = null;
                var responses = ResponseKeyword.Trim().Split(',');
                //var responses = model.ResponseKeyword.Trim().Split(',');
                if (responses.Count() > 1)
                {
                    keywordResponse = string.Join(", ", responses);
                }
                else
                {
                    keywordResponse = responses.FirstOrDefault();
                }


                Regex reg = new Regex("[*'\"_&#^@.?]");
                keywordResponse = reg.Replace(keywordResponse, string.Empty);

                Regex reg1 = new Regex("[ ]");
                keywordResponse = reg.Replace(keywordResponse, "-");

                await _repository.ParasaleRepository.Add(new Objection()
                {

                    InitialObjection = objectionName, //objectionName,//model.ObjectionName,
                    PitchKeyword = keywordResponse,//Regex.Replace(keywordResponse, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled),
                    // keywordResponse,
                    ValidPitchResponse = "good job",
                    BadPitchResponse = "try again",
                    AssignedAdmin = user.Id,
                    User = user,
                    collection = _repository.CollectionRepository.GetAllCollectionbyId(collectionId)
                });

                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = objectionName,
                    Type = "Created",
                    Field = "Objection created",
                    ModifiedBy = User.Identity.Name,
                    AdminId = user.Id,
                    UserId = user.Id,
                    ModifiedDate = DateTime.Now
                });

                if (await _repository.ParasaleRepository.Save())
                {
                    _flashMessage.Confirmation("Objection Saved Successfully");
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while saving objection");
                }
            }
            else
            {
                _flashMessage.Danger("Error Occurred while saving objection");
            }
            //return RedirectToAction("Index", new { area = "Admin", controller = "Dashboard" });
            return Json(new { success = true });
        }
        public async Task<IActionResult> EditObjection(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var objection = _repository.ObjectionRepository.GetObjection(id);
            return PartialView("~/Areas/Admin/Views/Dashboard/_EditObjection.cshtml", new ObjectionAddViewModel()
            {
                Id = objection.Id,

                ObjectionName = objection.InitialObjection,
                ResponseKeyword = objection.PitchKeyword,
                collectionId = objection.collection == null ? 0 : objection.collection.Id,

                collections = _repository.CollectionRepository.GetAdminManagerCollections(user.Id).Select(p => new SelectListItem()
                {
                    Text = p.CollectionName,
                    Value = p.Id.ToString()
                }).ToList()


            });
        }
        [HttpPost]
        public async Task<IActionResult> EditObjection(ObjectionAddViewModel model)
        //public async Task<IActionResult> EditObjection(int Id,string objectionName, string keyword, int collectionId)
        {
            if (ModelState.IsValid)
            {
                string keywordResponse = null;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var responses = model.ResponseKeyword.Trim().Split(',');
                if (responses.Count() > 1)
                    keywordResponse = string.Join(", ", responses);
                else
                    keywordResponse = responses.FirstOrDefault();

                var objection = _repository.ObjectionRepository.GetObjection(model.Id);

                if (objection.InitialObjection != model.ObjectionName)
                {
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = objection.InitialObjection,
                        NewData = model.ObjectionName,
                        Type = "Updated",
                        Field = "Objection Name updated",
                        ModifiedBy = User.Identity.Name,
                        AdminId = user.Id,
                        UserId = user.Id,
                        ModifiedDate = DateTime.Now
                    }); ;
                }


                if (objection.PitchKeyword != keywordResponse)
                {
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = objection.PitchKeyword,
                        NewData = keywordResponse,
                        Type = "Updated",
                        Field = "Objection Response Keyword updated",
                        ModifiedBy = User.Identity.Name,
                        AdminId = user.Id,
                        UserId = user.Id,
                        ModifiedDate = DateTime.Now
                    });
                }
                var collection = _repository.CollectionRepository.GetAllCollectionbyId(model.collectionId);
                if (objection.collection.CollectionName != collection.CollectionName)
                {
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = objection.collection.CollectionName.ToString(),
                        NewData = collection.CollectionName.ToString(),
                        Type = "Updated",
                        Field = "Objection Collection updated",
                        ModifiedBy = User.Identity.Name,
                        AdminId = user.Id,
                        UserId = user.Id,
                        ModifiedDate = DateTime.Now
                    });
                }

                objection.InitialObjection = model.ObjectionName;
                objection.PitchKeyword = keywordResponse;
                objection.AssignedAdmin = user.Id;
                objection.User = user;
                objection.collection = _repository.CollectionRepository.GetAllCollectionbyId(model.collectionId);
                _repository.ParasaleRepository.Update(objection);

                if (await _repository.ParasaleRepository.Save())
                {
                    _flashMessage.Confirmation("Objection Updated Successfully");
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while Updating objection");
                }
            }
            else
            {
                _flashMessage.Danger("Error Occurred while Updating objection");
            }
            // return Json(new { success=true});
            return RedirectToAction("ManageCollections", new { area = "Admin", controller = "Dashboard" });
        }
        public async Task<IActionResult> DeleteObjection(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var objection = _repository.ObjectionRepository.GetObjection(id);
            var logs = _repository.ObjectionLogRepository.GetObjectionLogs(objection.Id);
            var CCSObjections = _repository.CCSRepository.GetScorebyObjectionId(objection.Id);
            if (CCSObjections != null)
            {
                foreach (var ccs in CCSObjections)
                {
                    _repository.ParasaleRepository.Delete(ccs);
                }
            }
            if (logs != null && logs.Count > 0)
            {
                foreach (var log in logs)
                {
                    _repository.ParasaleRepository.Delete(log);
                }
            }
            _repository.ParasaleRepository.Delete(objection);

            await _repository.ParasaleRepository.Add(new AuditLog()
            {
                NewData = objection.InitialObjection,
                Type = "Deleted",
                Field = "Objection deleted",
                ModifiedBy = User.Identity.Name,
                AdminId = user.Id,
                UserId = user.Id,
                ModifiedDate = DateTime.Now
            });

            if (await _repository.ParasaleRepository.Save())
            {
                _flashMessage.Confirmation("Objection Deleted Successfully");
            }
            else
            {
                _flashMessage.Danger("Error Occurred while Deleting objection");
            }
            return RedirectToAction("ManageCollections", new { area = "Admin", controller = "Dashboard" });
        }
        public async Task<IActionResult> UnassignedObjectionsList()
        {
            return View();

        }
        public async Task<IActionResult> AdminCollections(string after)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.voiceboarding = user.voiceOnboarding;
            var collections = _repository.CollectionRepository.GetAllCollections(user.Id).Select(o => o.CollectionName).ToList();

            var quickCollections = _repository.QuickCollectionRepository.GetAllCollections().Where(x => x.QuickStart == true && !collections.Contains(x.CollectionName)).ToList();

            if (after != null)
            {
                return PartialView(quickCollections);
            }
            else
            {
                return View(quickCollections);
            }
        }
        public async Task<IActionResult> MoveToMyCollections(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var quickCollection = _repository.QuickCollectionRepository.GetAllCollectionbyId(id);
            var quickObjections = _repository.QuickObjectionRepository.GetAllObjectionbyCollectionId(id);
            if (quickCollection != null)
            {
                Collection collection = new Collection();
                collection.appUser = user;
                collection.AssignedAdmin = user.Id;
                collection.CollectionName = quickCollection.CollectionName;
                await _repository.ParasaleRepository.Add(collection);
                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = collection.CollectionName,
                    Type = "Moved From Quick",
                    Field = "Quick Start Collection",
                    ModifiedBy = User.Identity.Name,
                    UserId = user.Id,
                    AdminId = user.Id,
                    ModifiedDate = DateTime.Now
                });
                await _repository.ParasaleRepository.Save();
                if (quickObjections != null)
                {
                    foreach (var objections in quickObjections)
                    {
                        Objection objection = new Objection();
                        objection.AssignedAdmin = user.Id;
                        objection.BadPitchResponse = objections.BadPitchResponse;
                        objection.collection = collection;
                        objection.InitialObjection = objections.InitialObjection;
                        objection.PitchKeyword = objections.PitchKeyword;
                        objection.ValidPitchResponse = objections.ValidPitchResponse;
                        objection.User = user;
                        await _repository.ParasaleRepository.Add(objection);
                    }
                    if (await _repository.ParasaleRepository.Save())
                    {
                        _flashMessage.Confirmation("Moved Successfully.");
                    }
                    else
                    {
                        _flashMessage.Danger("Error Occurred while Moving.");
                    }
                }
            }
            return Json(true);
        }
        public class userInvite
        {
            public string value { get; set; }
        }
        #region Commented Code
        //public async Task<IActionResult> ForReportData(string type)
        #endregion
        public async Task<IActionResult> GetUserLists()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UsersCheckViewModel usersCheckViewModel = new UsersCheckViewModel();
            usersCheckViewModel.invitesList = _repository.InvitesRepository.GetAllInvities(user.Id);
            usersCheckViewModel.appUserList = _repository.UserRepository.GetUsers(true);
            return View(usersCheckViewModel);
        }
        public async Task<IActionResult> InviteUsers()
        {
            InvitesViewModel model = new InvitesViewModel();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.voiceboarding = user.voiceOnboarding;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> InviteUsers(InvitesViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            bool IsUserExist = false;
            var Emails = JsonConvert.DeserializeObject<List<userInvite>>(model.Invites.Email);
            var userid = await _userManager.FindByNameAsync(User.Identity.Name);
            if (Emails.Count() > 0)
            {
                var existingUsers = _repository.InvitesRepository.GetAllInvities(userid.Id);

                foreach (var email in Emails)
                {
                    var isUserExist = await _userManager.FindByEmailAsync(email.value);
                    if (isUserExist == null)
                    {
                        foreach (var exUsers in existingUsers)
                        {
                            if (exUsers.Email == email.value)
                            {
                                IsUserExist = true;
                                SendInvitations(exUsers.Email, exUsers.Guid, exUsers.AssignedAdmin);
                                await _repository.ParasaleRepository.Add(new AuditLog()
                                {
                                    NewData = email.value,
                                    Type = "Resent Invitation",
                                    Field = userid.Name + "Re-Invited" + email.value,
                                    ModifiedBy = User.Identity.Name,
                                    AdminId = user.Id,
                                    UserId = user.Id,
                                    ModifiedDate = DateTime.Now
                                });
                            }

                        }

                        if (!IsUserExist)
                        {
                            var guid = Guid.NewGuid().ToString();

                            await _repository.ParasaleRepository.Add(new Invites()
                            {
                                Email = email.value,
                                IsSigned = false,
                                Guid = guid,
                                AssignedAdmin = userid.Id,
                                //Link = Url.Action("Register", "Account", new { token = guid, value = userid.Id })
                                //Link = "https://parasale.co/Account/Register?token=" + guid + "&value=" + userid.Id
                                Link = Url.Action("Register", "Account", new { area = "", token = guid, value = userid.Id }, protocol: HttpContext.Request.Scheme)
                            });
                            await _repository.ParasaleRepository.Add(new AuditLog()
                            {
                                NewData = email.value,
                                Type = "Invitation",
                                Field = userid.Name + " has sent signup Invitation to " + email.value,
                                ModifiedBy = User.Identity.Name,
                                AdminId = user.Id,
                                UserId = user.Id,
                                ModifiedDate = DateTime.Now
                            });
                            SendInvitations(email.value, guid, userid.Id);
                            await _repository.ParasaleRepository.Save();
                        }

                    }
                }


            }
            model.success = true;
            //return View(model);
            return RedirectToAction("ManageTeamsAndUsers", new { area = "Admin", controller = "Dashboard" });
            //return RedirectToAction("Admin/Dashboard/ManageTeamsAndUsers",model);
        }
        #region Code not in use
        //public IActionResult ApproveUser(string id)
        //{

        //    var getUser = _repository.UserRepository.GetUser(id);
        //    if (getUser != null)
        //    {
        //        getUser.LockoutEnd = DateTime.Now;
        //        _repository.ParasaleRepository.Update(getUser);
        //        _repository.ParasaleRepository.Save();
        //        var subject = "Request Approved.";
        //        var body = "Your account is now unlocked. You can now sign in.";
        //        _emailServices.SendMail(getUser.Email, subject, body);

        //    }

        //    return RedirectToAction("InviteUsers", new { area = "Admin", controller = "Dashboard" });
        //}
        //public class RootObject
        //{
        //    public int y { get; set; }
        //    public string label { get; set; }
        //}
        #endregion
        public void SendInvitations(string email, string guid, string userId)
        {
            //var link = "https://parasale.co/Account/Register?token=" + guid + "&value=" + userId;
            var link = Url.Action("Register", "Account", new { area = "", token = guid, value = userId }, protocol: HttpContext.Request.Scheme);
            string subject = "Sign up Invitation";
            string body = $"You are invited for sign up. Please <a href='{link}'>click here</a> to proceed.";
            _emailServices.SendMail(email, subject, body);
        }
        public async Task<IActionResult> Reports()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dashboardModel = new DashboardIndexViewModel();
            var collections = _repository.CollectionRepository.GetAdminManagerCollections(user.Id);
            foreach (var item in collections)
            {
                dashboardModel.collections.Add(new CollectionListViewModel()
                {
                    Id = item.Id,
                    CollectionName = item.CollectionName

                });
            }
            var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id);

            var totalObjectionLogsCount = objectionLog.Count();//_repository.ObjectionLogRepository.GetObjectionLogs(user.Id).Count;
            dashboardModel.TotalActiveUsers = objectionLog.Select(p => p.AppUser).Distinct().Count(); //_repository.ObjectionLogRepository.GetObjectionLogs().Where(x => x.AssignedAdmin == user.Id).Select(p => p.AppUser).Distinct().Count();
            var ms = objectionLog.Where(x => x.IsCompleted == false).Count();
            var cm = objectionLog.Where(x => x.IsCompleted == true).Count();
            var missed = (double)ms / totalObjectionLogsCount * 100; //(double)_repository.ObjectionLogRepository.GetObjectionLogsCountbyAdmin(user.Id,false) / totalObjectionLogsCount * 100;
            var completed = (double)cm / totalObjectionLogsCount * 100;//_repository.ObjectionLogRepository.GetObjectionLogsCountbyAdmin(user.Id,true) / totalObjectionLogsCount * 100;

            var missedObjectionPercentage = double.IsNaN(missed) ? 0 : missed;
            var completedObjectionPercentage = double.IsNaN(completed) ? 0 : completed;

            var usersCount = _repository.UserRepository.GetUsersCount(user.Id);
            dashboardModel.Users = Convert.ToInt32(usersCount);
            //dashboardModel.TotalObjections = _repository.ObjectionRepository.GetObjectionsCount();
            dashboardModel.MissedObjections = missedObjectionPercentage;
            dashboardModel.CompletedObjections = completedObjectionPercentage;
            dashboardModel.voiceBoarding = user.voiceOnboarding;
            dashboardModel.userName = user.UserName;
            dashboardModel.currentStep = user.currentStep;
            dashboardModel.StepLevel = user.StepLevel;
            dashboardModel.path = user.path;

            return View(dashboardModel);
        }
        public class ReportData
        {
            public string name { get; set; }
            public List<int> data { get; set; }
        }
        public async Task<IActionResult> getReportData(string type, string startDate, string endDate)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);
            var userr = await _userManager.FindByNameAsync(User.Identity.Name);
            var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogs(userr.Id).Where(x => x.ObjectionTime.Date >= sDate.Date && x.ObjectionTime.Date <= eDate.Date);
            var allManagers = _repository.UserRepository.GetManagersByAdmin(userr.Id);
            var AllUsers = _repository.UserRepository.GetUsersByAdmin(userr.Id).Where(x => x.IsManager != true && x.IsCompanyAdmin != true).ToList();
            ReportData report = new ReportData();
            var dates = new List<DateTime>();
            var dateString = new List<string>();
            for (var dt = sDate; dt <= eDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
                dateString.Add(dt.ToShortDateString());
            }
            List<ReportData> finalList = new List<ReportData>();

            List<int> counts = new List<int>();
            if (type == "byUser")
            {
                foreach (var user in AllUsers)
                {
                    counts = new List<int>();
                    report = new ReportData();
                    foreach (var date in dates)
                    {
                        var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == user.Id && x.ObjectionTime.Date == date.Date).Distinct().Count();

                        counts.Add(CompletedObjections);
                    }

                    report.name = user.Name;
                    report.data = counts;

                    finalList.Add(report);

                }
            }
            if (type == "byManager")
            {

                foreach (var user in allManagers)
                {
                    var userIn = AllUsers.Where(x => x.AssignedManager == user.Id && x.IsManager == false);
                    var userId = userIn.Select(x => x.Id).ToList();
                    //foreach (var users in userIn)
                    //{
                    counts = new List<int>();
                    report = new ReportData();
                    foreach (var date in dates)
                    {
                        var CompletedObjections = allObjectionLog.Where(x => userId.Contains(x.AppUser.Id) && x.IsCompleted == true && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();
                        counts.Add(CompletedObjections);
                    }

                    report.name = user.Name;
                    report.data = counts;
                    finalList.Add(report);
                    //}

                }
            }

            if (type == "byCollection")
            {
                var allCollections = _repository.CollectionRepository.GetAdminManagerCollections(userr.Id);
                var allObjections = _repository.ObjectionRepository.GetAllObjectionsByAdmin(userr.Id);
                var TempObjections = allObjections.Where(x => x.collection != null);
                foreach (var collect in allCollections)
                {
                    var obj = TempObjections.Where(x => x.collection.Id == collect.Id).ToList();
                    var objId = obj.Select(x => x.Id);
                    if (obj != null)
                    {
                        counts = new List<int>();
                        report = new ReportData();
                        foreach (var date in dates)
                        {
                            var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && objId.Contains(x.Objection.Id) && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();
                            counts.Add(CompletedObjections);
                        }
                        report.name = collect.CollectionName;
                        report.data = counts;
                        finalList.Add(report);
                    }


                }

            }
            return Json(new { completed = finalList, completedDates = dateString });

        }
        #region commented Code
        //[HttpPost]
        //public async Task<IActionResult> GetSelectedData([FromForm] List<string> models, string type, string startDate, string endDate)
        //{
        //    DateTime sDate = Convert.ToDateTime(startDate);
        //    DateTime eDate = Convert.ToDateTime(endDate);
        //    var splited = models[0].Split(',');
        //    var userr = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogs(userr.Id).Where(x => x.ObjectionTime >= sDate && x.ObjectionTime <= eDate);
        //    var allManagers = _repository.UserRepository.GetManagersByAdmin(userr.Id);
        //    var AllUsers = _repository.UserRepository.GetUsersByAdmin(userr.Id).Where(x => x.IsManager != true && x.IsCompanyAdmin != true).ToList();
        //    List<AppUser> appUsers = new List<AppUser>();
        //    List<Collection> collections1 = new List<Collection>();
        //    ReportData report = new ReportData();
        //    var dates = new List<DateTime>();
        //    var datestring = new List<string>();
        //    for (var dt = sDate; dt <= eDate; dt = dt.AddDays(1))
        //    {
        //        dates.Add(dt);
        //        datestring.Add(dt.ToShortDateString());
        //    }
        //    List<ReportData> finalList = new List<ReportData>();

        //    List<int> counts = new List<int>();
        //    if (models.Count() != 0)
        //    {
        //        if (type == "byUser")
        //        {
        //            foreach (var users in splited)
        //            {
        //                var userss = AllUsers.Where(x => x.Id == users && x.IsManager == false && x.IsCompanyAdmin == false).ToList();
        //                if (userss.Count != 0)
        //                {
        //                    appUsers.Add(userss[0]);
        //                }
        //            }


        //            foreach (var user in appUsers)
        //            {
        //                counts = new List<int>();
        //                report = new ReportData();
        //                foreach (var date in dates)
        //                {
        //                    var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == user.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();

        //                    counts.Add(CompletedObjections);
        //                }

        //                report.name = user.Name;
        //                report.data = counts;

        //                finalList.Add(report);

        //            }
        //        }
        //    }
        //    if (type == "byManager")
        //    {
        //        foreach (var users in splited)
        //        {
        //            var userss = AllUsers.FirstOrDefault(x => x.Id == users);
        //            appUsers.Add(userss);
        //        }
        //        foreach (var user in appUsers)
        //        {
        //            var userIn = AllUsers.Where(x => x.AssignedManager == user.Id && x.IsManager == false);
        //            foreach (var users in userIn)
        //            {
        //                counts = new List<int>();
        //                report = new ReportData();
        //                foreach (var date in dates)
        //                {
        //                    var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == users.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();
        //                    counts.Add(CompletedObjections);
        //                }
        //                report.name = users.Name;
        //                report.data = counts;
        //                finalList.Add(report);
        //            }

        //        }
        //    }

        //    if (type == "byCollection")
        //    {
        //        var allCollections = _repository.CollectionRepository.GetAllCollections(userr.Id);
        //        var allObjections = _repository.ObjectionRepository.GetAllObjectionsByAdmin(userr.Id);
        //        var TempObjections = allObjections.Where(x => x.collection != null);
        //        foreach (var collection in splited)
        //        {
        //            var collections = allCollections.Where(x => x.Id == Convert.ToInt32(collection) && x.AssignedAdmin == userr.Id).ToList();
        //            collections1.Add(collections[0]);
        //        }
        //        foreach (var collect in collections1)
        //        {

        //            var obj = TempObjections.FirstOrDefault(x => x.collection.Id == collect.Id);
        //            if (obj != null)
        //            {
        //                counts = new List<int>();
        //                report = new ReportData();
        //                foreach (var date in dates)
        //                {
        //                    var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.Objection.Id == obj.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();
        //                    counts.Add(CompletedObjections);
        //                }
        //                report.name = collect.CollectionName;
        //                report.data = counts;
        //                finalList.Add(report);
        //            }


        //        }
        //        //foreach (var objections in allCollections)
        //        //{
        //        //    var obj = allObjections.FirstOrDefault(x => x.collection.Id == objections.Id);
        //        //    if (obj != null)
        //        //    {
        //        //        var temp = allObjectionLog.Where(x => x.IsCompleted == true && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //        //        var temp2 = allObjectionLog.Where(x => x.IsCompleted == false && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //        //        Completed.Add(objections.CollectionName, temp);
        //        //        Incompleted.Add(objections.CollectionName, temp2);

        //        //    }
        //        //}

        //        ////var allObjections = _repository.ObjectionRepository.GetAllbjections();
        //        //foreach (var objections in allCollections)
        //        //{
        //        //    var obj = allObjections.FirstOrDefault(x => x.collection.Id == objections.Id);
        //        //    if (obj != null)
        //        //    {
        //        //        var temp = allObjectionLog.Where(x => x.IsCompleted == true && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //        //        var temp2 = allObjectionLog.Where(x => x.IsCompleted == false && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //        //        Completed.Add(objections.CollectionName, temp);
        //        //        Incompleted.Add(objections.CollectionName, temp2);

        //        //    }
        //        //}
        //    }
        //    //if (type == "byCollection")
        //    //{
        //    //    var allCollections = _repository.CollectionRepository.GetAllCollections(userr.Id);
        //    //    var allObjections = _repository.ObjectionRepository.GetAllObjectionsByAdmin(userr.Id);
        //    //    //var allObjections = _repository.ObjectionRepository.GetAllbjections();
        //    //    foreach (var objections in allCollections)
        //    //    {
        //    //        var obj = allObjections.FirstOrDefault(x => x.collection.Id == objections.Id);
        //    //        if (obj != null)
        //    //        {
        //    //            var temp = allObjectionLog.Where(x => x.IsCompleted == true && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //    //            var temp2 = allObjectionLog.Where(x => x.IsCompleted == false && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //    //            Completed.Add(objections.CollectionName, temp);
        //    //            Incompleted.Add(objections.CollectionName, temp2);

        //    //        }
        //    //    }
        //    //    //foreach (var user in allCollections)
        //    //    //{


        //    //    //}
        //    //}
        //    return Json(new { completed = finalList, completedDates = datestring });

        //}
        #endregion
        public async Task<IActionResult> AuditLogs()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var logs = _repository.ObjectionLogRepository.AuditLogsbyAdmin(user.Id);
            ViewBag.voiceboarding = user.voiceOnboarding;
            return View(logs);
        }
        public async Task<IActionResult> PracticeObjections()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var VoiceOnBoarding = _repository.VoiceOnboarding.GetVoiceOnBoardingByUserId(user.Id);
            var LastUserHistory = _repository.UserHistory.GetLastUserHistory(user.Id);
            UserIndexViewModel model = new UserIndexViewModel()
            {
                collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin).Select(x => new SelectListItem()
                {
                    Text = x.CollectionName,
                    Value = x.Id.ToString()
                })
            };
            model.VoiceOnBoarding = VoiceOnBoarding;
            model.LastUserHistory = LastUserHistory;
            model.VoiceBoarding = user.voiceOnboarding;
            model.CurrentStep = user.currentStep;
            model.userId = user.Id;
            model.UserName = user.UserName;
            model.Path = user.path;
            return View(model);
        }
        //public async Task<IActionResult> GetMicPermission()
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var VoiceOnBoarding = _repository.VoiceOnboarding.GetVoiceOnBoardingByUserId(user.Id);
        //    var LastUserHistory = _repository.UserHistory.GetLastUserHistory(user.Id);
        //    UserIndexViewModel model = new UserIndexViewModel()
        //    {
        //        collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin).Select(x => new SelectListItem()
        //        {
        //            Text = x.CollectionName,
        //            Value = x.Id.ToString()
        //        })
        //    };
        //    model.VoiceOnBoarding = VoiceOnBoarding;
        //    model.LastUserHistory = LastUserHistory;
        //    model.VoiceBoarding = user.voiceOnboarding;
        //    model.CurrentStep = user.currentStep;
        //    model.userId = user.Id;
        //    model.UserName = user.UserName;
        //    model.Path = user.path;
        //    return View(model);
        //}
        #region CCS Report For Manager
        public async Task<IActionResult> getCCSReportData(string type, string startDate, string endDate)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);
            var userr = await _userManager.FindByNameAsync(User.Identity.Name);
            var AllUsers = _repository.UserRepository.GetUsersByAdmin(userr.Id).Where(x => x.IsManager == false && x.IsCompanyAdmin == false).ToList();
            var userIds = AllUsers.Select(x => x.Id).ToList();
            var allObjections = _repository.ObjectionRepository.GetAllObjectionsByAdmin(userr.Id);
            var allScore = _repository.CCSRepository.GetScorebyManagerUsers(userIds).Where(x => x.TimeStamp.Value.Date >= sDate.Date && x.TimeStamp.Value.Date <= eDate.Date);
            var TempObjections = allObjections.Where(x => x.collection != null);
            ReportData report = new ReportData();
            var dates = new List<DateTime>();
            var dateString = new List<string>();
            for (var dt = sDate; dt <= eDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
                dateString.Add(dt.ToShortDateString());

            }
            List<ReportData> finalList = new List<ReportData>();
            List<int> counts = new List<int>();
            if (type == "byObjection")
            {
                var allCollections = _repository.CollectionRepository.GetAdminManagerCollections(userr.Id);
                var collectionId = allCollections.Select(x => x.Id).ToList();
                var assignedObjections = TempObjections.Where(x => collectionId.Contains(x.collection.Id));
                foreach (var objection in assignedObjections)
                {
                    counts = new List<int>();
                    report = new ReportData();

                    foreach (var date in dates)
                    {
                        var dateScore = allScore.Where(x => x.objection.Id == objection.Id && x.TimeStamp.Value.Date == date.Date).Select(o => o.TotalScore).Average();
                        counts.Add(Convert.ToInt32(dateScore));
                    }
                    var objString = objection.InitialObjection.Split();
                    if (objString.Count() > 3)
                        report.name = objString[0] + ' ' + objString[1] + ' ' + objString[2];
                    else
                        report.name = objection.InitialObjection;
                    report.data = counts;
                    finalList.Add(report);

                }
            }

            if (type == "byCollection")
            {
                var allCollections = _repository.CollectionRepository.GetAdminManagerCollections(userr.Id);
                var collectionId = allCollections.Select(x => x.Id).ToList();
                foreach (var collect in allCollections)
                {
                    var obj = TempObjections.Where(x => x.collection.Id == collect.Id).ToList();
                    var objId = obj.Select(x => x.Id).ToList();
                    if (obj != null)
                    {
                        counts = new List<int>();
                        report = new ReportData();
                        foreach (var date in dates)
                        {
                            var dateScore = allScore.Where(x => objId.Contains(x.objection.Id) && x.TimeStamp.Value.Date == date.Date).Select(o => o.TotalScore).Average();
                            counts.Add(Convert.ToInt32(dateScore));
                        }
                    }
                    report.name = collect.CollectionName;
                    report.data = counts;
                    finalList.Add(report);
                }
            }

            if (type == "byUser")
            {
                foreach (var user in AllUsers)
                {
                    counts = new List<int>();
                    report = new ReportData();
                    foreach (var date in dates)
                    {
                        var dateScore = allScore.Where(x => x.appUser.Id == user.Id && x.TimeStamp.Value.Date == date.Date).Select(o => o.TotalScore).Average();
                        counts.Add(Convert.ToInt32(dateScore));
                    }

                    report.name = user.Name;
                    report.data = counts;

                    finalList.Add(report);

                }

            }

            if (type == "byManager")
            {
                var allManagers = _repository.UserRepository.GetManagersByAdmin(userr.Id);
                foreach (var user in allManagers)
                {

                    var userIn = AllUsers.Where(x => x.AssignedManager == user.Id && x.IsManager == false);
                    var userId = userIn.Select(x => x.Id).ToList();
                    //foreach (var users in userIn)
                    //{
                    counts = new List<int>();
                    report = new ReportData();
                    foreach (var date in dates)
                    {
                        //var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == users.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();
                        //counts.Add(CompletedObjections);
                        var dateScore = allScore.Where(x => userId.Contains(x.appUser.Id) && x.TimeStamp.Value.Date == date.Date).Select(o => o.TotalScore).Average();
                        counts.Add(Convert.ToInt32(dateScore));
                    }

                    report.name = user.Name;
                    report.data = counts;
                    finalList.Add(report);
                    //}

                }
            }

            return Json(new { completed = finalList, completedDates = dateString });
        }
        #endregion
        #region Tutorial
        public async Task<IActionResult> Tutorial()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dashboardModel = new DashboardIndexViewModel();
            dashboardModel.currentStep = user.currentStep;
            return View(dashboardModel);
        }
        #endregion
        #region GetStep
        public async Task<IActionResult> CheckStep()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return Json(new { step = user.currentStep, path = user.path });
        }
        #endregion
        #region Delete Dummy Data
        public async Task<ActionResult> Delete()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            DummyDataViewModel dummyData = new DummyDataViewModel();
            dummyData.collection = _repository.CollectionRepository.GetAllCollections(user.Id).ToList();
            dummyData.objection = _repository.ObjectionRepository.GetAllObjectionsByAdmin(user.Id).ToList();
            dummyData.cCS = _repository.CCSRepository.GetScorebyObjectionbyUser(user.Id).ToList(); ;
            dummyData.objectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyUser(user.Id).ToList();
            dummyData.quickCollection = _repository.QuickCollectionRepository.GetAllCollectionbyAdmin(user.Id).ToList();
            dummyData.startObjections = _repository.QuickObjectionRepository.GetAllObjectionbyUser(user.Id);
            dummyData.appUsers = _repository.UserRepository.GetUsersByAdmin(user.Id).ToList();

            //var dummyData = _dummyDataService.DeleteData(user.Id);
            return PartialView(dummyData);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string[] dataArray)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (dataArray.Length != 0)
            {
                for (int i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] == "qCollection")
                    {
                        var qcollection = _repository.QuickCollectionRepository.GetAllCollectionbyAdmin(user.Id);
                        var qColId = qcollection.Select(x => x.Id).ToList();
                        if (qcollection != null)
                        {
                            var qObjection = _repository.QuickObjectionRepository.GetAllObjectionbyCollectionIds(qColId);
                            if (qObjection.Count() > 0)
                            {
                                foreach (var qobj in qObjection)
                                {
                                    _repository.ParasaleRepository.Delete(qobj);

                                }
                                await _repository.ParasaleRepository.Save();
                            }
                            _repository.ParasaleRepository.Delete(qcollection);
                            await _repository.ParasaleRepository.Save();

                        }

                    }
                    if (dataArray[i] == "qObjections")
                    {
                        var qobjection = _repository.QuickObjectionRepository.GetAllObjectionbyUser(user.Id);
                        if (qobjection != null)
                        {
                            _repository.ParasaleRepository.Delete(qobjection);
                            await _repository.ParasaleRepository.Save();
                        }

                    }
                    if (dataArray[i] == "collection")
                    {
                        var collection = _repository.QuickObjectionRepository.GetAllObjectionbyUser(user.Id);
                        var colIds = collection.Select(x => x.Id).ToList();
                        if (collection != null)
                        {
                            var Objection = _repository.ObjectionRepository.getObjectionsbyCollections(colIds);
                            if (Objection.Count() > 0)
                            {
                                foreach (var obj in Objection)
                                {
                                    var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyObjId(obj.Id);
                                    if (objectionLog != null)
                                    {
                                        _repository.ParasaleRepository.Delete(objectionLog);

                                    }
                                    var ccs = _repository.CCSRepository.GetScorebyObjectionId(obj.Id);
                                    if (ccs.Count() > 0)
                                    {
                                        foreach (var ccss in ccs)
                                        {
                                            _repository.ParasaleRepository.Delete(ccss);

                                        }
                                        await _repository.ParasaleRepository.Save();
                                    }
                                    _repository.ParasaleRepository.Delete(obj);
                                }
                                await _repository.ParasaleRepository.Save();
                            }

                            _repository.ParasaleRepository.Delete(collection);
                            await _repository.ParasaleRepository.Save();

                        }
                    }
                    if (dataArray[i] == "objection")
                    {
                        var Objection = _repository.ObjectionRepository.GetAllObjectionsByAdmin(user.Id).ToList();
                        if (Objection.Count() > 0)
                        {
                            foreach (var obj in Objection)
                            {
                                var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyObjId(obj.Id);
                                if (objectionLog != null)
                                {
                                    _repository.ParasaleRepository.Delete(objectionLog);

                                }
                                var ccs = _repository.CCSRepository.GetScorebyObjectionId(obj.Id);
                                if (ccs.Count() > 0)
                                {
                                    foreach (var ccss in ccs)
                                    {
                                        _repository.ParasaleRepository.Delete(ccss);

                                    }
                                    await _repository.ParasaleRepository.Save();
                                }
                                _repository.ParasaleRepository.Delete(obj);
                            }
                            await _repository.ParasaleRepository.Save();
                        }

                    }
                    if (dataArray[i] == "objectionLogs")
                    {
                        var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyUser(user.Id);
                        if (objectionLog.Count() != 0)
                        {
                            foreach (var ol in objectionLog)
                            {

                                _repository.ParasaleRepository.Delete(ol);
                            }
                            await _repository.ParasaleRepository.Save();
                        }


                    }
                    if (dataArray[i] == "ccs")
                    {
                        var ccs = _repository.CCSRepository.GetScorebyObjectionbyUser(user.Id);
                        if (ccs.Count() > 0)
                        {
                            foreach (var ccss in ccs)
                            {
                                _repository.ParasaleRepository.Delete(ccss);

                            }
                            await _repository.ParasaleRepository.Save();
                        }

                    }
                    if (dataArray[i] == "users")
                    {
                        var users = _repository.UserRepository.GetUsersByAdmin(user.Id);
                        foreach (var usr in users)
                        {
                            _repository.ParasaleRepository.Delete(user);
                        }
                        await _repository.ParasaleRepository.Save();
                    }

                }
            }

            return Json(true);

        }
        [HttpPost]
        public async Task<ActionResult> DeleteAllData()
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var a = _dummyDataService.DeleteAdminData(user.Id);
            //return Json(a);
            var qcollection = _repository.QuickCollectionRepository.GetAllCollectionbyAdmin(user.Id);
            var colIds = qcollection.Select(x => x.Id).ToList();
            var invites = _repository.InvitesRepository.GetAllInvities().Where(x => x.AssignedAdmin == user.Id).ToList();
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

            var collection = _repository.CollectionRepository.GetAdminManagerCollections(user.Id);//.Where(x => x.isDummy == true);
            if (collection.Count != 0)
            {
                foreach (var col in collection)
                {
                    var Objection = _repository.ObjectionRepository.GetAllObjectionsByAdmin(user.Id);
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
                                    var assignedCol = _repository.CollectionRepository.GetAllAdminCollections(user.Id).ToList();
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
            var auditLog = _repository.AuditLogRepository.GetAuditLogsbyAdmin(user.Id);
            if (auditLog.Count > 0)
            {
                foreach (var log in auditLog)
                {
                    _repository.ParasaleRepository.Delete(log);
                }
                await _repository.ParasaleRepository.Save();
            }
            foreach (var invite in invites)
            {
                _repository.ParasaleRepository.Delete(invite);
            }
            var dummyuser = _repository.UserRepository.GetUsersByAdmin(user.Id).Where(x => x.UserName.Contains("dummy")).FirstOrDefault();
            if (dummyuser != null)
            {
                _repository.ParasaleRepository.Delete(dummyuser);
            }
            await _repository.ParasaleRepository.Save();
            return Json(true);
        }
        #endregion
    }
}