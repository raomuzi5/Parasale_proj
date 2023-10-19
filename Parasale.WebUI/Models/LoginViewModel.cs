using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Parasale.WebUI.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        public string Username { get; set; }
        public string Id { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [EmailAddress(ErrorMessage = "{0} is not a valid Email")]
        [Remote("CheckEmailExist", "Account", ErrorMessage = "{0} email does not exist")]
        public string Email { get; set; }
    }
}
