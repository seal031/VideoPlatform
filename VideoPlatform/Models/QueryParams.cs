using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoPlatform.Models
{
    public class BaseQueryParams
    {
        public int? pageIndex { get; set; }
        public int? pageSize { get; set; }
    }
    public class VideoQueryParams:BaseQueryParams
    {
        public string videoId { get; set; }
        public string userId { get; set; }
        public string videoType { get; set; }

        public int? videoYear { get; set; }
        public string keyword { get; set; }
        public string publicType { get; set; }

        public string videoState { get; set; }

        public string uploader { get; set; }
        public string publicSchool { get; set; }
        public string videoSchool { get; set; }
        public string userRole { get; set; }

        public string orderBy { get; set; }
        public int? topN { get; set; }
    }

    public class BriefQueryParams:BaseQueryParams
    {
        public string briefType { get; set; }
        public string keyword { get; set; }
        public string briefState { get; set; }
        public int? topN { get; set; }
    }

    public class SchoolQueryParams : BaseQueryParams
    {
        public string schoolType { get; set; }

        public string schoolCategoly { get; set; }
        public string keyword { get; set; }
    }
}
