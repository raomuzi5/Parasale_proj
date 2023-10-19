using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Admin.Models
{
    public class ObjectionListViewModel
    {
        public ObjectionListViewModel()
        {
            Objections = new List<ObjectionViewModel>();
        }
        public bool isFromAdmin { get; set; }
        public bool isCompleted { get; set; }

        public List<ObjectionViewModel> Objections { get; set; }
    }
}
