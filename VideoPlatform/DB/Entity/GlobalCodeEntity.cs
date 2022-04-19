using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    [Table("Globalcode")]
    public class GlobalCodeEntity : BaseEntity
    {
        [Key]
        public string code_id { get; set; }

        public string code_name { get; set; }
        public string parent_code { get; set; }
        public int is_deleted { get; set; }
    }
}
