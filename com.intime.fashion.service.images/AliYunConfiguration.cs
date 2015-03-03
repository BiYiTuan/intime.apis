using com.intime.fashion.common.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.images
{
    public class AliYunConfiguration : CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "aliyun"; }

        }
        public string ACCESS_ID { get { return GetItem("access_id"); } }
        public string ACCESS_KEY { get { return GetItem("access_key"); } }
        public string RSS_BUCKET_NAME { get { return GetItem("bucket_name"); } }
        public string END_POINT { get { return GetItem("end_point"); } }


    }
}
