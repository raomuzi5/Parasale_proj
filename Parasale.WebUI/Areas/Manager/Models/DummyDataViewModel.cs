using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.Models
{
    public class DummyDataViewModel
    {
        public List<Collection> collection { get; set; }
        public List<Objection> objection { get; set; }
        public List<QuickCollection> quickCollection { get; set; }
        public List<QuickStartObjections> startObjections { get; set; }
        public List<CCS> cCS { get; set; }
        public List<ObjectionLog> objectionLog { get; set; }
        public List<AppUser> appUsers { get; set; }
        //public Collection collection { get; set; }
        //public Objection objection { get; set; }
        //public QuickCollection quickCollection { get; set; }
        //public QuickStartObjections startObjections { get; set; }
        //public CCS cCS { get; set; }
        //public ObjectionLog objectionLog { get; set; }
        //public AssignedCollection assignedCollection { get; set; }
        //public AppUser appUser { get; set; }
        //public AuditLog auditLog { get; set; }

    }
}
