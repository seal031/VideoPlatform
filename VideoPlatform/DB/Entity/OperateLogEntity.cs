using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    [Table("operate_log")]
    public class OperateLogEntity
    {

        [Key]
        public int id { get; set; }
        [NotNull]
        public string user_id { get; set; }
        public string user_ip { get; set; }
        public string operate_content { get; set; }
        public DateTime operate_time { get; set; }
    }
}
