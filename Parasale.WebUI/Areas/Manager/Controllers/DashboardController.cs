using System;
using System.Collections.Generic;
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
using Parasale.WebUI.Areas.Manager.Models;
using Parasale.WebUI.Models;
using Parasale.WebUI.Services;
using Vereyon.Web;

namespace Parasale.WebUI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class DashboardController : Controller
    {
        private IRepositoryWrapper _repository;
        private IFlashMessage _flashMessage;
        private UserManager<AppUser> _userManager;
        private EmailService _emailServices;
        private DummyDataService _dummyDataService;

        public DashboardController(IRepositoryWrapper repository, IFlashMessage flashMessage, EmailService emailService, UserManager<AppUser> userManager, DummyDataService dummyDataService)
        {
            _repository = repository;
            _flashMessage = flashMessage;
            _userManager = userManager;
            _emailServices = emailService;
            _dummyDataService = dummyDataService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dashboardModel = new DashboardIndexViewModel();
            var collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin);
            foreach (var item in collections)
            {
                CollectionListViewModel collection = new CollectionListViewModel();
                collection.Id = item.Id;
                collection.CollectionName = item.CollectionName;
                if (user.Id != item.appUser.Id)
                {
                    collection.IsAdmin = true;
                }
                dashboardModel.collections.Add(collection);
                //dashboardModel.collections.Add(new CollectionListViewModel()
                //{
                //    Id = item.Id,
                //    CollectionName = item.CollectionName,

                //});
            }

            var objectionslogs = _repository.ObjectionLogRepository.GetObjectionLogs();

            var ActiveUsers = objectionslogs.Select(p => p.AppUser).Where(p => p.AssignedManager == user.Id).Distinct();

            var getManagerUsers = _repository.UserRepository.GetUsersUnderManager(user.Id);
            var userIds = getManagerUsers.Select(x => x.Id).ToList();
            var tLogs = objectionslogs.Where(x => userIds.Contains(x.AppUser.Id)).Count();
            var missedObj = objectionslogs.Where(x => userIds.Contains(x.AppUser.Id) && x.IsCompleted == false).Count(); //_repository.ObjectionLogRepository.GetObjectionLogsbyUser(userIds, false).Count();
            Double objMissed = (Double)missedObj / tLogs;
            var missed = (Double)objMissed * 100;
            var completedObj = objectionslogs.Where(x => userIds.Contains(x.AppUser.Id) && x.IsCompleted == true).Count();//_repository.ObjectionLogRepository.GetObjectionLogsbyUser(userIds, true).Count();
            Double objCompleted = (Double)completedObj / tLogs;

            var completed = (Double)objCompleted * 100;

            dashboardModel.TotalActiveUsers = ActiveUsers.Count();

            var missedObjectionPercentage = double.IsNaN(missed) ? 0 : missed;
            var completedObjectionPercentage = double.IsNaN(completed) ? 0 : completed;

            dashboardModel.Users = _repository.UserRepository.GetUsersUnderManager(user.Id).Count;
            // dashboardModel.TotalObjections = _repository.ObjectionRepository.GetAllbjectionsFromManager(user.Id).Count;
            dashboardModel.MissedObjections = missedObjectionPercentage;
            dashboardModel.CompletedObjections = completedObjectionPercentage;
            dashboardModel.voiceboarding = user.voiceOnboarding;
            dashboardModel.currentStep = user.currentStep;
            return View(dashboardModel);
        }

        public async Task<IActionResult> ManageCollections(string after)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var manageCollections = new ManageCollectionsViewModel();
            var collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin);
            foreach (var item in collections)
            {
                CollectionListViewModel collection = new CollectionListViewModel();
                collection.Id = item.Id;
                collection.CollectionName = item.CollectionName;
                if (user.Id != item.appUser.Id)
                {
                    collection.IsAdmin = true;
                }
                manageCollections.collections.Add(collection);
               
            }
            manageCollections.voiceBoarding = user.voiceOnboarding;
            manageCollections.userName = user.UserName;
            manageCollections.currentStep = user.currentStep;
            manageCollections.StepLevel = user.StepLevel;
            manageCollections.path = user.path;
            var qcollections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin).Select(o => o.CollectionName).ToList();
            var quickCollections = _repository.QuickCollectionRepository.GetAllCollections().Where(x => x.QuickStart == true && !qcollections.Contains(x.CollectionName)).ToList();
            manageCollections.QuickCollection =quickCollections;
            if (after != null)
            {
                return PartialView(manageCollections);
            }
            else
            {
                return View(manageCollections);
            }
           // return View(manageCollections);

        }
        public async Task<IActionResult> ManageTeamsAndUsers()
        {
            InvitesByManagerViewModel model = new InvitesByManagerViewModel();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.voiceboarding = user.voiceOnboarding;
            var objections = _repository.ObjectionLogRepository.GetObjectionLogs();

            var users = _repository.UserRepository.GetUsersUnderManager(user.Id).Select(p => new Teams()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                MissedCount = objections.Where(q => q.AppUser.Id == p.Id && q.IsCompleted == false).Count(),
                CompletedCount = objections.Where(q => q.AppUser.Id == p.Id && q.IsCompleted == true).Count(),
                IsManager = p.IsManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true
            }).ToList();
            model.teams = users;
            model.currentStep = user.currentStep;
            return View(model);
        }
        public async Task<IActionResult> Reports()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dashboardModel = new DashboardIndexViewModel();
            var collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin);
            foreach (var item in collections)
            {
                CollectionListViewModel collection = new CollectionListViewModel();
                collection.Id = item.Id;
                collection.CollectionName = item.CollectionName;
                if (user.Id != item.appUser.Id)
                {
                    collection.IsAdmin = true;
                }
                dashboardModel.collections.Add(collection);
                //dashboardModel.collections.Add(new CollectionListViewModel()
                //{
                //    Id = item.Id,
                //    CollectionName = item.CollectionName,

                //});
            }

            var objectionslogs = _repository.ObjectionLogRepository.GetObjectionLogs();

            var ActiveUsers = objectionslogs.Select(p => p.AppUser).Where(p => p.AssignedManager == user.Id).Distinct();

            var getManagerUsers = _repository.UserRepository.GetUsersUnderManager(user.Id);
            var userIds = getManagerUsers.Select(x => x.Id).ToList();
            var tLogs = objectionslogs.Where(x => userIds.Contains(x.AppUser.Id)).Count();
            var missedObj = objectionslogs.Where(x => userIds.Contains(x.AppUser.Id) && x.IsCompleted == false).Count(); //_repository.ObjectionLogRepository.GetObjectionLogsbyUser(userIds, false).Count();
            Double objMissed = (Double)missedObj / tLogs;
            var missed = (Double)objMissed * 100;
            var completedObj = objectionslogs.Where(x => userIds.Contains(x.AppUser.Id) && x.IsCompleted == true).Count();//_repository.ObjectionLogRepository.GetObjectionLogsbyUser(userIds, true).Count();
            Double objCompleted = (Double)completedObj / tLogs;

            var completed = (Double)objCompleted * 100;

            dashboardModel.TotalActiveUsers = ActiveUsers.Count();

            var missedObjectionPercentage = double.IsNaN(missed) ? 0 : missed;
            var completedObjectionPercentage = double.IsNaN(completed) ? 0 : completed;

            dashboardModel.Users = _repository.UserRepository.GetUsersUnderManager(user.Id).Count;
            // dashboardModel.TotalObjections = _repository.ObjectionRepository.GetAllbjectionsFromManager(user.Id).Count;
            dashboardModel.MissedObjections = missedObjectionPercentage;
            dashboardModel.CompletedObjections = completedObjectionPercentage;
            dashboardModel.voiceboarding = user.voiceOnboarding;
            dashboardModel.currentStep = user.currentStep;
            return View(dashboardModel);
        }
        public async Task<IActionResult> ResetAccount()
        {
            return View();
        }
        public async Task<IActionResult> AddObjection(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return PartialView("~/Areas/Manager/Views/Dashboard/_AddObjection.cshtml", new ManagerObjectionAddViewModel()
            {
                collectionId = id,
                collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin).Select(p => new SelectListItem()
                {
                    Text = p.CollectionName,
                    Value = p.Id.ToString()
                }).ToList()


            });
        }


        public IActionResult GetObjectionsByCollection(int id)
        {
            return ViewComponent("ObjectionsbyCollection", new { id = id });
        }

        public async Task<IActionResult> GetAllObjections()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var objections = _repository.ObjectionRepository.GetAllbjectionsFromManager(user.Id, user.AssginedAdmin);
            var dashboardModel = new DashboardIndexViewModel();
            foreach (var item in objections)
            {
                dashboardModel.Objections.Add(new ObjectionViewModel()
                {
                    Id = item.Id,
                    ObjectionName = item.InitialObjection,
                    ResponseKeyword = item.PitchKeyword
                });
            }
            return View(dashboardModel);
        }
        [HttpPost]
        // public async Task<IActionResult> AddObjection(ManagerObjectionAddViewModel model)
        public async Task<IActionResult> AddObjection(string objectionName, string ResponseKeyword, int collectionId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                string keywordResponse = null;
                var responses = ResponseKeyword.Trim().Split(',');
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
                    InitialObjection = objectionName,
                    PitchKeyword = keywordResponse,
                    ValidPitchResponse = "good job",
                    BadPitchResponse = "try again",
                    User = user,
                    AssignedAdmin = user.AssginedAdmin,
                    collection = _repository.CollectionRepository.GetAllCollectionbyId(collectionId)
                });

                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = objectionName,
                    Type = "Created",
                    Field = "Objection created",
                    ModifiedBy = User.Identity.Name,
                    ManagerId = user.Id,
                    AdminId = user.AssginedAdmin,
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
            //return RedirectToAction("Index", new { area = "Manager", controller = "Dashboard" });
            return Json(new { success = true });
        }

        public async Task<IActionResult> EditObjection(int id)
        {
            var objection = _repository.ObjectionRepository.GetObjection(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return PartialView("~/Areas/Manager/Views/Dashboard/_EditObjection.cshtml", new ManagerObjectionAddViewModel()
            {
                Id = objection.Id,
                ObjectionName = objection.InitialObjection,
                ResponseKeyword = objection.PitchKeyword,
                collectionId = objection.collection.Id,
                collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin).Select(p => new SelectListItem()
                {
                    Text = p.CollectionName,
                    Value = p.Id.ToString()
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditObjection(ManagerObjectionAddViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ModelState.IsValid)
            {
                string keywordResponse = null;
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
                        ManagerId = user.Id,
                        UserId = user.Id,
                        AdminId = user.AssginedAdmin,
                        ModifiedDate = DateTime.Now
                    });
                }


                if (objection.PitchKeyword != keywordResponse)
                {
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = objection.PitchKeyword,
                        NewData = keywordResponse,
                        Type = "Updated",
                        Field = "Objection Response Keyword updated",
                        ManagerId = user.Id,
                        UserId = user.Id,
                        AdminId = user.AssginedAdmin,
                        ModifiedBy = User.Identity.Name,
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
                        ManagerId = user.Id,
                        UserId = user.Id,
                        AdminId = user.AssginedAdmin,
                        ModifiedDate = DateTime.Now
                    });
                }

                objection.InitialObjection = model.ObjectionName;
                objection.PitchKeyword = keywordResponse;
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
            //return Json(new { success = true });
            return RedirectToAction("ManageCollections", new { area = "Manager", controller = "Dashboard" });
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
                //     await _repository.ParasaleRepository.Save();
            }
            _repository.ParasaleRepository.Delete(objection);


            await _repository.ParasaleRepository.Add(new AuditLog()
            {
                NewData = objection.InitialObjection,
                Type = "Deleted",
                Field = "Objection deleted",
                ModifiedBy = User.Identity.Name,
                ManagerId = user.Id,
                AdminId = user.AssginedAdmin,
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
            return RedirectToAction("ManageCollections", new { area = "Manager", controller = "Dashboard" });
        }

        public async Task<IActionResult> ObjectionsList(string UserId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return ViewComponent("ManagerObjectionList", new { userId = UserId, managerUserId = user.Id });
        }

        public async Task<IActionResult> CollectionList(string UserId)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return ViewComponent("ManagerCollectionsList", new { userId = UserId });//, managerUserId = user.Id
        }


        public async Task<IActionResult> AuditLog()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.voiceboarding = user.voiceOnboarding;
            return View(_repository.ObjectionLogRepository.AuditLogs(user.Id, user.AssginedAdmin));
        }

        public async Task<IActionResult> PracticeObjections()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var LastUserHistory = _repository.UserHistory.GetLastUserHistory(user.Id);
            UserIndexViewModel model = new UserIndexViewModel()
            {
                collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin).Select(x => new SelectListItem()
                {
                    Text = x.CollectionName,
                    Value = x.Id.ToString()
                })
            };
            model.VoiceBoarding = user.voiceOnboarding;
            model.LastUserHistory = LastUserHistory;
            return View(model);
        }
        #region ReportData
        public class ReportData
        {
            public string name { get; set; }
            public List<int> data { get; set; }
        }


        #region commented code
        //public async Task<IActionResult> ForReportData(string type)
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    ForReportDataViewModel viewModel = new ForReportDataViewModel();
        //    viewModel.type = type;
        //    if (type == "byUser")
        //    {
        //        viewModel.appUserList = _repository.UserRepository.GetUsersUnderManager(user.Id);
        //        return PartialView("~/Areas/Manager/Views/Dashboard/_ForReportData.cshtml", viewModel);
        //    }


        //    if (type == "byCollection")
        //    {
        //        viewModel.collectionList = _repository.CollectionRepository.GetAllCollections(user.Id);
        //        return PartialView("~/Areas/Manager/Views/Dashboard/_ForReportData.cshtml", viewModel);

        //    }
        //    return PartialView("~/Areas/Manager/Views/Dashboard/_ForReportData.cshtml");
        //}

        //[HttpPost]
        //public async Task<IActionResult> GetSelectedData([FromForm] List<string> models, string type, string startDate, string endDate)
        //{
        //    DateTime sDate = Convert.ToDateTime(startDate);
        //    DateTime eDate = Convert.ToDateTime(endDate);
        //    var splited = models[0].Split(',');
        //    var userr = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var AllUsers = _repository.UserRepository.GetUsersUnderManager(userr.Id);
        //    var userIds = AllUsers.Select(x => x.Id).ToList();
        //    var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyManager(userIds).Where(x => x.ObjectionTime.Date >= sDate.Date && x.ObjectionTime.Date <= eDate.Date);

        //    List<AppUser> appUsers = new List<AppUser>();
        //    List<Collection> collections1 = new List<Collection>();
        //    ReportData report = new ReportData();
        //    var dates = new List<DateTime>();
        //    var dateString = new List<string>();
        //    for (var dt = sDate; dt <= eDate; dt = dt.AddDays(1))
        //    {
        //        dates.Add(dt);
        //        dateString.Add(dt.ToShortDateString());
        //    }
        //    List<ReportData> finalList = new List<ReportData>();

        //    List<int> counts = new List<int>();
        //    if (models.Count() != 0)
        //    {
        //        //if (type == "byUser")
        //        //{
        //        //    foreach (var users in splited)
        //        //    {
        //        //        var userss = AllUsers.Where(x => x.Id == users && x.IsManager == false && x.IsCompanyAdmin == false).ToList();
        //        //        if (userss.Count != 0)
        //        //        {
        //        //            appUsers.Add(userss[0]);
        //        //        }
        //        //    }


        //        //    foreach (var user in appUsers)
        //        //    {
        //        //        counts = new List<int>();
        //        //        report = new ReportData();
        //        //        foreach (var date in dates)
        //        //        {
        //        //            var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == user.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();

        //        //            counts.Add(CompletedObjections);
        //        //        }

        //        //        report.name = user.Name;
        //        //        report.data = counts;

        //        //        finalList.Add(report);

        //        //    }
        //        //}


        //        if (type == "byCollection")
        //        {
        //            var allCollections = _repository.CollectionRepository.GetAllCollections(userr.Id);
        //            var allObjections = _repository.ObjectionRepository.GetAllbjectionsFromManager(userr.Id);
        //            var TempObjections = allObjections.Where(x => x.collection != null);
        //            //foreach (var collection in splited)
        //            //{
        //            //    var collections = allCollections.Where(x => x.Id == Convert.ToInt32(collection) && x.appUser.Id == userr.Id).ToList();
        //            //    collections1.Add(collections[0]);
        //            //}
        //            foreach (var collect in collections1)
        //            {

        //                var obj = TempObjections.FirstOrDefault(x => x.collection.Id == collect.Id);
        //                if (obj != null)
        //                {
        //                    counts = new List<int>();
        //                    report = new ReportData();
        //                    foreach (var date in dates)
        //                    {
        //                        var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.Objection.Id == obj.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();
        //                        counts.Add(CompletedObjections);
        //                    }
        //                    report.name = collect.CollectionName;
        //                    report.data = counts;
        //                    finalList.Add(report);
        //                }
        //            }
        //        }
        //    }
        //    return Json(new { completed = finalList, completedDates = dateString });

        //}
        #endregion

        public async Task<IActionResult> getReportData(string type, string startDate, string endDate)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);
            var userr = await _userManager.FindByNameAsync(User.Identity.Name);
            var AllUsers = _repository.UserRepository.GetUsersUnderManager(userr.Id);
            var userIds = AllUsers.Select(x => x.Id).ToList();
            var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyManager(userIds).Where(x => x.ObjectionTime.Date >= sDate.Date && x.ObjectionTime.Date <= eDate.Date);
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
                        var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == user.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();

                        counts.Add(CompletedObjections);
                    }

                    report.name = user.Name;
                    report.data = counts;

                    finalList.Add(report);

                }
            }
            if (type == "byCollection")
            {
                var allCollections = _repository.CollectionRepository.GetAllCollections(userr.Id);
                var allObjections = _repository.ObjectionRepository.GetAllbjectionsFromManager(userr.Id);
                var TempObjections = allObjections.Where(x => x.collection != null);

                foreach (var collect in allCollections)
                {

                    var obj = TempObjections.Where(x => x.collection.Id == collect.Id);
                    var objId = obj.Select(x => x.Id).ToList();
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


        #endregion

        public async Task<IActionResult> ManagerCollections(string after)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var collections = _repository.CollectionRepository.GetAllCollections(user.Id, user.AssginedAdmin).Select(o => o.CollectionName).ToList();
           // ViewBag.voiceboarding = user.voiceOnboarding;
            var quickCollections = _repository.QuickCollectionRepository.GetAllCollections().Where(x => x.QuickStart == true && !collections.Contains(x.CollectionName)).ToList();
            
            if (after != null)
            {
                return PartialView(quickCollections);
            }
            else
            {
                return View(quickCollections);
            }
            //return View(_repository.QuickCollectionRepository.GetAllCollections().Where(x => x.QuickStart == true));
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
                collection.AssignedAdmin = user.AssginedAdmin;
                collection.CollectionName = quickCollection.CollectionName;
                await _repository.ParasaleRepository.Add(collection);
                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = collection.CollectionName,
                    Type = "Moved From Quick",
                    Field = "Quick Start Collection",
                    ModifiedBy = User.Identity.Name,
                    ManagerId = user.Id,
                    AdminId = user.AssginedAdmin,
                    UserId = user.Id,
                    ModifiedDate = DateTime.Now
                });
                await _repository.ParasaleRepository.Save();
                if (quickObjections != null)
                {
                    foreach (var objections in quickObjections)
                    {
                        Objection objection = new Objection();
                        objection.AssignedAdmin = user.AssginedAdmin;
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
            return RedirectToAction("ManagerCollections");
        }
        #region commented

        //public async Task<IActionResult> UnassignedObjectionsList()
        //{
        //    return View();

        //}

        //public async Task<IActionResult> PushMissedObjectionsToUser(string username)
        //{
        //    var user = await _userManager.FindByNameAsync(username);
        //    var manager = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var missedObjections = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id, false);

        //    foreach (var model in missedObjections)
        //    {
        //        await _repository.ParasaleRepository.Add(new ObjectionNotification()
        //        {
        //            IsCleared = false,
        //            PushedByUserId = manager.Id,
        //            PushedToUserId = user.Id,
        //            Objection = _repository.ObjectionRepository.GetObjection(model.Objection.Id),
        //            CreatedTime = DateTime.Now
        //        });


        //    }

        //    await _repository.ParasaleRepository.Save();
        //    return Json(true);
        //}

        //public IActionResult Reports()
        //{
        //    return View();
        //}

        //public IActionResult getReportData(string type, string startDate, string endDate)
        //{
        //    DateTime sDate = Convert.ToDateTime(startDate);
        //    DateTime eDate = Convert.ToDateTime(endDate);

        //    var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogs().Where(x => x.ObjectionTime >= sDate && x.ObjectionTime <= eDate);
        //    var allManagers = _repository.UserRepository.GetManagers();
        //    var AllUsers = _repository.UserRepository.GetUsers();
        //    Dictionary<string, int> Completed = new Dictionary<string, int>();
        //    Dictionary<string, int> Incompleted = new Dictionary<string, int>();
        //    List<AppUser> appUsers = new List<AppUser>();
        //    if (type == "byUser")
        //    {

        //        foreach (var user in AllUsers)
        //        {

        //            var temp = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == user.Id).Distinct().ToList().Count();
        //            var temp2 = allObjectionLog.Where(x => x.IsCompleted == false && x.AppUser.Id == user.Id).Distinct().ToList().Count();
        //            Completed.Add(user.UserName, temp);
        //            Incompleted.Add(user.UserName, temp2);
        //        }
        //    }

        //    if (type == "byCollection")
        //    {
        //        var allCollections = _repository.CollectionRepository.GetAllCollections();
        //        var allObjections = _repository.ObjectionRepository.GetAllbjections();
        //        foreach (var objections in allCollections)
        //        {
        //            var obj = allObjections.FirstOrDefault(x => x.collection.Id == objections.Id);
        //            if (obj != null)
        //            {
        //                var temp = allObjectionLog.Where(x => x.IsCompleted == true && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //                var temp2 = allObjectionLog.Where(x => x.IsCompleted == false && x.Objection.Id == obj.Id).Distinct().ToList().Count();
        //                Completed.Add(objections.CollectionName, temp);
        //                Incompleted.Add(objections.CollectionName, temp2);

        //            }
        //        }
        //    }
        //    return Json(new { completed = Completed, incomplete = Incompleted });
        //}

        #endregion


        #region CCS Report For Manager
        public async Task<IActionResult> getCCSReportData(string type, string startDate, string endDate)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);
            var userr = await _userManager.FindByNameAsync(User.Identity.Name);
            var AllUsers = _repository.UserRepository.GetUsersUnderManager(userr.Id);
            var userIds = AllUsers.Select(x => x.Id).ToList();
            var allObjections = _repository.ObjectionRepository.GetAllbjectionsFromManager(userr.Id, userr.AssginedAdmin);
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
                var allCollections = _repository.CollectionRepository.GetAllCollections(userr.Id, userr.AssginedAdmin);
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
                var allCollections = _repository.CollectionRepository.GetAllCollections(userr.Id, userr.AssginedAdmin);
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




                        report.name = collect.CollectionName;
                        report.data = counts;
                        finalList.Add(report);
                    }
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

            return Json(new { completed = finalList, completedDates = dateString });
        }


        #endregion
        #region Inivtes by Manager
        public class userInvite
        {
            public string value { get; set; }

        }
        public async Task<IActionResult> InviteUsers()
        {
            InvitesByManagerViewModel model = new InvitesByManagerViewModel();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.voiceboarding =user.voiceOnboarding;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InviteUsers(InvitesByManagerViewModel model)
        {
            bool IsUserExist = false;

            var Emails = JsonConvert.DeserializeObject<List<userInvite>>(model.invitesByManager.Email);
            var userid = await _userManager.FindByNameAsync(User.Identity.Name);
            if (Emails.Count() > 0)
            {
                var existingUsers = _repository.InvitesRepository.GetAllInvitiesbyManager(userid.Id);
                foreach (var email in Emails)
                {
                    foreach (var exUsers in existingUsers)
                    {
                        if (exUsers.Email == email.value)
                        {
                            IsUserExist = true;
                            SendInvitations(exUsers.Email, exUsers.Guid, exUsers.AssignedManager);
                            await _repository.ParasaleRepository.Add(new AuditLog()
                            {
                                NewData = email.value,
                                Type = "Resent Invitation ",
                                Field = userid.Name + "Re-Invited" + email.value,
                                ModifiedBy = User.Identity.Name,
                                ManagerId = userid.Id,
                                UserId = userid.Id,
                                AdminId = userid.AssginedAdmin,
                                ModifiedDate = DateTime.Now
                            });
                        }

                    }

                    if (!IsUserExist)
                    {
                        var guid = Guid.NewGuid().ToString();

                        await _repository.ParasaleRepository.Add(new InvitesByManager()
                        {
                            Email = email.value,
                            IsSigned = false,
                            Guid = guid,
                            AssignedManager = userid.Id,
                            AssignedAdmin = userid.AssginedAdmin,
                            //Link = Url.Action("Register", "Account", new { token = guid, value=userid.Id} )
                            //Link = "https://parasale.co/Account/Register?token=" + guid + "&value=" + userid.Id
                            Link = Url.Action("Register", "Account", new { area = "", token = guid, value = userid.Id }, protocol: HttpContext.Request.Scheme)
                        });
                        await _repository.ParasaleRepository.Add(new AuditLog()
                        {
                            NewData = email.value,
                            Type = "Invitation from",
                            Field = userid.Name + " has sent signup Invitation to " + email.value,
                            ModifiedBy = User.Identity.Name,
                            ModifiedDate = DateTime.Now
                        });
                        SendInvitations(email.value, guid, userid.Id);
                        await _repository.ParasaleRepository.Save();
                    }


                }
            }
            model.success = true;
            return View(model);
        }

        public void SendInvitations(string email, string guid, string userId)
        {
            //var link = "https://parasale.co/Account/Register?token=" + guid + "&value=" + userId;
            var link = Url.Action("Register", "Account", new { area = "", token = guid, value = userId }, protocol: HttpContext.Request.Scheme);
            string subject = "Sign up Invitation";
            string body = $"You are invited for sign up. Please <a href='{link}'>click here</a> to proceed.";
            _emailServices.SendMail(email, subject, body);
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
            return Json(user.currentStep);
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
            dummyData.appUsers = _repository.UserRepository.GetUsersUnderManager(user.Id).ToList();
            //dummyData.collection = _repository.CollectionRepository.GetAllCollections(user.Id).Where(x => x.isDummy == true).FirstOrDefault();
            //dummyData.objection = _repository.ObjectionRepository.GetAllObjectionsByAdmin(user.Id).Where(x => x.isDummy == true).FirstOrDefault();
            //dummyData.cCS = _repository.CCSRepository.GetScorebyObjectionbyUser(user.Id).Where(x => x.isDummy == true).FirstOrDefault(); ;
            //dummyData.objectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyUser(user.Id).Where(x => x.isDummy == true).FirstOrDefault();
            //dummyData.quickCollection = _repository.QuickCollectionRepository.GetAllCollectionbyAdmin(user.Id);
            //dummyData.startObjections = _repository.QuickObjectionRepository.GetAllObjectionbyUser(user.Id);
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
                        var colIds = collection.Select(x => x.Id).ToList() ;
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
                        var Objection = _repository.ObjectionRepository.GetAllbjectionsFromManager(user.Id).ToList();
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
                        var users = _repository.UserRepository.GetUsersUnderManager(user.Id);

                        foreach (var usr in users)
                        {
                            _repository.ParasaleRepository.Delete(usr);
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
            var qcollection = _repository.QuickCollectionRepository.GetAllCollectionbyAdmin(user.Id);
            var colIds = qcollection.Select(x => x.Id).ToList();
            if (qcollection.Count()>0)
            {

                var qObjection = _repository.QuickObjectionRepository.GetAllObjectionbyCollectionIds(colIds);
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
            var auditLog = _repository.AuditLogRepository.GetAuditLogsbyManager(user.Id);
            if (auditLog.Count > 0)
            {
                foreach (var log in auditLog)
                {
                    _repository.ParasaleRepository.Delete(log);
                }
                await _repository.ParasaleRepository.Save();
            }

            var collection = _repository.CollectionRepository.GetAdminManagerCollections(user.AssginedAdmin);//.Where(x => x.isDummy == true);
            if (collection.Count>0)
            {
                foreach (var col in collection)
                {
                    var Objection = _repository.ObjectionRepository.getObjectionsbyCollection(col.Id);
                    if (Objection.Count() > 0)
                    {
                        foreach (var obj in Objection)
                        {
                            try
                            {
                                var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogs(obj.Id);
                                var ccs = _repository.CCSRepository.GetScorebyObjectionId(obj.Id);
                                var sessions = _repository.SessionRepository.GetSessionObjection(obj.Id);
                                var objectionNotification = _repository.ObjectionNotificationRepository.GetObjectionsPushedByManagers(user.Id);
                                if (objectionLog.Count() > 0)
                                {
                                    foreach (var log in objectionLog)
                                    {
                                        _repository.ParasaleRepository.Delete(log);
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
                                if (objectionNotification.Count() > 0)
                                {
                                    foreach (var oN in objectionNotification)
                                    {

                                    _repository.ParasaleRepository.Delete(oN);
                                    }
                                    await _repository.ParasaleRepository.Save();
                                }
                                //_repository.ParasaleRepository.Delete(obj);
                                _repository.ParasaleRepository.Delete(obj);
                                await _repository.ParasaleRepository.Save();

                            }
                            catch(Exception ex)
                            {
                                throw ex.GetBaseException();
                            }
                            }

                        await _repository.ParasaleRepository.Save();
                    }
                    var assignedCol = _repository.CollectionRepository.GetAllAdminCollections(user.Id).ToList();
                    if (assignedCol.Count() > 0)
                    {
                        foreach (var aCol in assignedCol)
                        {
                            _repository.ParasaleRepository.Delete(aCol);
                        }
                        await _repository.ParasaleRepository.Save();
                    }
                    _repository.ParasaleRepository.Delete(col);
                    
                await _repository.ParasaleRepository.Save();
                }

            }
            var invites = _repository.InvitesRepository.GetAllInvitiesbyManager(user.Id);
            foreach (var invite in invites)
            {
                _repository.ParasaleRepository.Delete(invite);
            }
            var dummyuser = _repository.UserRepository.GetUsersByAdmin(user.Id).Where(x => x.UserName.Contains("dummy")).FirstOrDefault();
            if(dummyuser != null)
            {
                _repository.ParasaleRepository.Delete(dummyuser);
            }
            await _repository.ParasaleRepository.Save();


            return Json(true);
        }
        #endregion
    }
}