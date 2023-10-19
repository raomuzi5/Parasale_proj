using System;
using System.Collections.Generic;
using System.Text;

namespace Parasale.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string PreviousData { get; set; }
        public string NewData { get; set; }
        public string Field { get; set; }
        public string Type { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy  { get; set; }
        public string AdminId { get; set; }
        public string UserId { get; set; }

        public string ManagerId { get; set; }
    }
}
