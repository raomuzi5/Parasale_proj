using Microsoft.AspNetCore.Mvc.Rendering;
using Parasale.WebUI.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class MUnassingedObjectionVM
    {
        public MUnassingedObjectionVM()
        {
            objectionViewModels = new List<ObjectionViewModel>();
        }
        [Required, Display(Name = "Collection")]
        public int collectionId { get; set; }
        public IEnumerable<SelectListItem> collections { get; set; }
        public List<ObjectionViewModel> objectionViewModels { get; set; }
    }
}
