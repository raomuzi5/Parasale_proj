using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.ViewComponents
{
    public class UserReportViewComponent:ViewComponent
    {
        private IRepositoryWrapper _repository;
        private UserManager<AppUser> _userManager;

        public UserReportViewComponent(IRepositoryWrapper repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }
        private async Task<string> GetItemsAsync()
        {

            var allObjectionLog = _repository.ObjectionLogRepository.GetObjectionLogs().Where(x => x.ObjectionTime >= sDate && x.ObjectionTime <= eDate);
            var allManagers = _repository.UserRepository.GetManagers();
            var AllUsers = _repository.UserRepository.GetUsers();
            Dictionary<string, int> Completed = new Dictionary<string, int>();
            Dictionary<string, int> Incompleted = new Dictionary<string, int>();
            List<AppUser> appUsers = new List<AppUser>();
           

                var allCollections = _repository.CollectionRepository.GetAllCollections();
                var allObjections = _repository.ObjectionRepository.GetAllbjections();
                foreach (var objections in allCollections)
                {
                    var obj = allObjections.FirstOrDefault(x => x.collection.Id == objections.Id);
                    if (obj != null)
                    {
                        var temp = allObjectionLog.Where(x => x.IsCompleted == true && x.Objection.Id == obj.Id).Distinct().ToList().Count();
                        var temp2 = allObjectionLog.Where(x => x.IsCompleted == false && x.Objection.Id == obj.Id).Distinct().ToList().Count();
                        Completed.Add(objections.CollectionName, temp);
                        Incompleted.Add(objections.CollectionName, temp2);

                    }
                }
            
            return Json(new { completed = Completed, incomplete = Incompleted });


            return null;
        }



    }
}
