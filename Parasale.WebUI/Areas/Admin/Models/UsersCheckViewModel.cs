using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Admin.Models
{
    public class UsersCheckViewModel
    {
        public Invites invites { get; set; }
        public AppUser appUser { get; set; }
        public List<Invites> invitesList { get; set; }
        public List<AppUser> appUserList { get; set; }
    }
}
