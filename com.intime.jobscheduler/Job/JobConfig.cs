using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.jobscheduler.Job
{
    public static class JobConfig
    {
        public static readonly int DEFAULT_PAGE_SIZE = int.Parse(ConfigurationManager.AppSettings["PageSize"]);

        public static readonly string WGW_API_BASE_URL = ConfigurationManager.AppSettings["Wgw_api_base_url"];
    }
}
