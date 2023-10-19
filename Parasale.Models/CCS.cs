using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
    public class CCS
    {
        public int Id { get; set; }

        public Double? TotalScore { get; set; }
        public Double? WPMTarget { get; set; }

        public Double? WPMMeasure { get; set; }
        public Double? WPMScore { get; set; }
        public Double? WPRTarget { get; set; }
        public Double? WPRMeasure { get; set; }

        public Double? WPRScore { get; set; }
        public Double? RPATarget { get; set; }
        public Double? RPA { get; set; }
        public Double? RPAScore { get; set; }
        public DateTime? TimeStamp { get; set; }
        public bool? isDummy { get; set; }
        public AppUser appUser { get; set; }
        public Objection objection { get; set; }
    }
}
