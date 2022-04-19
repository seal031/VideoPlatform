using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    [Table("brief")]
    public class BriefEntity:BaseEntity
    {
        [Key]
        public string brief_id { get; set; }
        public string brief_type{ get; set; }
        public string brief_title { get; set; }
        public string brief_content { get; set; }
        public string brief_state { get; set; }
        public DateTime create_time { get; set; }
        public string operate_admin { get; set; }
        public string brief_image { get; set; }
        public int is_deleted { get; set; }
    }
}
