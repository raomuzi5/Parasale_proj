using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using Parasale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Admin.ViewComponents
{
    public class UnassignedObjectionsViewComponent: ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public UnassignedObjectionsViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }

        private async Task<UnAssignedViewModel> GetItemsAsync()
        {

            UnAssignedViewModel model = new UnAssignedViewModel();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.collections = _repository.CollectionRepository.GetAllCollections(user.Id).Select(o => new SelectListItem()
            {
                Text = o.CollectionName,
                Value = o.Id.ToString()
            }).ToList();

            var objections = _repository.ObjectionRepository.GetAllObjectionsWithnoCollections().ToList();

            foreach (var item in objections)
            {
                model.objectionViewModels.Add(new ObjectionViewModel()
                {
                    Id = item.Id,
                    ObjectionName = item.InitialObjection,
                    ResponseKeyword = item.PitchKeyword,

                });

            }
            return model;
        }
    }
}
