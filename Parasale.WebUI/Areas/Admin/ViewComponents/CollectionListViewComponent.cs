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

namespace Parasale.WebUI.Areas.Admin.ViewComponents
{
    public class CollectionListViewComponent : ViewComponent
    {
        private IRepositoryWrapper _repositoryWrapper;
        private UserManager<AppUser> _userManager;

        public CollectionListViewComponent(IRepositoryWrapper repositoryWrapper, UserManager<AppUser> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetItemsAsync();
            return View(result);
        }

        private async Task<List<CollectionListViewModel>> GetItemsAsync()
        {
            var LoggedinAdmin = await _userManager.FindByNameAsync(User.Identity.Name); 
            
            return await Task.FromResult(_repositoryWrapper.CollectionRepository.GetAllCollections(LoggedinAdmin.Id).Select(x => new CollectionListViewModel()
            {
                CollectionName = x.CollectionName,
                Id = x.Id
            }).ToList());
        }
    }
}
