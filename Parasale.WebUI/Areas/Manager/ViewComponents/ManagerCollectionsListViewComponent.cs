using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.ViewComponents
{
    public class ManagerCollectionsListViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private IRepositoryWrapper _repository;

        public ManagerCollectionsListViewComponent(UserManager<AppUser> userManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _repository = repository;
        }


        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var result = await GetItemsAsync(userId);
            return View(result);
        }

        private async Task<List<CollectionListViewModel>> GetItemsAsync(string userId)
        {
            var isExist = false;
            var user = await _userManager.FindByIdAsync(userId);
            var assignedCollections = _repository.CollectionRepository.GetAllAssignedCollections();
            var allCollections = _repository.CollectionRepository.GetAllCollections(user.AssignedManager, user.AssginedAdmin);
            
            List<CollectionListViewModel> finalList = new List<CollectionListViewModel>();
            foreach (var aCollections in allCollections)
            {
                CollectionListViewModel model = new CollectionListViewModel();
                isExist = false;
                var isUserin = assignedCollections.Where(x => x.CollectionId == aCollections.Id && x.UserId == user.Id);
                foreach (var aColl in isUserin)
                {
                    if (aColl.CollectionId == aCollections.Id && aColl.UserId == user.Id)
                    {
                        isExist = true;
                        model.Id = aColl.collection.Id;
                        model.CollectionName = aColl.collection.CollectionName;
                        model.IsCollectionAssigned = true;
                        model.UserId = userId;
                        model.ManagerUserId = user.AssignedManager;
                        finalList.Add(model);
                    }

                }
                if (!isExist)
                {
                    model.Id = aCollections.Id;
                    model.CollectionName = aCollections.CollectionName;
                    model.IsCollectionAssigned = false;
                    model.UserId = userId;
                    model.ManagerUserId = user.AssignedManager;
                    finalList.Add(model);
                }
            }

            //return _repository.CollectionRepository.GetAllAssignedCollections().Select(p => new CollectionListViewModel()
            //{
            //    Id = p.collection.Id,
            //    CollectionName = p.collection.CollectionName,
            //    UserId = userId,
            //    ManagerUserId = managerUserId,
            //    IsCollectionAssigned = p.UserId == null ? false : true,

            //}).ToList();
            //return _repository.CollectionRepository.GetAllCollections(managerUserId, user.AssginedAdmin).Select(p => new CollectionListViewModel()
            //{
            //    Id = p.Id,
            //    CollectionName = p.CollectionName,
            //    UserId = userId,
            //    ManagerUserId = managerUserId

            //}).ToList();
            return finalList;
        }
    }
}
