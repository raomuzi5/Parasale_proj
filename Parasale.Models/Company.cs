using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AppUser appUser { get; set; }
    }
}
