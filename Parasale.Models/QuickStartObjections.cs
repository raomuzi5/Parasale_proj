using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
   public class QuickStartObjections
    {
        public int Id { get; set; }
        public string InitialObjection { get; set; }
        public string PitchKeyword { get; set; }
        public string ValidPitchResponse { get; set; }
        public string BadPitchResponse { get; set; }
         public bool? isDummy { get; set; }
        public AppUser User { get; set; }
        public string AssignedAdmin { get; set; }
        public QuickCollection collection { get; set; }

        
    }
}
