using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public class WeixinConfiguration:CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "weixin_common"; }

        }
        public string ServiceBaseUri { get { return GetItem("weixin_service_base"); } }

    }
}
