using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Parasale.Models
{
   public class VoiceOnBoardingSubSteps
    {
        public int Id { get; set;}
        public int Step { get; set; }
        public int? SubStep { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string Element { get; set; }
        public string Position { get; set; }
        public bool? IsManager { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsUser { get; set; }
        public bool? IsNextExist { get; set; }
        public bool? IsPreviousExist { get; set; }
        public int? VoiceOnBoardingId { get; set; }
        [ForeignKey("VoiceOnBoardingId")]
        public VoiceOnBoarding VoiceOnBoarding { get; set; }
    }
}
