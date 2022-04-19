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
    [Table("appreciate")]
    public class AppreciateEntity
    {
        [Key]
        public int id { get; set; }
        [NotNull]
        public string user_id { get; set; }
        [NotNull]
        public string video_id { get; set; }
        public DateTime appreciate_time { get; set; }
    }
}
