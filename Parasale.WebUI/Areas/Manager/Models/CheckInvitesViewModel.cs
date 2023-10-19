using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.Models
{
    public class CheckInvitesViewModel
    {
        public InvitesByManager invites { get; set; }
        public AppUser appUser { get; set; }
        public List<InvitesByManager> invitesList { get; set; }
        public List<AppUser> appUserList { get; set; }
    }
}
