using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
   public class AssignedCollection
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public string UserId { get; set; }
        public AppUser appUser { get; set; }
        public Collection collection { get; set; }
        public string AssignedAdmin { get; set; }
    }
}
