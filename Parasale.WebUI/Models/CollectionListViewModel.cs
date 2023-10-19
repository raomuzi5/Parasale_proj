using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class CollectionListViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Collection Name")]
        public string CollectionName { get; set; }
        public string UserId { get; set; }
        public string ManagerUserId { get; set; }
        public bool IsCollectionAssigned { get; set; }
        public string AssignedAdmin {get;set;}
    public bool IsAdmin { get; set; }


    }
}
