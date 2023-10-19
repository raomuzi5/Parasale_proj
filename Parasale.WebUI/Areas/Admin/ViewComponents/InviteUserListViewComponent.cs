using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Areas.Admin.ViewComponents
{
    public class InviteUserListViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public InviteUserListViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }

        private async Task<UsersCheckViewModel> GetItemsAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UsersCheckViewModel usersCheckViewModel = new UsersCheckViewModel();
            usersCheckViewModel.invitesList = _repository.InvitesRepository.GetAllInvities(user.Id);
            usersCheckViewModel.appUserList = _repository.UserRepository.GetUsers(true);
            return usersCheckViewModel;

        }
    }
}
