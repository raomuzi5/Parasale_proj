using Microsoft.AspNetCore.Identity;
using System;

namespace Parasale.Models
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
