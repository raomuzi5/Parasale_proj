using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.WebUI.Areas.Manager.Models;
using Vereyon.Web;

namespace Parasale.WebUI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IFlashMessage _flashMessage;

        public AccountController(UserManager<AppUser> userManager, IFlashMessage flashMessage)
        {
            _userManager = userManager;
            _flashMessage = flashMessage;
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                if (result.Succeeded)
                    _flashMessage.Confirmation("Success: ", "Password has been updated");
                else
                    _flashMessage.Danger("Error: ", "Current Password is incorrect");
            }
            return RedirectToAction("Index", new { area = "Manager", controller = "Dashboard" });
        }

        public async Task<IActionResult> CheckCurrentPassword(string CurrentPassword)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.CheckPasswordAsync(user, CurrentPassword);
            return Json(result);
        }
    }
}