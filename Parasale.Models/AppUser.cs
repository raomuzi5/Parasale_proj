using Microsoft.AspNetCore.Identity;
using System;

namespace Parasale.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime LastNotifiedOn { get; set; }
        public int LastObjectionCount { get; set; }
        public bool IsManager { get; set; } = false;
        public string AssignedManager { get; set; }
        public bool IsCompanyAdmin { get; set; }
        public string AssginedAdmin { get; set; }
        public string currentStep { get; set; }
        public int? StepLevel { get; set; }
        public int? SubStep { get; set; }
        public string StepTitle { get; set; }
        public bool? voiceOnboarding { get; set; }
        public string path { get; set; }
        public string lastVoice { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}
