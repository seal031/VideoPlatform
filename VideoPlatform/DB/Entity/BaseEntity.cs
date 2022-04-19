using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.DB.Entity
{
    public class BaseEntity
    {
        /// <summary>
        /// 管理员操作人id
        /// </summary>
        [NotMapped]
        public string admin_id { get; set; }

        /// <summary>
        /// 管理员操作人IP
        /// </summary>
        [NotMapped]
        public string admin_ip { get; set; }

        /// <summary>
        /// 用于所有add、del方法中的管理员操作记录
        /// </summary>
        /// <returns></returns>
        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
