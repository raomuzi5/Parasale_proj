using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
   public class UserSession
    {
        public int Id { get; set; }
        public int SessionID { get; set; }
        public string SessionName { get; set; }
        public Nullable<DateTime> SessionStart { get; set; }
        public Nullable<DateTime> SessionEnd { get; set; }
        public AppUser appUser { get; set; }
    }
}
