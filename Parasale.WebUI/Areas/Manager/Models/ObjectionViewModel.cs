using System;
using System.ComponentModel.DataAnnotations;

namespace Parasale.WebUI.Areas.Manager.Models
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

        public string ManagerUserId { get; set; }
        public string userId { get; set; }

    }
}
