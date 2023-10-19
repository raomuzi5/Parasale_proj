using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using Parasale.WebUI.Models;
using Vereyon.Web;

namespace Parasale.WebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private IRepositoryWrapper _repository;
        private IFlashMessage _flashMessage;
        private UserManager<AppUser> _userManager;
        public HomeController(UserManager<AppUser> userManager, IRepositoryWrapper repository, IFlashMessage flashMessage)
        {
            _userManager = userManager;
            _repository = repository;
            _flashMessage = flashMessage;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var LastUserHistory = _repository.UserHistory.GetLastUserHistory(user.Id);
            UserIndexViewModel model = new UserIndexViewModel()
            {
                collections = _repository.CollectionRepository.GetAllAssignedCollections(user.Id).Select(x => new SelectListItem()
                {
                    Text = x.collection.CollectionName,
                    Value = x.collection.Id.ToString()
                })
            };
            #region User Report
            model.LastUserHistory = LastUserHistory;
            model.lastVoice = user.lastVoice;
            //var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogs();
            //var allCollections = _repository.CollectionRepository.GetAllAssignedCollections(user.Id);
            //var allObjections = _repository.ObjectionRepository.GetAllbjections();

            //var totalcount = 0;
            //var totalcount2 = 0;
            //foreach (var objections in allCollections)
            //{
            //    var objection = allObjections.Where(x => x.collection.Id == objections.collection.Id && x.User.Id == user.Id);
            //    foreach (var usrs in objection)
            //    {
            //        var temp = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == usrs.User.Id).Distinct().ToList().Count();
            //        var temp2 = allObjectionLog.Where(x => x.IsCompleted == false && x.AppUser.Id == usrs.User.Id).Distinct().ToList().Count();
            //        totalcount = totalcount + temp;
            //        totalcount2 = totalcount2 + temp2;
            //    }
            //    model.IncompletedCount.Add(totalcount2);
            //    model.completedCount.Add(totalcount);
            //    model.completed.Add(objections.collection.CollectionName);
            //    //Completed.Add(objections.collection.CollectionName, totalcount);
            //    //Incompleted.Add(objections.collection.CollectionName, totalcount2);
            //}

            #endregion

            return View(model);
        }


        public IActionResult GetObjectionsList(bool IsCompleted, bool IsFromAdmin)
        {
            return ViewComponent("ObjectionLog", new { name = User.Identity.Name, isCompleted = IsCompleted, isFromAdmin = IsFromAdmin });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult TermsCondition()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region ReportData
        public class ReportData
        {
            public string name { get; set; }
            public List<int> data { get; set; }
        }

        //public async Task<IActionResult> ForReportData()
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    //ReportDataViewModel viewModel = new ReportDataViewModel();
        //    //var assigendCollection = _repository.CollectionRepository.GetAllAssignedCollections(user.Id).Select(x => new ReportDataViewModel() {
        //    //Names = x.collection.CollectionName,
        //    //Ids=x.CollectionId
        //    //});

        //    return PartialView("~/Views/Home/_ForReportData.cshtml", _repository.CollectionRepository.GetAllAssignedCollections(user.Id).Select(x => new ReportDataViewModel()
        //    {
        //        Names = x.collection.CollectionName,
        //        Ids = x.CollectionId
        //    }).ToList());

        //}

        public async Task<IActionResult> getReportData(string startDate, string endDate)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);
            var userr = await _userManager.FindByNameAsync(User.Identity.Name);
            var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyUser(userr.Id).Where(x => x.ObjectionTime.Date >= sDate.Date && x.ObjectionTime.Date <= eDate.Date);
            var allObjections = _repository.ObjectionRepository.GetAllbjections();
            var assignedCollections = _repository.CollectionRepository.GetAllAssignedCollections(userr.Id);
            List<AssignedCollection> collections1 = new List<AssignedCollection>();
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
            var TempObjections = allObjections.Where(x => x.collection != null);
            foreach (var collect in assignedCollections)
            {
                var obj = allObjections.Where(x => x.collection.Id == collect.collection.Id).ToList();
                var objId = obj.Select(x => x.Id);
                if (obj != null)
                {
                    counts = new List<int>();
                    report = new ReportData();
                    foreach (var date in dates)
                    {
                        var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == userr.Id && objId.Contains(x.Objection.Id) && x.ObjectionTime.Date == date.Date).Distinct().Count();
                        counts.Add(CompletedObjections);
                    }
                    report.name = collect.collection.CollectionName;
                    report.data = counts;
                    finalList.Add(report);
                }
            }
            return Json(new { completed = finalList, completedDates = dateString });
        }


        //[HttpPost]
        //public async Task<IActionResult> GetSelectedData([FromForm] List<string> models, string type, string startDate, string endDate)
        //{
        //    DateTime sDate = Convert.ToDateTime(startDate);
        //    DateTime eDate = Convert.ToDateTime(endDate);
        //    var splited = models[0].Split(',');
        //    var userr = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogs(userr.Id).Where(x => x.ObjectionTime >= sDate && x.ObjectionTime <= eDate);

        //    List<AppUser> appUsers = new List<AppUser>();
        //    List<AssignedCollection> collections1 = new List<AssignedCollection>();
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
        //        if (type == "byCollection")
        //        {
        //            //var allCollections = _repository.CollectionRepository.GetAllCollections(userr.Id);
        //            var allObjections = _repository.ObjectionRepository.GetAllbjections();
        //            var assignedCollections = _repository.CollectionRepository.GetAllAssignedCollections(userr.Id);
        //            var TempObjections = allObjections.Where(x => x.collection != null);
        //            foreach (var collection in splited)
        //            {
        //                var collections = assignedCollections.Where(x => x.CollectionId == Convert.ToInt32(collection) && x.appUser.Id == userr.Id).ToList();
        //                collections1.Add(collections[0]);
        //            }
        //            foreach (var collect in collections1)
        //            {

        //                var obj = TempObjections.FirstOrDefault(x => x.collection.Id == collect.Id);
        //                if (obj != null)
        //                {
        //                    counts = new List<int>();
        //                    report = new ReportData();
        //                    foreach (var date in dates)
        //                    {
        //                        var CompletedObjections = allObjectionLog.Where(x => x.IsCompleted == true && x.AppUser.Id == userr.Id && x.Objection.Id == obj.Id && x.ObjectionTime.ToShortDateString() == date.ToShortDateString()).Distinct().Count();
        //                        counts.Add(CompletedObjections);
        //                    }
        //                    report.name = collect.collection.CollectionName;
        //                    report.data = counts;
        //                    finalList.Add(report);
        //                }
        //            }
        //        }
        //    }
        //    return Json(new { completed = finalList, completedDates = dateString });

        //}
        #endregion

        #region CCS
        public async Task<IActionResult> getCCSReportData(string type, string startDate, string endDate)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);
            var userr = await _userManager.FindByNameAsync(User.Identity.Name);


            var allScore = _repository.CCSRepository.GetScorebyObjectionbyUser(userr.Id).Where(x => x.TimeStamp.Value.Date >= sDate.Date && x.TimeStamp.Value.Date <= eDate.Date);
            var allObjections = _repository.ObjectionRepository.GetAllbjections();
            var assignedCollections = _repository.CollectionRepository.GetAllAssignedCollections(userr.Id);

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
            var TempObjections = allObjections.Where(x => x.collection != null);
            if (type == "byObjection")
            {
                var collectionId = assignedCollections.Select(x => x.CollectionId).ToList();
                var assignedObjections = TempObjections.Where(x => collectionId.Contains(x.collection.Id));
                foreach (var objection in assignedObjections)
                {
                    if (objection != null)
                    {
                        counts = new List<int>();
                        report = new ReportData();
                        foreach (var date in dates)
                        {
                            var dateScore = allScore.Where(x => x.appUser.Id == userr.Id && x.objection.Id == objection.Id && x.TimeStamp.Value.Date == date.Date).Select(o => o.TotalScore).Average();
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
            }
            if (type == "byCollection")
            {

                var collectionId = assignedCollections.Select(x => x.CollectionId).ToList();
                //var assignedObjections = TempObjections.Where(x => collectionId.Contains(x.collection.Id));
                foreach (var collection in assignedCollections)
                {
                    //var obj = TempObjections.Where(x => x.collection.Id == collection.Id).ToList();
                    var obj = TempObjections.Where(x => collectionId.Contains(x.collection.Id));
                    var objId = obj.Select(x => x.Id).ToList();
                    if (obj != null)
                    {
                        counts = new List<int>();
                        report = new ReportData();
                        foreach (var date in dates)
                        {
                            var dateScore = allScore.Where(x => x.appUser.Id == userr.Id && objId.Contains(x.objection.Id) && x.TimeStamp.Value.Date == date.Date).Select(o => o.TotalScore).Average();
                            counts.Add(Convert.ToInt32(dateScore));
                        }

                        report.name = collection.collection.CollectionName;
                        report.data = counts;
                        finalList.Add(report);
                    }
                }
            }
            return Json(new { completed = finalList, completedDates = dateString });
        }

        #endregion

        
        #region Firefox
        public async Task<IActionResult> MozilaFF()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserIndexViewModel model = new UserIndexViewModel()
            {
                collections = _repository.CollectionRepository.GetAllAssignedCollections(user.Id).Select(x => new SelectListItem()
                {
                    Text = x.collection.CollectionName,
                    Value = x.collection.Id.ToString()
                })
            };
            return View(model);
        }

        public async Task<IActionResult> GoToLinks()
        {
            return View();
        }

        public async Task<IActionResult> TestFF()
        {
            return View();
        }


        public IActionResult TestIOS()
        {
            return View();
        }
        #endregion
    }
}
