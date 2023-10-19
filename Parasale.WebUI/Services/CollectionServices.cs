using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Parasale.WebUI.Services
{
    public class CollectionServices
    {
        private readonly IConfigurationRoot _config;
        private readonly IRepositoryWrapper _repository;
        private readonly UserManager<AppUser> _userManager;
        private IFlashMessage _flashMessage;
        public CollectionServices(IRepositoryWrapper repository, UserManager<AppUser> userManager, IFlashMessage flashMessage, IConfigurationRoot config)
        {
            _repository = repository;
            _config = config;
            _userManager = userManager;
            _flashMessage = flashMessage; 
        }

        public async Task<string> AddCollectionForAdmin(CollectionListViewModel collectionListViewModel, AppUser users)
        {
            //CollectionListViewModel collection = new CollectionListViewModel();
            try
            {

                //if (ModelState.IsValid)
                //{
                //var users = await _userManager.FindByNameAsync(User.Identity.Name);
                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = collectionListViewModel.CollectionName,
                    Type = "Created",
                    Field = "Collection Created",
                    ModifiedBy = users.UserName,//.Identity.Name,
                    UserId = users.Id,
                    AdminId = users.Id,
                    ModifiedDate = DateTime.Now
                });

                await _repository.ParasaleRepository.Add(new Collection()
                {
                    CollectionName = collectionListViewModel.CollectionName,
                    appUser = users,
                    AssignedAdmin = users.Id
                });

                if (await _repository.ParasaleRepository.Save())
                {
                    _flashMessage.Confirmation("Collection Saved Successfully");
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while saving Collection");
                }
                //}
                string a = "ok";
                //else
                //{
                //    _flashMessage.Danger("Error Occurred while saving collection");
                //}
                // return RedirectToAction("ManageCollections", new { area = "Admin", controller = "Dashboard" });
                return a;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
