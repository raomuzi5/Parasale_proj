using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.ViewComponents
{
    public class ManagerUnassignedObjectionsViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public ManagerUnassignedObjectionsViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }

        private async Task<MUnassingedObjectionVM> GetItemsAsync()
        {
            MUnassingedObjectionVM mUnassingedObjectionVM = new MUnassingedObjectionVM();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            mUnassingedObjectionVM.collections = _repository.CollectionRepository.GetAllCollections(user.Id).Select(o => new SelectListItem()
            {
                Text = o.CollectionName,
                Value = o.Id.ToString()
            }).ToList();

            var objections = _repository.ObjectionRepository.GetAllbjectionsFromManagerWithnoCollection(user.Id).ToList();

            foreach (var item in objections)
            {
                mUnassingedObjectionVM.objectionViewModels.Add(new Models.ObjectionViewModel()
                {
                    Id = item.Id,
                    ObjectionName = item.InitialObjection,
                    ResponseKeyword = item.PitchKeyword,
                    //userId = managerUserId,
                    ManagerUserId = user.Id

                });

        }
            return mUnassingedObjectionVM;
        }

    }
}
