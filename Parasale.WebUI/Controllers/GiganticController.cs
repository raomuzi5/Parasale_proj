using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using Parasale.WebUI.Models;
using Vereyon.Web;


namespace Parasale.WebUI.Controllers
{
    [Authorize(Roles = "Gigantic")]
    public class GiganticController : Controller
    {
        private IRepositoryWrapper _repository;
        private IFlashMessage _flashMessage;
        private UserManager<AppUser> _userManager;
        public GiganticController(UserManager<AppUser> userManager, IRepositoryWrapper repository, IFlashMessage flashMessage)
        {
            _userManager = userManager;
            _repository = repository;
            _flashMessage = flashMessage;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(_repository.QuickCollectionRepository.GetAllCollections().ToList());
        }
        #region Quick Start Collection
        public IActionResult AddQuickCollection()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AddQuickCollection(string CollectionName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (ModelState.IsValid)
                {
                    await _repository.ParasaleRepository.Add(new QuickCollection()
                    {
                        CollectionName = CollectionName,
                        AssignedAdmin = null,
                        appUser = user,
                        QuickStart = false
                    });
                    await _repository.ParasaleRepository.Save();
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { success=true});
        }


        public async Task<IActionResult> EditQuickCollection(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var quickCollection = _repository.QuickCollectionRepository.GetAllCollectionbyId(id);
            if (quickCollection != null)
            {
                return PartialView(quickCollection);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditQuickCollection(QuickCollection collection)
        {
            var qcollection = _repository.QuickCollectionRepository.GetAllCollectionbyId(collection.Id);
            qcollection.CollectionName = collection.CollectionName;
            _repository.ParasaleRepository.Update(qcollection);
            if (await _repository.ParasaleRepository.Save())
            {
                _flashMessage.Confirmation("Collection Updated Successfully");
            }
            else
            {
                _flashMessage.Danger("Error Occurred while Updating collection");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteQuickCollection(int id)
        {
            var qCollection = _repository.QuickCollectionRepository.GetAllCollectionbyId(id);
            var qobjection = _repository.QuickObjectionRepository.GetAllObjectionbyCollectionId(id);
            foreach (var objects in qobjection)
            {
                _repository.ParasaleRepository.Delete(objects);
            }
            if (qCollection != null)
            {
                _repository.ParasaleRepository.Delete(qCollection);
                if (await _repository.ParasaleRepository.Save())
                {
                    _flashMessage.Confirmation("Collection Deleted Successfully");
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while Deleting collection");
                }
            }
            return RedirectToAction("Index");
        }

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
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Quick Objections
        public IActionResult getQuickObjections(int id)
        {

            return ViewComponent("ObjectionsByCollectionId", new { id = id });
        }
        public async Task<IActionResult> AddQuickObjection(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return PartialView("~/Views/Gigantic/_AddObjection.cshtml", new ObjectionAddViewModel()
            {
                collectionId = id,
                collections = _repository.QuickCollectionRepository.GetAllCollections(user.Id).Select(p => new SelectListItem()
                {
                    Text = p.CollectionName,
                    Value = p.Id.ToString()
                }).ToList()
            });

        }
        [HttpPost]
        public async Task<IActionResult> AddQuickObjection(string ObjectionName, string ResponseKeyword, int collectionId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                string keywordResponse = null;
                var responses = ResponseKeyword.Trim().Split(',');
                if (responses.Count() > 1)
                    keywordResponse = string.Join(", ", responses);
                else
                    keywordResponse = responses.FirstOrDefault();
                await _repository.ParasaleRepository.Add(new QuickStartObjections()
                {
                    InitialObjection = ObjectionName,
                    PitchKeyword = keywordResponse,
                    ValidPitchResponse = "good job",
                    BadPitchResponse = "try again",
                    AssignedAdmin = user.Id,
                    User = user,
                    collection = _repository.QuickCollectionRepository.GetAllCollectionbyId(collectionId)
                });

                if (await _repository.ParasaleRepository.Save())
                {
                    _flashMessage.Confirmation("Objection Saved Successfully");
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while saving objection");
                }
            }
            else
            {
                _flashMessage.Danger("Error Occurred while saving objection");
            }
            return Json(new { success=true});
            //return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditQuickObjection(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var objection = _repository.QuickObjectionRepository.GetAllObjectionbyId(id);
            return PartialView("~/Views/Gigantic/_EditObjection.cshtml", new ObjectionAddViewModel()
            {
                Id = objection.Id,

                ObjectionName = objection.InitialObjection,
                ResponseKeyword = objection.PitchKeyword,
                collectionId = objection.collection == null ? 0 : objection.collection.Id,

                collections = _repository.QuickCollectionRepository.GetAllCollections(user.Id).Select(p => new SelectListItem()
                {
                    Text = p.CollectionName,
                    Value = p.Id.ToString()
                }).ToList()


            });
        }
        [HttpPost]
        public async Task<IActionResult> EditQuickObjection(ObjectionAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                string keywordResponse = null;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var responses = model.ResponseKeyword.Trim().Split(',');
                if (responses.Count() > 1)
                    keywordResponse = string.Join(", ", responses);
                else
                    keywordResponse = responses.FirstOrDefault();

                var objection = _repository.QuickObjectionRepository.GetAllObjectionbyId(model.Id);

                // var collection = _repository.CollectionRepository.GetAllCollectionbyId(model.collectionId);

                objection.InitialObjection = model.ObjectionName;
                objection.PitchKeyword = keywordResponse;
                objection.AssignedAdmin = user.Id;
                objection.User = user;
                objection.collection = _repository.QuickCollectionRepository.GetAllCollectionbyId(model.collectionId);
                _repository.ParasaleRepository.Update(objection);

                if (await _repository.ParasaleRepository.Save())
                {
                    _flashMessage.Confirmation("Objection Updated Successfully");
                }
                else
                {
                    _flashMessage.Danger("Error Occurred while Updating objection");
                }
            }
            else
            {
                _flashMessage.Danger("Error Occurred while Updating objection");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteQuickObjection(int id)
        {
            var objection = _repository.QuickObjectionRepository.GetAllObjectionbyId(id);
            //var logs = _repository.ObjectionLogRepository.GetObjectionLogs(objection.Id);
            //if (logs != null && logs.Count > 0)
            //{
            //    foreach (var log in logs)
            //    {
            //        _repository.ParasaleRepository.Delete(log);
            //    }
            //}
            _repository.ParasaleRepository.Delete(objection);

            //await _repository.ParasaleRepository.Add(new AuditLog()
            //{
            //    NewData = objection.InitialObjection,
            //    Type = "Deleted",
            //    Field = "Objection deleted",
            //    ModifiedBy = User.Identity.Name,
            //    ModifiedDate = DateTime.Now
            //});

            if (await _repository.ParasaleRepository.Save())
            {
                _flashMessage.Confirmation("Objection Deleted Successfully");
            }
            else
            {
                _flashMessage.Danger("Error Occurred while Deleting objection");
            }
            return RedirectToAction("Index");
        }

        public IActionResult MarkQuickObjection(int id)
        {
            return View();
        }
        #endregion
    }
}