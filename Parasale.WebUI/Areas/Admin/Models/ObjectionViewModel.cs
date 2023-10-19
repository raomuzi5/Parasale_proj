using System;
using System.ComponentModel.DataAnnotations;

namespace Parasale.WebUI.Areas.Admin.Models
{
    public class ObjectionViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Objection Name")]
        public string ObjectionName { get; set; }

        [Required]
        [Display(Name = "Response Keyword(s)")]
        public string ResponseKeyword { get; set; }
        public DateTime ObjectionTime { get; set; }
        public string AssignedAdmin { get; set; }
        public string ManagerName { get; set; }
    }
}
