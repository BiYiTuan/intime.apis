using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.data.sync.Tmall
{
    public class ConstValue
    {
        public static string TOP_SERVICE_URL = ConfigurationManager.AppSettings["TOP_SERVICE_URL"] ??
                                               "http://gw.api.taobao.com/router/rest";

        public static string TOP_APP_KEY = ConfigurationManager.AppSettings["TOP_APP_KEY"] ?? "23021668";

        public static string TOP_APP_SECRET = ConfigurationManager.AppSettings["TOP_APP_SECRET"] ?? "baf0dcd2f1ee89159dcccfab0db3368f";

        public static string TOP_SESSION_KEY = ConfigurationManager.AppSettings["TOP_SESSION_KEY"] ?? "6101827124a157b78801f2b9d11bc9e0c2c1f3a2b4c2f5e2247396485";

        public static string IMS_SERVICE_URL = ConfigurationManager.AppSettings["IMS_SERVICE_URL"] ??
                                               "http://111.207.166.195:8088/";

        public static string IMS_APP_KEY = ConfigurationManager.AppSettings["IMS_APP_KEY"] ?? "tmall";

        public static string IMS_APP_SECRET = ConfigurationManager.AppSettings["IMS_APP_SECRET"] ?? "imstest";


    }
}
