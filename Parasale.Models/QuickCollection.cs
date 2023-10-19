using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Parasale.Models
{
   public class QuickCollection
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Collection Name")]
        public string CollectionName { get; set; }

        public AppUser appUser { get; set; }
        public string AssignedAdmin { get; set; }
        public bool? QuickStart { get; set; }
        public bool voiceboarding { get; set; }
        public bool? isDummy { get; set; }
    }
}
