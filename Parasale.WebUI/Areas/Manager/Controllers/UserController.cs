using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Manager.Models;
using Parasale.WebUI.Models;
using Vereyon.Web;
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly RoleManager<AppRole> _roleManager;
        private IFlashMessage _flashMessage;

        public UserController(UserManager<AppUser> userManager, IFlashMessage flashMessage, IRepositoryWrapper repository, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> List()
        {
            var objections = _repository.ObjectionLogRepository.GetObjectionLogs();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var users = _repository.UserRepository.GetUsersUnderManager(user.Id).Select(p => new UserListViewModel()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                MissedCount = objections.Where(q => q.AppUser.Id == p.Id && q.IsCompleted == false).Count(),
                CompletedCount = objections.Where(q => q.AppUser.Id == p.Id && q.IsCompleted == true).Count(),
                IsManager = p.IsManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true
            }).ToList();
            ViewBag.voicebaording = user.voiceOnboarding;
            ViewBag.currentStep = user.currentStep;

            return View(users);
        }

        public IActionResult UserCompletedObjections(string UserName)
        {
            return ViewComponent("ObjectionLog", new { name = UserName, isCompleted = true });
        }

        public IActionResult UserMissedObjections(string UserName)
        {
            return ViewComponent("ObjectionLog", new { name = UserName, isCompleted = false });
        }

        public async Task<IActionResult> UsersList(int Id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return ViewComponent("UserList", new { userId = user.Id, objectionId = Id });
        }

        public async Task<IActionResult> UnassignedUsers()
        {
            return ViewComponent("UserListForManager");
        }
        public async Task<IActionResult> UpdateTeam(bool isTeamMember, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var Mangeruser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (isTeamMember)
            {
                user.AssignedManager = Mangeruser.Id;
                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = user.UserName,
                    Type = "User Assigned",
                    Field = "Manager : " + Mangeruser.UserName + " ,has added "+user.Name+" to their team.",
                    ModifiedBy = User.Identity.Name,
                    ManagerId = Mangeruser.Id,
                    UserId = user.Id,
                    AdminId = user.AssginedAdmin,
                    ModifiedDate = DateTime.Now
                });


            }
            else
            {
                user.AssignedManager = null;
                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    PreviousData = user.UserName,
                    Type = "User UnAssigned",
                    Field = "User : " + user.UserName + " , is removed From Manager : " + Mangeruser.UserName+"'s Team",
                    ModifiedBy = User.Identity.Name,
                    ManagerId = Mangeruser.Id,
                    UserId = user.Id,
                    AdminId = user.AssginedAdmin,
                    ModifiedDate = DateTime.Now
                });

            }
            await _userManager.UpdateAsync(user);

            return Json(string.IsNullOrWhiteSpace(user.AssignedManager) ? new { removed = true, user = user.Name } : new { removed = false, user = user.Name });
        }

        public async Task<IActionResult> PushObjection(List<ObjectionNotificationViewModel> models)
        {
            foreach (var model in models)
            {
                await _repository.ParasaleRepository.Add(new ObjectionNotification()
                {
                    IsCleared = false,
                    PushedByUserId = model.PushedByUserId,
                    PushedToUserId = model.PushedToUserId,
                    Objection = _repository.ObjectionRepository.GetObjection(model.ObjectionId),
                    CreatedTime = DateTime.Now
                });
            }

            await _repository.ParasaleRepository.Save();

            return Json(true);
        }

        public async Task<IActionResult> PushCollection(List<AssignedCollectionViewModel> models)
        {

            foreach (var model in models)
            {


                await _repository.ParasaleRepository.Add(new AssignedCollection()
                {
                    appUser = await _userManager.FindByIdAsync(model.PushedToUserId),
                    UserId = model.PushedToUserId,
                    collection = _repository.CollectionRepository.GetAllCollectionbyId(model.collectionId)
                });

            }

            await _repository.ParasaleRepository.Save();

            return Json(true);
        }


        public async Task<IActionResult> UpdateAssignCollection(bool isChecked, string userId, int CollectionId, string ManagerId)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = await _userManager.FindByIdAsync(userId);
            //var Mangeruser = await _userManager.FindByIdAsync(ManagerId);
            var Collection = _repository.CollectionRepository.GetAllCollectionbyId(CollectionId);
            if (isChecked)
            {
                //user.AssignedManager = ManagerId;
                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = user.UserName,
                    Type = "Collection Assigned",
                    ManagerId = user.AssignedManager,
                    UserId = userId,
                    AdminId = user.AssginedAdmin,
                    Field = "User : " + user.UserName + " , Assigned To Collection : " + Collection.CollectionName,
                    ModifiedBy = User.Identity.Name,
                    ModifiedDate = DateTime.Now
                }); ;
                await _repository.ParasaleRepository.Add(new AssignedCollection()
                {
                    appUser = user,
                    UserId = userId,
                    AssignedAdmin = user.AssginedAdmin,
                    collection = Collection

                });
                await _repository.ParasaleRepository.Save();
                //await _userManager.UpdateAsync(user);
                _flashMessage.Confirmation("Changes Saved Successfully");
                return Json(new { removed = false, user = user.Name });

            }
            else
            {

                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    PreviousData = user.UserName,
                    Type = "Collection UnAssigned",
                    ManagerId = user.AssignedManager,
                    UserId = userId,
                    AdminId = user.AssginedAdmin,
                    Field = "User : " + user.UserName + " , UnAssigned From Collection : " + Collection.CollectionName,
                    ModifiedBy = User.Identity.Name,
                    ModifiedDate = DateTime.Now
                });

                var Unassigncollection = _repository.CollectionRepository.GetAssignedCollections(CollectionId, userId);
                if (Unassigncollection != null)
                {
                    _repository.ParasaleRepository.Delete(Unassigncollection);

                }
                await _repository.ParasaleRepository.Save();
                //await _userManager.UpdateAsync(user);
                _flashMessage.Confirmation("Changes Saved Successfully");
                return Json(new { removed = true, user = user.Name });

            }

            //await _repository.ParasaleRepository.Save();
            //await _userManager.UpdateAsync(user);
            //_flashMessage.Confirmation("Changes Saved Successfully");

            //return Json(string.IsNullOrWhiteSpace(user.AssignedManager) ? new { removed = true, user = user.Name } : new { removed = false, user = user.Name });
        }


    }
}