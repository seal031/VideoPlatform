using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    [Table("collect")]
    public class CollectEntity:BaseEntity
    {
        [Key]
        public int id { get; set; }
        [NotNull]
        public string user_id { get; set; }
        [NotNull]
        public string video_id { get; set; }
        public DateTime collect_time { get; set; }
    }
}
