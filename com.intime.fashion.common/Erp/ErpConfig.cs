using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Erp
{
   public static  class ErpConfig
    {
       public static string PUBLIC_KEY = ConfigurationManager.AppSettings["Erp_Public_Key"];
       public static string Private_KEY = ConfigurationManager.AppSettings["Erp_Private_Key"];
    }
}
