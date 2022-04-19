using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.Models
{
    public class BaseResponse
    {
        public string resultCode { get; set; } = "200";

        public object data { get; set; } = null;

        public string message { get; set; } = null;

        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static string makeAbnormalResponse2str(string code, string message)
        {
            BaseResponse response = new BaseResponse()
            {
                resultCode=code,
                message=message
            };
            return response.toJson();
        }
    }
}
