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
    public class TeamManagerViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public TeamManagerViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var result = await GetItemsAsync(userId);
            return View(result);
        }

        private async Task<List<UserListViewModel>> GetItemsAsync(string userId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
           // return await Task.FromResult(_userManager.GetUsersInRoleAsync(Role.User.ToString()).Result
            return _repository.UserRepository.GetUsersByAdmin(user.Id)
                .Where(p => p.IsManager == false && (string.IsNullOrWhiteSpace(p.AssignedManager) || p.AssignedManager == userId))
                .Select(p => new UserListViewModel()
                {
                    Id = p.Id,
                    Email = p.Email,
                    Name = p.Name,
                    Username = p.UserName,
                    IsAlreadyTeamMember = p.AssignedManager == null ? false : true,
                    ManagerUserId = p.AssignedManager
                }).ToList();
        }
    }
}
