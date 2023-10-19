using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.ViewComponents
{
    public class ObjectionLogViewComponent : ViewComponent
    {
        private IRepositoryWrapper _repository;
        private UserManager<AppUser> _userManager;

        public ObjectionLogViewComponent(IRepositoryWrapper repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name, bool isCompleted, bool isFromAdmin)
        {
            var result = await GetItemsAsync(name, isCompleted, isFromAdmin);
            return View(result);
        }

        private async Task<ObjectionListViewModel> GetItemsAsync(string name, bool isCompleted, bool isFromAdmin)
        {
            var user = await _userManager.FindByNameAsync(name);
            var objections = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id, isCompleted);
            var model = new ObjectionListViewModel()
            {
                Objections = _repository.ObjectionLogRepository.GetObjectionLogs(user.Id, isCompleted).Select(p => new ObjectionViewModel()
                {
                    Id = p.Id,
                    ObjectionName = p.Objection.InitialObjection,
                    ResponseKeyword = p.Objection.PitchKeyword,
                    ObjectionTime = p.ObjectionTime,
                }).ToList(),
                isCompleted = isCompleted,
                isFromAdmin = isFromAdmin
            };

            return model;
        }
    }
}
