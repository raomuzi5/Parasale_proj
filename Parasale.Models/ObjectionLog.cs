using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
    public class ObjectionLog
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime ObjectionTime { get; set; }

        public Objection Objection { get; set; }
        public AppUser AppUser { get; set; }
        public string AssignedAdmin { get; set; }
        public bool? isDummy { get; set; }
    }
}
