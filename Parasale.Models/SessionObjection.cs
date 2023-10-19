using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
   public class SessionObjection
    {
        public int Id { get; set; }
        public UserSession session { get; set; }
        public Objection objection { get; set; }
        public ObjectionLog objectionLog { get; set; }
        public CCS cCS{ get; set; }
    }
}
