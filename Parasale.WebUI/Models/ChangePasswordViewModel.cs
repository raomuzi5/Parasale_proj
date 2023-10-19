using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Parasale.WebUI.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Current Password")]
        [Remote("CheckCurrentPassword", "Account", ErrorMessage = "{0} is incorrect")]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [StringLength(15, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password Not Matched")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
