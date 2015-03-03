using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public class Cache_AuthkeyConfiguration:CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "cache_auth"; }
        }
        public string Host { get { return GetItem("host"); } }
        public string Port { get { return GetItem("port"); } }
        public string UserName { get { return GetItem("user_name"); } }
        public string Password { get { return GetItem("pass_word"); } }
        public string Prefix { get { return GetItem("prefix"); } }
    }
}
