using Parasale.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class QuickObjectionVM
    {
     
        public int? id { get; set; }
        public List<ObjectionViewModel> Obj { get; set; }
    }
}
