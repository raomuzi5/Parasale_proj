using System;
using System.Collections.Generic;
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
using Vereyon.Web;

namespace Parasale.WebUI.Areas.Manager.Controllers
{

    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class CollectionController : Controller
    {
        private IRepositoryWrapper _repository;
        private IFlashMessage _flashMessage;
        private UserManager<AppUser> _userManager;

        public CollectionController(UserManager<AppUser> userManager, IRepositoryWrapper repository, IFlashMessage flashMessage)
        {
            _userManager = userManager;
            _repository = repository;
            _flashMessage = flashMessage;
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

        // POST: Collection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionListViewModel collectionListViewModel)
        {
            var users = await _userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                if (ModelState.IsValid)
                {
                   
                    await _repository.ParasaleRepository.Add(new Collection()
                    {
                        CollectionName = collectionListViewModel.CollectionName,
                        AssignedAdmin = users.AssginedAdmin,
                        appUser = users
                    });

                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        NewData = collectionListViewModel.CollectionName,
                        Type = "Created",
                        Field = "Collection Created",
                        ModifiedBy = User.Identity.Name,
                        ManagerId = users.Id,
                        AdminId = users.AssginedAdmin,
                        UserId = users.Id,
                        ModifiedDate = DateTime.Now
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
                return RedirectToAction("ManageCollections", new { area = "Manager", controller = "Dashboard" });

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

            //CollectionListViewModel collectionListViewModel = new CollectionListViewModel()
            //{
            //    CollectionName = collection.CollectionName,
            //    UserId = collection.appUser.Id,
            //    Id = collection.Id,
            //    AssignedAdmin = collection.AssignedAdmin


            //};
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
            var users = await _userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                if (ModelState.IsValid)
                {
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
                            ManagerId = users.Id,
                            AdminId = users.AssginedAdmin,
                            UserId = users.Id,
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
                return RedirectToAction("ManageCollections", new { area = "Manager", controller = "Dashboard" });

            }
            catch
            {
                return View();
            }
        }



        // GET: Collection/Delete/5
    
        public async Task<IActionResult> Delete(int id)
        {
            var users = await _userManager.FindByNameAsync(User.Identity.Name);
            var collection = _repository.CollectionRepository.GetAllCollectionbyId(id);
            var objections = _repository.ObjectionRepository.getObjectionsbyCollection(collection.Id);
            var assignCollections = _repository.CollectionRepository.GetAssignedCollections(collection.Id);
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
            foreach (var objects in objections)
            {
                _repository.ParasaleRepository.Delete(objects);
            }
            foreach (var aUser in assignCollections)
            {
                _repository.ParasaleRepository.Delete(aUser);
            }

            _repository.ParasaleRepository.Delete(collection);

            await _repository.ParasaleRepository.Add(new AuditLog()
            {
                NewData = collection.CollectionName,
                Type = "Deleted",
                Field = "Collection Deleted",
                UserId = users.Id,
                AdminId = users.AssginedAdmin,
                ManagerId = users.Id,
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
           // return RedirectToAction("ManageCollections", new { area = "Manager", controller = "Collection" });
        }

        #region commented code
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
        //    var users = await _userManager.FindByNameAsync(User.Identity.Name);

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
        //                ManagerId = users.Id,
        //                AdminId = users.AssginedAdmin,
        //                Field = "Un Assigned Objection(s) : " + getObjection.InitialObjection + " Pushed to collection : " + getObjection.collection.CollectionName,
        //                ModifiedBy = User.Identity.Name,
        //                ModifiedDate = DateTime.Now
        //            });

        //        }

        //    }

        //    await _repository.ParasaleRepository.Save();

        //    return RedirectToAction("Index", new { area = "Manager", controller = "Dashboard" });

        //}

        #endregion
    }
}