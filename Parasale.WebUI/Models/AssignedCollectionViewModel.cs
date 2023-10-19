using Parasale.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.WebUI.Models
{
    public class AssignedCollectionViewModel
    {

        public long Id { get; set; }
        public string PushedToUserId { get; set; }
        public int collectionId { get; set; }
    }

    public partial class ObjectionVM
    {
        public int? id { get; set; }
        public List<ObjectionViewModel> Obj { get; set; }

    }
}
