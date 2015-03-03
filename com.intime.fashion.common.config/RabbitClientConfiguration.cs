using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
   public  class RabbitClientConfiguration: CommonConfigurationBase
    {
        private static RabbitClientConfiguration instance = new RabbitClientConfiguration();
        internal RabbitClientConfiguration() { }
        public static RabbitClientConfiguration Current
        {
            get
            {
                return instance;
            }
        }

        public string Host { get { return GetItem("host"); } }
        public string QueueName { get { return GetItem("queue"); } }
        public string FailQueue { get { return GetItem("failqueue"); } }

        protected override string SectionName
        {
            get
            {
                return "rabbit";
            }
        }

        public string Password { get { return GetItem("password"); } }

        public string UserName { get { return GetItem("username"); } }
    }
}
