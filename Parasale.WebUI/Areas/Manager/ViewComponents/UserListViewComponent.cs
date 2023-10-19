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
    public class UserListViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public UserListViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, int objectionId)
        {
            var result = await GetItemsAsync(userId, objectionId);
            return View(result);
        }

        private async Task<List<UserListViewModel>> GetItemsAsync(string userId, int objectionId)
        {
            return await Task.FromResult(_repository.UserRepository.GetUsersUnderManager(userId).Select(p => new UserListViewModel()
            {

                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                objectionId = objectionId,
                ManagerUserId = userId
            }).ToList());
        }
    }
}
