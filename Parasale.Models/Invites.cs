using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
    public class Invites
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public bool IsSigned { get; set; }
        public string AssignedAdmin { get; set; }


}
}
