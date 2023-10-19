using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Parasale.Models
{
    [Table("Objection")]
    public class Objection
    {
        public int Id { get; set; }
        public string InitialObjection { get; set; }
        public string PitchKeyword { get; set; }
        public string ValidPitchResponse { get; set; }
        public string BadPitchResponse { get; set; }
        public bool? isDummy { get; set; }
        public AppUser User { get; set; }
        public string AssignedAdmin { get; set; }
        public Collection collection { get; set; }

   
    }
}