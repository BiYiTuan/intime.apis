using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Repository.Configuratons
{
    internal class ConfigManager
    {
        public static string ConnString()
        {
            return ConfigurationManager.ConnectionStrings["YintaiHangzhouContext"].ConnectionString;
        }
    }
}
