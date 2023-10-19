using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Areas.Manager.Models
{
    public class ObjectionNotificationViewModel
    {
        public long Id { get; set; }
        public string PushedByUserId { get; set; }
        public string PushedToUserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsCleared { get; set; }
        public int ObjectionId { get; set; }
        public int collectionId { get; set; }
    }
}
