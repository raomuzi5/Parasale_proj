using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Parasale.Models
{
    [Table("VoiceOnBoardings")]
    public class VoiceOnBoarding
    {
        public int Id { get; set; }
        public int Step { get; set; }
        public int? SubStep { get; set; }
        //public string UserId { get; set; }
        public bool? IsStartupPopUp { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public string Element { get; set; }
        public string Position { get; set; }
        public string Url { get; set; }
        public bool? IsManager { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsUser { get; set; }
        public bool? IsNextExist { get; set; }
        public bool? IsPreviousExist { get; set; }
        public int? ParentVOBId { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsIntro { get; set; }
        public virtual VoiceOnBoarding ParentVOB { get; set; }
        public virtual ICollection<VoiceOnBoarding> SubordinateVOBS { get; set; }
        public VoiceOnBoarding()
        {
            SubordinateVOBS = new HashSet<VoiceOnBoarding>();
        }
    }
}
