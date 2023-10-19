using Parasale.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class PushedObjectionsViewModel : ObjectionViewModel
    {
        public string PushedBy { get; set; }
        public bool IsPracticed { get; set; }
    }
}
