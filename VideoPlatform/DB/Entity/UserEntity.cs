using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    [Table("User")]
    public class UserEntity:BaseEntity
    {
        [Key]
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_pwd { get; set; }
        public string real_name { get; set; }

        public string user_role { get; set; }
        public string parent_user { get; set; }
        public string user_school { get; set; }
        public int is_deleted { get; set; }

        public static UserEntity fromJson(string json)
        {
            return JsonConvert.DeserializeObject<UserEntity>(json);
        }
    }
}
