using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Parasale.Models
{
    [Table("ObjectionNotification")]
    public class ObjectionNotification
    {
        public long Id { get; set; }
        public string PushedByUserId { get; set; }
        public string PushedToUserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public Objection Objection { get; set; }
        public bool IsCleared { get; set; }
        public string AssignedAdmin { get; set; }
        
    }
}
