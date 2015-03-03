using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public class ElasticSearchConfigurationSetting : CommonConfigurationBase
    {
        private static ElasticSearchConfigurationSetting instance = new ElasticSearchConfigurationSetting();
        internal ElasticSearchConfigurationSetting() { }
        public static ElasticSearchConfigurationSetting Current
        {
            get
            {
                return instance;
            }
        }

        public string Host { get { return GetItem("host"); } }
        public string Index { get { return GetItem("index"); } }

        protected override string SectionName
        {
            get
            {
                return "elasticSearch";
            }
        }
    }
}
