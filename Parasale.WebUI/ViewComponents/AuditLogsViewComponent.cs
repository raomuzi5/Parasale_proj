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
   

    public class AuditLogsViewComponent : ViewComponent
    {
        private IRepositoryWrapper _repository;
        private UserManager<AppUser> _userManager;

        public AuditLogsViewComponent(IRepositoryWrapper repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            var result = await GetItemsAsync(name);
            return View(result);
        }

        private async Task<List<AuditLog>> GetItemsAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var objections = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id);
           
            return _repository.ObjectionLogRepository.UserAuditLogs(user.Id);
        }
    }
}
