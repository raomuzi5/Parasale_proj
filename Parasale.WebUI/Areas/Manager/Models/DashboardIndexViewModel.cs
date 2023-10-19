using Parasale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.Models
{
    public class DashboardIndexViewModel
    {
        public DashboardIndexViewModel()
        {
            Objections = new List<ObjectionViewModel>();
            collections = new List<CollectionListViewModel>();
        }
        public int Users { get; set; }
        public int TotalObjections { get; set; }
        public int TotalActiveUsers { get; set; }
        public double MissedObjections { get; set; }
        public double CompletedObjections { get; set; }
        public List<ObjectionViewModel> Objections { get; set; }
        public List<CollectionListViewModel> collections { get; set; }
        public bool? voiceboarding { get; set; }
        public string currentStep { get; set; }
    }
}
