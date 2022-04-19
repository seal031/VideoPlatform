using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.Models
{
    public class TestResponseModel:BaseResponse
    {
        public int userId { get; set; }
        public string userName { get; set; }
    }
}
