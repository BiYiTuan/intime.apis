using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.o2o.data.exchange.IT;

namespace com.intime.o2o.data.exchange.Ims.Request
{
    public class CreateOrderRequest:ImsRequest<dynamic,Response<dynamic>>
    {
        public override string GetResourceUri()
        {
            return "gg/Order/CreateOrder";
        }
    }
}
