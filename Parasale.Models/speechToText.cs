using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Parasale.Models
{
    [Table("SpeechToText")]
    public class speechToText
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Recieved { get; set; }

        public AppUser AppUser { get; set; }
        public string AssignedAdmin { get; set; }
    }
}
