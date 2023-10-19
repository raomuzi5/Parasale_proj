using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Parasale.WebUI.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        [Remote("CheckUsername", "Account", ErrorMessage = "{0} Already exist")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(15, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 5)]
        public string Password { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not a valid Email")]
        [Remote("CheckEmail", "Account", ErrorMessage = "{0} Already exist")]
        public string EmailAddress { get; set; }

        public string adminId { get; set; }
        public string managerID { get; set; }
        public string token { get; set; }
        
        [Required(ErrorMessage = "Please! check this to continue.")]
        public bool IsTermsConditionAccepted { get; set; }
        
        public bool IsEmailExist { get; set; }
        public bool IsAdmin { get; set; }
        public string YesNo { get; set; }
    }

}
