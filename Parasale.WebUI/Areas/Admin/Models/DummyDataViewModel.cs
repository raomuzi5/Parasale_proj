using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Admin.Models
{
    public class DummyDataViewModel
    {
        public List<Collection> collection { get; set; }
        public List<Objection> objection { get; set; }
        public List<QuickCollection> quickCollection { get; set; }
        public List<QuickStartObjections> startObjections {get; set;}
        public List<CCS> cCS { get; set; }
        public List<ObjectionLog> objectionLog {get; set;}
        public List<AppUser> appUsers { get; set; }

    }
}
