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
    public class UserMissedObjectionsViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public UserMissedObjectionsViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }

        private async Task<List<UserListViewModel>> GetItemsAsync()
        {
            var manager = await _userManager.FindByNameAsync(User.Identity.Name);
            var users = _repository.UserRepository.GetUsersUnderManager(manager.Id);
            List<UserListViewModel> model = new List<UserListViewModel>();
            if (users != null && users.Count > 0)
            {
                foreach (var user in users)
                {
                    var missedObjections = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id, false).Select(p => p.Objection);
                    var notifiedObjections = _repository.ObjectionNotificationRepository.GetObjectionsPushedByManager(user.Id).Select(p => p.Objection);

                    if (!CheckIsAlreadyNotified<Objection>(missedObjections, notifiedObjections))
                    {
                        if (missedObjections != null && missedObjections.Count() > 0)
                        {
                            model.Add(new UserListViewModel()
                            {
                                MissedCount = missedObjections.Count(),
                                Name = user.Name,
                                Email = user.Email,
                                Username = user.UserName
                            });
                        }
                    }
                }
            }
            return model;
        }

        public bool CheckIsAlreadyNotified<T>(IEnumerable<T> missed, IEnumerable<T> notified)
        {
            return missed.All(item => notified.Contains(item));
        }
    }
}
