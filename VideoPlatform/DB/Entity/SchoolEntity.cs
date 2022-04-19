using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    [Table("School")]
    public class SchoolEntity : BaseEntity
    {
        [Key]
        public string school_id { get; set; }
        public string school_name { get; set; }
        public string school_type_code { get; set; }
        public string school_category_code { get; set; }
        public string administrator { get; set; }
        public int is_deleted { get; set; }
        [NotMapped]
        public bool hasChildren { get; set; } = true;
    }
}
