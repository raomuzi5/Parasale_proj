using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class CompanyViewModel
    {

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Email")]
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not a valid Email")]
        [Remote("CheckEmail", "Account", ErrorMessage = "{0} Already exist")]
        public string EmailAddress { get; set; }

        [Display(Name = "Company Address")]
        [Required(ErrorMessage = "{0} is required")]
      
        public string Address { get; set; }

        [Display(Name = "Company Phone")]
        [Required(ErrorMessage = "{0} is required")]

        public string Phone { get; set; }
    }

}
