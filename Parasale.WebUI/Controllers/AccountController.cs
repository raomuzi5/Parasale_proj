using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Models;
using Parasale.WebUI.Services;
using Vereyon.Web;
using static Parasale.WebUI.Helpers.Constants;

namespace Parasale.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IRepositoryWrapper _repository;
        private EmailService _emailService;
        private DummyDataService _dummyDataService;
        private IFlashMessage _flashMessage;
        public AccountController(IRepositoryWrapper repository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFlashMessage flashMessage, EmailService emailService, DummyDataService dummyDataService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _flashMessage = flashMessage;
            _repository = repository;
            _emailService = emailService;
            _dummyDataService = dummyDataService;
        }


        public IActionResult Register(string token, string value)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", new { area = "", controller = "Home" });
            }
            var tokenExist = _repository.InvitesRepository.getInviteByGuid(token);
            var ManagerInvitation = _repository.InvitesRepository.getManagerInviteByGuid(token);
            RegisterViewModel registerViewModel = new RegisterViewModel();
            if (tokenExist != null)
            {
                registerViewModel.EmailAddress = tokenExist.Email;
                registerViewModel.adminId = tokenExist.AssignedAdmin;
                registerViewModel.token = tokenExist.Guid;
                registerViewModel.IsEmailExist = true;
                registerViewModel.YesNo = "Yes";
                //registerViewModel.IsAdmin = true;
            }
            if (ManagerInvitation != null)
            {
                registerViewModel.EmailAddress = ManagerInvitation.Email;
                registerViewModel.adminId = ManagerInvitation.AssignedAdmin;
                registerViewModel.IsEmailExist = true;
                registerViewModel.token = ManagerInvitation.Guid;
                registerViewModel.managerID = ManagerInvitation.AssignedManager;
                registerViewModel.YesNo = "Yes";
            }
            return View(registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                if (!model.IsTermsConditionAccepted)
                {
                    ModelState.AddModelError("", "Please Accept Terms & Conditions");
                    return View();
                }

                else
                {

                    var result = await _userManager.CreateAsync(new AppUser()
                    {
                        Name = model.Name,
                        Email = model.EmailAddress,
                        UserName = model.Username,
                        IsFirstLogin=true
                    }, model.Password);
                    if (result.Succeeded)
                    {
                        var isSigned = _repository.InvitesRepository.getInviteByEmail(model.EmailAddress, model.token);
                        var isManagerInvitation = _repository.InvitesRepository.getManagerInviteByEmail(model.EmailAddress, model.token);
                        var userToAssignRole = await _userManager.FindByNameAsync(model.Username);

                        if (isManagerInvitation != null)
                        {
                            isManagerInvitation.IsSigned = true;
                            if (model.YesNo == "Yes")
                            {
                                userToAssignRole.IsCompanyAdmin = false;
                                userToAssignRole.AssginedAdmin = isManagerInvitation.AssignedAdmin;
                                userToAssignRole.AssignedManager = isManagerInvitation.AssignedManager;
                                await _userManager.AddToRoleAsync(userToAssignRole, Role.User.ToString());

                            }
                            else
                            {
                                userToAssignRole.IsCompanyAdmin = true;
                                await _userManager.AddToRoleAsync(userToAssignRole, Role.Admin.ToString());
                            }
                            _repository.ParasaleRepository.Update(isManagerInvitation);
                            _repository.ParasaleRepository.Update(userToAssignRole);

                            await _repository.ParasaleRepository.Save();
                            return RedirectToAction("Login", new { area = "", controller = "Account" });

                        }
                        if (isSigned != null)
                        {
                            isSigned.IsSigned = true;
                            if (model.YesNo == "Yes")
                            {
                                userToAssignRole.IsCompanyAdmin = false;
                                userToAssignRole.AssginedAdmin = isSigned.AssignedAdmin;
                                _repository.ParasaleRepository.Update(isSigned);
                                await _userManager.AddToRoleAsync(userToAssignRole, Role.User.ToString());
                            }
                            else
                            {
                                userToAssignRole.IsCompanyAdmin = true;
                                _repository.ParasaleRepository.Update(isSigned);
                                await _userManager.AddToRoleAsync(userToAssignRole, Role.User.ToString());
                            }
                            _repository.ParasaleRepository.Update(userToAssignRole);

                            await _repository.ParasaleRepository.Save();
                            return RedirectToAction("Login", new { area = "", controller = "Account" });

                        }
                        else
                        {
                            userToAssignRole.IsCompanyAdmin = true;
                            _repository.ParasaleRepository.Update(userToAssignRole);
                            //await _userManager.SetLockoutEndDateAsync(userToAssignRole, DateTimeOffset.MaxValue);
                            await _userManager.AddToRoleAsync(userToAssignRole, Role.Admin.ToString());
                            await _repository.ParasaleRepository.Save();
                            return RedirectToAction("Login", new { area = "", controller = "Account" });


                        }

                        #region Commented
                        //if (!_repository.UserRepository.CheckUser())
                        //{
                        //    await _userManager.AddToRoleAsync(userToAssignRole, Role.Admin.ToString());
                        //    return RedirectToAction("Login", new { area = "", controller = "Account" });
                        //}
                        //else
                        //{
                        //    await _userManager.AddToRoleAsync(userToAssignRole, Role.User.ToString());
                        //    return RedirectToAction("Login", new { area = "", controller = "Account" });
                        //}
                        #endregion
                    }
                    else
                        ModelState.AddModelError("", "Error While Saving Data to Db.");
                }
            }
            else
                ModelState.AddModelError("", "Something Went Wrong.");
            return View();
        }
        public IActionResult LoginApproval()
        {
            return View();
        }

        #region Commented
        //public IActionResult CreateCompany(RegisterViewModel model)
        //{
        //    CompanyRegisterViewModel companyRegisterView = new CompanyRegisterViewModel();
        //    companyRegisterView.register = model;
        //    return View(companyRegisterView);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateCompany(CompanyRegisterViewModel model)
        //{
        //    var result = await _userManager.CreateAsync(new AppUser()
        //    {
        //        Name = model.register.Name,
        //        Email = model.register.EmailAddress,
        //        UserName = model.register.Username,
        //        IsCompanyAdmin = true,
        //    }, model.register.Password);

        //    if (result.Succeeded)
        //    {
        //        var userToAssignRole = await _userManager.FindByNameAsync(model.register.Username);
        //        await _userManager.AddToRoleAsync(userToAssignRole, Role.Admin.ToString());

        //        var company = _repository.ParasaleRepository.Add(new Company()
        //        {

        //            CompanyName = model.company.CompanyName,
        //            Address = model.company.Address,
        //            Email = model.company.EmailAddress,
        //            Phone = model.company.Phone,
        //            appUser = userToAssignRole

        //        });
        //        _repository.ParasaleRepository.Add(company);
        //        _repository.ParasaleRepository.Save();

        //        return RedirectToAction("Login", new { area = "", controller = "Account" });
        //    }
        //    else
        //        ModelState.AddModelError("", "Error While Saving Data to Db.");
        //    return View();
        //}

        #endregion
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                return await CheckLogin(user);
                //if (await _userManager.IsInRoleAsync(user, Role.Admin.ToString()))
                //{

                //    return RedirectToAction("Index", new { area = "Admin", controller = "Dashboard" });
                //}
                //else if (await _userManager.IsInRoleAsync(user, Role.Gigantic.ToString()))
                //{
                //    return RedirectToAction("Index", new { area = "", controller = "Gigantic" });

                //}
                //else if (await _userManager.IsInRoleAsync(user, Role.Manager.ToString()))
                //{
                //    return RedirectToAction("Index", new { area = "Manager", controller = "Dashboard" });
                //}
                //else
                //{
                //    return RedirectToAction("Index", new { area = "", controller = "Home" });
                //}
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Append("username", model.Username, options);
                    var user = await _userManager.FindByNameAsync(model.Username);
                    ConfigClass.Username = model.Username;
                    if (user.voiceOnboarding != true && user.IsCompanyAdmin || user.IsManager)
                    {
                        if (!_repository.ParasaleRepository.DataExist(user.Id))
                        {
                            var abc = await _dummyDataService.AddDummyData(user.UserName);
                        }
                    }
                    return await CheckLogin(user);
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                }
            }
            return View();
        }
        public async Task<IActionResult> CheckLogin(AppUser user)
        {


            if (await _userManager.IsInRoleAsync(user, Role.Admin.ToString()))
            {
                return RedirectToAction("PracticeObjections", new { area = "Admin", controller = "Dashboard" });
            }
            else if (await _userManager.IsInRoleAsync(user, Role.Manager.ToString()))
            {
                return RedirectToAction("PracticeObjections", new { area = "Manager", controller = "Dashboard" });
            }
            else if (await _userManager.IsInRoleAsync(user, Role.Gigantic.ToString()))
            {
                return RedirectToAction("Index", new { area = "", controller = "Gigantic" });
            }
            else
            {
                return RedirectToAction("Index", new { area = "", controller = "Home" });
            }
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
                {
                    _flashMessage.Confirmation("Success: ", "Password has been updated");
                }  
                else
                {
                    _flashMessage.Danger("Error: ", "Current Password is incorrect");
                }
                return await CheckLogin(user);
            }
            
            return RedirectToAction("Index", new { area = "", controller = "Home" });
        }

        public async Task<IActionResult> CheckCurrentPassword(string CurrentPassword)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.CheckPasswordAsync(user, CurrentPassword);
            return Json(result);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new { area = "", controller = "Account" });
        }

        public async Task<IActionResult> AccessDenied()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                return await CheckLogin(user);

            }
            return View();
        }

        public async Task<IActionResult> CheckUsername(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);
            return Json(user == null ? true : false);
        }

        public async Task<IActionResult> CheckEmailExist(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return Json(user == null ? false : true);
        }
        public async Task<IActionResult> CheckEmail(string EmailAddress)
        {
            var user = await _userManager.FindByEmailAsync(EmailAddress);
            return Json(user == null ? true : false);
        }

        public async Task<IActionResult> CheckInvitedEmail(string EmailAddress)
        {
            var users = _repository.UserRepository.GetUsersByEmail(EmailAddress);
            if (users == null)
            {
                var ManagerInvitations = _repository.InvitesRepository.getManagerInviteByEmail(EmailAddress);
                var AdminInvitations = _repository.InvitesRepository.getInviteByEmail(EmailAddress);
                if (ManagerInvitations != null && ManagerInvitations.IsSigned == false)
                {
                    var user = await _userManager.FindByIdAsync(ManagerInvitations.AssignedManager);
                    var token = ManagerInvitations.Guid;
                    return Json(new { success = true, user.Name, token = token });
                }
                if (AdminInvitations != null && AdminInvitations.IsSigned == false)
                {
                    var user = await _userManager.FindByIdAsync(AdminInvitations.AssignedAdmin);
                    var token = AdminInvitations.Guid;
                    return Json(new { success = true, user.Name, token = token });
                }
            }
            return Json(new { success = false });
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendPasswordResetLink(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var resetLink = "";
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetLink += $"We've received a request to reset your password. If you'd like to reset your password, click <br>";
                var resetL = Url.Action("ResetPassword",
                                "Account", new { token = token },
                                 protocol: HttpContext.Request.Scheme) + $"&username={user.UserName}";
                var fi = "<a href=" + resetL + "'>here</a>";
                resetLink += fi;
                //                resetLink += $"&username={user.UserName}";

                _emailService.SendMail(user.Email, "Parasale Password Reset", resetLink);
                //TempData["Email"] = user.Email;
            }

            ViewBag.Message = "Password reset link has been sent to your email address!";


            return RedirectToAction("AfterForgetPassword", new { email = user.Email });

        }

        public IActionResult AfterForgetPassword(string email)
        {

            ForgetPassViewModel forget = new ForgetPassViewModel();
            forget.email = email;
            return View(forget);

        }
        public IActionResult ResetPassword(string token, string username)
        {
            var model = new ResetPasswordViewModel()
            {
                Token = token,
                UserName = username
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel obj)
        {
            var user = await _userManager.FindByNameAsync(obj.UserName);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, obj.Token, obj.Password);
            if (result.Succeeded)
                return View("Login");
            else
                return View("Error");
        }
    }
}