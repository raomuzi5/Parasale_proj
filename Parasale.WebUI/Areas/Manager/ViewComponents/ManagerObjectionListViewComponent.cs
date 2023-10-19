using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Areas.Manager.ViewComponents
{
    public class ManagerObjectionListViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public ManagerObjectionListViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, string managerUserId)
        {
            var result = await GetItemsAsync(userId, managerUserId);
            return View(result);
        }

        private async Task<List<ObjectionViewModel>> GetItemsAsync(string userId, string managerUserId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return _repository.ObjectionRepository.GetAllbjectionsFromManager(managerUserId, user.AssginedAdmin).Select(p => new ObjectionViewModel()
            {
                Id = p.Id,
                ObjectionName = p.InitialObjection,
                ResponseKeyword = p.PitchKeyword,
                userId = userId,
                ManagerUserId = managerUserId
            }).ToList();
        }
    }
}
