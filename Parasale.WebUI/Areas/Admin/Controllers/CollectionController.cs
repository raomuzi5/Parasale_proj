using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Manager.Models;
using Parasale.WebUI.Models;
using Parasale.WebUI.Services;
using Vereyon.Web;
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class CollectionController : Controller
    {
        private IRepositoryWrapper _repository;
        private IFlashMessage _flashMessage;
        private UserManager<AppUser> _userManager;
        private CollectionServices _collectionSerives;

        public CollectionController(UserManager<AppUser> userManager, IRepositoryWrapper repository, IFlashMessage flashMessage, CollectionServices collectionServices)
        {
            _userManager = userManager;
            _repository = repository;
            _flashMessage = flashMessage;
            _collectionSerives = collectionServices;
        }
        // GET: Collection
        public IActionResult Index()
        {
            return View();
        }

        // GET: Collection/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Collection/Create
        public IActionResult Create()
        {
            return PartialView();
        }
        public IActionResult GetObjectionsByCollection(int id)
        {
            return ViewComponent("AObjectionsbyCollection", new { id = id });
        }
        // POST: Collection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionListViewModel collectionListViewModel)
        {
            //var users = await _userManager.FindByNameAsync(User.Identity.Name);
            //var collection = _collectionSerives.AddCollectionForAdmin(collectionListViewModel,users);
            try
            {
                if (ModelState.IsValid)
                {
                    var users = await _userManager.FindByNameAsync(User.Identity.Name);
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        NewData = collectionListViewModel.CollectionName,
                        Type = "Created",
                        Field = "Collection Created",
                        ModifiedBy = User.Identity.Name,
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

                }
                else
                {
                    _flashMessage.Danger("Error Occurred while saving collection");
                }
                return RedirectToAction("ManageCollections", new { area = "Admin", controller = "Dashboard" });

            }
            catch
            {
                return View();
            }
        }

        // GET: Collection/Edit/5
        public IActionResult Edit(int id)
        {
            var collection = _repository.CollectionRepository.GetAllCollectionbyId(id);
            return PartialView(new CollectionListViewModel()
            {
                CollectionName = collection.CollectionName,
                UserId = collection.appUser.Id,
                Id = collection.Id,
                AssignedAdmin = collection.AssignedAdmin

            });
        }

        // POST: Collection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CollectionListViewModel collectionListViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = await _userManager.FindByNameAsync(User.Identity.Name);
                    var collection = _repository.CollectionRepository.GetAllCollectionbyId(collectionListViewModel.Id);


                    if (collection.CollectionName != collectionListViewModel.CollectionName)
                    {
                        await _repository.ParasaleRepository.Add(new AuditLog()
                        {
                            PreviousData = collection.CollectionName,
                            NewData = collectionListViewModel.CollectionName,
                            Type = "Updated",
                            Field = "Collection Updated",
                            ModifiedBy = User.Identity.Name,
                            UserId = users.Id,
                            AdminId = users.Id,
                            ModifiedDate = DateTime.Now
                        });
                    }

                    collection.CollectionName = collectionListViewModel.CollectionName;
                    _repository.ParasaleRepository.Update(collection);
                    if (await _repository.ParasaleRepository.Save())
                    {
                        _flashMessage.Confirmation("Collection Updated Successfully");
                    }
                    else
                    {
                        _flashMessage.Danger("Error Occurred while Updating collection");
                    }
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while Updating collection");
                }
                return RedirectToAction("ManageCollections", new { area = "Admin", controller = "Dashboard" });

            }
            catch
            {
                return View();
            }
        }

        #region Quick Start Collection
        public async Task<IActionResult> MarkQuickStart(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var collection = _repository.QuickCollectionRepository.GetAllCollectionbyId(id);


                    collection.QuickStart = (collection.QuickStart == true) ? false : true;
                    _repository.ParasaleRepository.Update(collection);
                    if (await _repository.ParasaleRepository.Save())
                    {
                        _flashMessage.Confirmation("Collection Updated Successfully");
                    }
                    else
                    {
                        _flashMessage.Danger("Error Occurred while Updating collection");
                    }
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while Updating collection");
                }
                return RedirectToAction("Gigantic", new { area = "", controller = "Home" });

            }
            catch
            {
                return View();
            }
        }
        #endregion


        // GET: Collection/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var users = await _userManager.FindByNameAsync(User.Identity.Name); 
            var collection = _repository.CollectionRepository.GetAllCollectionbyId(id);
            var objections = _repository.ObjectionRepository.getObjectionsbyCollection(collection.Id);
            var objIds = objections.Select(x => x.Id).ToList();
            var objectionLogs = _repository.ObjectionLogRepository.GetObjectionLogsbyObjId(objIds);
            var CCSObjections = _repository.CCSRepository.GetScorebyObjectionId(objIds);
            if (CCSObjections != null)
            {
                foreach (var ccs in CCSObjections)
                {
                    _repository.ParasaleRepository.Delete(ccs);
                }
            }
            if (objectionLogs != null)
            {
                foreach (var logs in objectionLogs)
                {
                    _repository.ParasaleRepository.Delete(logs);
                }
            }
            if (objections != null)
            {
                foreach (var objects in objections)
                {
                    _repository.ParasaleRepository.Delete(objects);
                }
            }

            _repository.ParasaleRepository.Delete(collection);

            await _repository.ParasaleRepository.Add(new AuditLog()
            {
                NewData = collection.CollectionName,
                Type = "Deleted",
                Field = "Collection Deleted",
                UserId = users.Id,
                AdminId = users.Id,
                ModifiedBy = User.Identity.Name,
                ModifiedDate = DateTime.Now
            });

            if (await _repository.ParasaleRepository.Save())
            {
                _flashMessage.Confirmation("Collection Deleted Successfully");
            }
            else
            {
                _flashMessage.Danger("Error Occurred while Deleting collection");
            }
            return Json(true);
            //return RedirectToAction("ManageCollections", new { area = "Admin", controller = "Dashboard" });
        }

        #region Code not in use

        //// POST: Collection/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //public async Task<IActionResult> PushObjectionsToCollection(List<ObjectionNotificationViewModel> models, int collectionId)
        //{
        //    foreach (var model in models)
        //    {
        //        var getObjection = _repository.ObjectionRepository.GetObjection(model.ObjectionId);
        //        if (getObjection.collection == null)
        //        {
        //            getObjection.collection = _repository.CollectionRepository.GetAllCollectionbyId(collectionId);
        //            _repository.ParasaleRepository.Update(getObjection);


        //            await _repository.ParasaleRepository.Add(new AuditLog()
        //            {
        //                NewData = getObjection.collection.CollectionName,
        //                Type = "Added",
        //                Field = "Un Assigned Objection : " + getObjection.InitialObjection + " Push to collection : " + getObjection.collection.CollectionName,
        //                ModifiedBy = User.Identity.Name,
        //                ModifiedDate = DateTime.Now
        //            });
        //        }

        //    }

        //    await _repository.ParasaleRepository.Save();

        //    return RedirectToAction("Index", new { area = "Admin", controller = "Dashboard" });

        //}

        #endregion
    }
}