using com.intime.fashion.common.config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.notifyqueue
{
    public class AwsConfig : CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "aws"; }

        }
        public string PUBLIC_KEY { get { return GetItem("public_key"); } }
        public string PRIVATE_KEY { get { return GetItem("private_key"); } }
        public string BASE_URL { get { return GetItem("base_url"); } }
        public string S3_BUCKET_NAME { get { return GetItem("s3_bucket"); } }
        public string QUEUE { get { return GetItem("sqs_queue"); } }


    }
}
