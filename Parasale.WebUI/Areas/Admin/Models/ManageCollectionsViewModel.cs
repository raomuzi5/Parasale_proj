using Parasale.Models;
using Parasale.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Admin.Models
{
    public class ManageCollectionsViewModel
    {
        public ManageCollectionsViewModel()
        {
            Objections = new List<ObjectionViewModel>();
            collections = new List<CollectionListViewModel>();
        }
        public List<ObjectionViewModel> Objections { get; set; }
        public List<CollectionListViewModel> collections { get; set; }
        public List<QuickCollection> QuickCollection { get; set; }
        public string userId { get; set; }
        public int collectionId { get; set; }
        public bool? voiceBoarding { get; set; }
        public string userName { get; set; }
        public string currentStep { get; set; }
        public string path { get; set; }
        public int? StepLevel { get; set; }
    }
}
