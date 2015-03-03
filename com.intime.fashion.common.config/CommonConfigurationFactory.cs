using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
   public static  class CommonConfigurationFactory
    {
       public static NameValueCollection GetConfiguration(string groupName, string sectionName)
       {
           if (string.IsNullOrEmpty(sectionName))
               throw new ArgumentNullException(sectionName);
           if (!string.IsNullOrEmpty(groupName))
               groupName = groupName + "/";
           return ConfigurationManager.GetSection(string.Concat(groupName, sectionName)) as NameValueCollection;
       }
    }
}
