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
    public class ObjectionsByCollectionIdViewComponent:ViewComponent
    {
        private IRepositoryWrapper _repository;
        private UserManager<AppUser> _userManager;

        public ObjectionsByCollectionIdViewComponent(IRepositoryWrapper repository, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var result = await GetItemsAsync(id);
            ObjectionVM objectionVM = new ObjectionVM()
            {
                id = id,
                Obj = result
            };
            return View(objectionVM);
        }
        private async Task<List<ObjectionViewModel>> GetItemsAsync(int id)
        {
            return await Task.FromResult(_repository.QuickObjectionRepository.GetAllObjectionbyCollectionId(id).Select(x => new ObjectionViewModel()
            {
                Id = x.Id,
                ObjectionName = x.InitialObjection,
                ResponseKeyword = x.PitchKeyword
            }).ToList()
        );
        }
    }
}
