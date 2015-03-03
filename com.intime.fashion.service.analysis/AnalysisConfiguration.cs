using com.intime.fashion.common.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.analysis
{
    class AnalysisConfiguration:CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "flurry"; }

        }
        public string ServiceBaseUri { get { return GetItem("api_base_uri"); } }

        public string ApiAccessCode { get{ return GetItem("access_code"); }}

        public string ApiKey { get { return GetItem("api_key"); } }

        public string AppName { get { return GetItem("app_name"); } }
    }
}
