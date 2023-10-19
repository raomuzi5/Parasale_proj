using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
    class VoiceOnBoardingHistory
    {
        public int Id { get; set; }
        public bool? IsSavedForLater { get; set; }
        public bool? IsTourFinished { get; set; }
        public VoiceOnBoarding boarding { get; set; }
        public AppUser appUser { get; set; }
    }
}
