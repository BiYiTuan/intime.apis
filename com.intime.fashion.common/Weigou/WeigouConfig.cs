using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Weigou
{
    public static class WeigouConfig
    {
        public static string BASE_URL = ConfigurationManager.AppSettings["Wg_Base_Url"];
        public static string MESSAGE_TARGET_URL = ConfigurationManager.AppSettings["Message_TargetUrl"];
    }
}
