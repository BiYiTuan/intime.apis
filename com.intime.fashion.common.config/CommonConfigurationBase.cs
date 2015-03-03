using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public abstract class CommonConfigurationBase
    {
        private NameValueCollection internalCollection;

        protected abstract string SectionName{get;}

        protected virtual string GetItem(string key)
        {
            EnsureConfiguration();
            return internalCollection[key];
        }

        private void EnsureConfiguration(){
            if (internalCollection == null)
                internalCollection = CommonConfigurationFactory.GetConfiguration("commonIntime",SectionName);
        }
    }
}
