using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.Models
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int MissedCount { get; set; }
        public int CompletedCount { get; set; }
        public bool IsManager { get; set; }
        public bool IsAlreadyTeamMember { get; set; }
        public bool IsAlreadyNotified { get; set; }
        public string ManagerUserId { get; set; }
        public int objectionId { get; set; }
    }
}
