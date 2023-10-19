using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.ViewComponents
{
    public class InvitedUsersByManagerViewComponent:ViewComponent
    {

        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public InvitedUsersByManagerViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }

        private async Task<CheckInvitesViewModel> GetItemsAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            CheckInvitesViewModel usersCheckViewModel = new CheckInvitesViewModel();
            usersCheckViewModel.invitesList = _repository.InvitesRepository.GetAllInvitiesbyManager(user.Id);
            usersCheckViewModel.appUserList = _repository.UserRepository.GetUsers(true);
            return usersCheckViewModel;

        }
    }
}
