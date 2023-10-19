using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public int SessionID { get; set; }
        public string SessionName { get; set; }
        public Nullable<DateTime> SessionStart { get; set; }
        public Nullable<DateTime> SessionEnd { get; set; }

    }
}
