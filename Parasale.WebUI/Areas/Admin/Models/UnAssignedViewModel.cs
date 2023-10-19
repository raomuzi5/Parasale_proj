using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Admin.Models
{
    public class UnAssignedViewModel
    {
        public UnAssignedViewModel()
        {
            objectionViewModels = new List<ObjectionViewModel>();
        }
        [Required, Display(Name = "Collection")]
        public int collectionId { get; set; }
        public IEnumerable<SelectListItem> collections { get; set; }
        public List<ObjectionViewModel> objectionViewModels { get; set; }
    }
}
