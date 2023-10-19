using Microsoft.AspNetCore.Mvc.Rendering;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class UserIndexViewModel
    {

        [Required, Display(Name = "Collection")]
        public int collectionId { get; set; }

        public IEnumerable<SelectListItem> collections { get; set; }
        public List<string> completed { get; set; }
        public List<int> completedCount { get; set; }
        public List<int> IncompletedCount { get; set; }
        public bool? VoiceBoarding { get; set; }
        public string lastVoice { get; set; }
        public UserHistory LastUserHistory { get;  set; }
        public string userId { get; set; }
        public string UserName { get; set; }
        public string CurrentStep { get; set; }
        public string Path { get; set; }
        public int? StepLevel { get; set; }
        public VoiceOnBoarding VoiceOnBoarding { get; internal set; }
    }
    public class DashboardIndexViewModel1
    {
        public DashboardIndexViewModel1()
        {
            collections = new List<QuickCollection>();
        }
        public int Users { get; set; }
        public int TotalObjections { get; set; }
        public int TotalActiveUsers { get; set; }
        public double MissedObjections { get; set; }
        public double CompletedObjections { get; set; }
        public List<QuickCollection> collections { get; set; }
        public string userId { get; set; }
        public int collectionId { get; set; }
       

    }
}
