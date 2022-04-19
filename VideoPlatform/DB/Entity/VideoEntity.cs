using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    [Table("Video")]
    public class VideoEntity:BaseEntity
    {
        [Key]
        public string video_id { get; set; }
        public string video_title { get; set; }
        public string video_brief { get; set; }
        public string video_type { get; set; }
        public int video_year { get; set; }
        public string teacher { get; set; }
        public string award { get; set; }
        public string public_type { get; set; }
        public string public_school { get; set; }
        public string video_school { get; set; }
        public string video_state { get; set; }
        public string video_url { get; set; }
        public DateTime create_time { get; set; }
        public DateTime edit_time { get; set; }
        public int view_count { get; set; }
        public int collection_count { get; set; }
        public int appreciate_count { get; set; }
        public int is_deleted { get; set; }
        public string video_facede { get; set; }
        public string uploader { get; set; }
    }
}
