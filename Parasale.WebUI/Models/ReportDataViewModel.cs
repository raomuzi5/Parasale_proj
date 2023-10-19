using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class ReportDataViewModel
    {
        public List<string> Name { get; set; }
        public string type { get; set; }
        public string Id { get; set; }
        public Collection collection { get;set; }
        public AppUser appUser { get; set; }
        public List<Collection> collectionList { get; set; }
        public List<AppUser> appUserList { get; set; }
        public string Names { get; set; }
        public int Ids { get; set; }

    }
}
