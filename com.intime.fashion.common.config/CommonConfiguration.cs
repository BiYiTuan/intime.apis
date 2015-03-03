using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public class CommonConfiguration<T> where T:CommonConfigurationBase,new()
    {
        private  static T instance = new T();
        private CommonConfiguration() { }
        public static T Current { get {
            return instance;
        } }
    }
}
