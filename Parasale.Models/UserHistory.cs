using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Parasale.Models
{
    [Table("UserHistory")]
    public class UserHistory
    {
        public int UserHistoryId { get; set; }
        public int? DialectId { get; set; }
        public string LanguageDialect { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
        public int CollectionId { get; set; }
        [ForeignKey("CollectionId")]
        public Collection Collection { get; set; }
        public string DialectDataName { get; set; }
        public int? DialectDataId { get; set; }
    }
}
