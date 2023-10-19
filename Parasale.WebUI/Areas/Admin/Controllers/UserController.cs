using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Areas.Admin.Models;
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repository;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, IRepositoryWrapper repository, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _repository = repository;
            _roleManager = roleManager;
        }

        public async Task <IActionResult> List()
        {
            var objections = _repository.ObjectionLogRepository.GetObjectionLogs();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            // _userManager.GetUsersInRoleAsync(Role.User.ToString()).Result.
            
            var users = _repository.UserRepository.GetUsersByAdmin(user.Id).Where(x=>x.IsCompanyAdmin==false).Select(p => new UserListViewModel()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                //MissedCount = objections.Where(q => q.AppUser.Id == p.Id && q.IsCompleted == false).Count(),
                //CompletedCount = objections.Where(q => q.AppUser.Id == p.Id && q.IsCompleted == true).Count(),
                IsManager = p.IsManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true
               
            });
            ViewBag.voiceboarding = user.voiceOnboarding;
            return View(users);
        }

        public async Task<IActionResult> Teams()
        {
            var objections = _repository.ObjectionLogRepository.GetObjectionLogs();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            // _userManager.GetUsersInRoleAsync(Role.User.ToString()).Result.
            var users = _repository.UserRepository.GetUsersByAdmin(user.Id).Select(p => new UserListViewModel()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                IsManager = p.IsManager,
                ManagerUserId= p.AssignedManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true
            });
            ViewBag.voiceboarding = user.voiceOnboarding;
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

        public async Task<IActionResult> AddToManager(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var role = await _roleManager.FindByNameAsync(Role.Manager.ToString());
                if (role == null)
                {
                    var newRole = new AppRole()
                    {
                        Name = "Manager",
                        CreatedDate = DateTime.Today,
                        Description = "Role for Manage Users",
                    };
                    await _roleManager.CreateAsync(newRole);
                }
                if (await _userManager.IsInRoleAsync(user, Role.Manager.ToString()))
                {
                    user.IsManager = false;
                    await _userManager.RemoveFromRoleAsync(user, Role.Manager.ToString());
                    await _userManager.AddToRoleAsync(user, Role.User.ToString());
                    var underUsers = _repository.UserRepository.GetUsersUnderManager(user.Id);
                    if (underUsers != null)
                    {
                        foreach (var users in underUsers)
                        {
                            users.AssignedManager = null;
                             _repository.ParasaleRepository.Update(users);
                        }

                    }
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = user.Name,
                        NewData = user.Name,
                        Type = "Removed",
                        Field = "as Manager",
                        UserId = user.Id,
                        
                        AdminId = user.AssginedAdmin,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    });
                }
                else
                {
                    user.IsManager = true;
                    await _userManager.RemoveFromRoleAsync(user, Role.User.ToString());
                    await _userManager.AddToRoleAsync(user, Role.Manager.ToString());
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = user.Name,
                        NewData = user.Name,
                        Type = "Promoted",
                        Field = "as Manager",
                        UserId = user.Id,
                        ManagerId = user.Id,
                        AdminId = user.AssginedAdmin,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    });
                }

                
                await _userManager.UpdateAsync(user);

                await _repository.ParasaleRepository.Save();
                
                
                //await _userManager.AddToRoleAsync(user, Role.Manager.ToString());
            }
            return Json(true);
        }

        public IActionResult UsersList(string Id)
        {
            return ViewComponent("UserListForAdmin", new { userId = Id });
        }

        public IActionResult InviteUserList()
        {
            return ViewComponent("InviteUserList");
        }
        public IActionResult TeamView(string Id)
        {
            return ViewComponent("TeamManager", new { userId = Id });
        }
        public async Task<IActionResult> UpdateTeam(bool isTeamMember, string userId, string managerUserId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var Mangeruser = await _userManager.FindByIdAsync(managerUserId);
            if (isTeamMember)
            {
                user.AssignedManager = managerUserId;

                await _repository.ParasaleRepository.Add(new AuditLog()
                {
                    NewData = user.UserName,
                    Type = "User Assigned",
                    Field = "User : " +user.UserName+ " , Assigned To Manager : " + Mangeruser.UserName,
                    UserId = user.Id,
                    ManagerId = managerUserId,
                    AdminId = user.AssginedAdmin,
                    ModifiedBy = User.Identity.Name,
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
                    Field = "User : " + user.UserName + " , UnAssigned From Manager : " + Mangeruser.UserName ,
                    ModifiedBy = User.Identity.Name,
                    UserId = user.Id,
                    ManagerId = managerUserId,
                    AdminId = user.AssginedAdmin,
                    ModifiedDate = DateTime.Now
                });


            }
            await _userManager.UpdateAsync(user);

            return Json(string.IsNullOrWhiteSpace(user.AssignedManager) ? new { removed = true, user = user.Name } : new { removed = false, user = user.Name });
        }


        public async Task<IActionResult> ManageTeamsAndUsers()
        {
            InvitesViewModel model = new InvitesViewModel();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.voiceboarding = user.voiceOnboarding;

            // Manage Team
            var users = _repository.UserRepository.GetUsersByAdmin(user.Id);
            var userTeam = users.Select(p => new Teams()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                IsManager = p.IsManager,
                ManagerUserId = p.AssignedManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true
            }).ToList();
            model.teams = userTeam;


            //User List
            var usrsList = users.Where(x => x.IsCompanyAdmin == false).Select(p => new Teams()
            {
                Id = p.Id,
                Email = p.Email,
                Name = p.Name,
                Username = p.UserName,
                IsManager = p.IsManager,
                IsAlreadyTeamMember = string.IsNullOrWhiteSpace(p.AssignedManager) ? false : true

            }).ToList();
            model.userList = usrsList;
            return View(model);
        }

        #region commented code
        //public async Task<IActionResult> RemoveFromManager(string Id)
        //{
        //    var user = await _userManager.FindByIdAsync(Id);
        //    if (user != null)
        //    {
        //        user.IsManager = false;
        //        var teamUsers = _userManager.GetUsersInRoleAsync(Role.User.ToString()).Result.Where(p => p.AssignedManager == Id);
        //        await _userManager.RemoveFromRoleAsync(user, Role.Manager.ToString());
        //        await _userManager.UpdateAsync(user);
        //        foreach (var teamuser in teamUsers)
        //        {
        //            teamuser.AssignedManager = null;
        //            await _userManager.UpdateAsync(teamuser);
        //        }
        //    }
        //    return Json(true);
        //}

        #endregion


    }
}
