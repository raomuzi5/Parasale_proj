using Microsoft.AspNetCore.Mvc.Rendering;
using Parasale.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class ObjectionAddViewModel:ObjectionViewModel
    {
        [Required, Display(Name ="Collection")]
        public int collectionId { get; set; }
        public IEnumerable<SelectListItem> collections { get; set; }
    }
}
