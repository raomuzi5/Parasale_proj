using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using Parasale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.ViewComponents
{
    public class PushedObjectionViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper _repository;
        private readonly UserManager<AppUser> _userManager;

        public PushedObjectionViewComponent(IRepositoryWrapper repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            var result = await GetItemsAsync(name);
            return View(result);
        }

        private async Task<List<PushedObjectionsViewModel>> GetItemsAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var objections = _repository.ObjectionNotificationRepository.GetObjectionsPushedByManager(user.Id);
            List<PushedObjectionsViewModel> model = new List<PushedObjectionsViewModel>();
            string managerName = null;
            if (objections != null && objections.Count > 0)
            {
                var manager = await _userManager.FindByIdAsync(objections.FirstOrDefault().PushedByUserId);
                managerName = manager.Name;
                model = objections.Select(p => new PushedObjectionsViewModel()
                {
                    Id = Convert.ToInt32(p.Id),
                    IsPracticed = p.IsCleared,
                    ObjectionName = p.Objection.InitialObjection,
                    ObjectionTime = p.CreatedTime,
                    PushedBy = managerName
                }).ToList();
            }
            return model;
        }
    }
}
