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
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Areas.Manager.ViewComponents
{
    public class ManagerCollectionListViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repositoryWrapper;

        public ManagerCollectionListViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }

        private async Task<List<CollectionListViewModel>> GetItemsAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return await Task.FromResult(_repositoryWrapper.CollectionRepository.GetAllCollections(user.Id).Select(x => new CollectionListViewModel()
            {
                CollectionName = x.CollectionName,
                Id = x.Id
            }).ToList());
        }
    }
}
