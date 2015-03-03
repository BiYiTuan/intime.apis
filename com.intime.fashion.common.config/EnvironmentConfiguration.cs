using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public class EnvironmentConfiguration:CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "env"; }
        }
        public string Name { get { return GetItem("name"); } }

        public bool IsProduction { get { return string.Compare(Name,"Production",true)==0; } }
  
    }
}
