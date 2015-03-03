
using com.intime.o2o.data.exchange.Ims.Response;

namespace com.intime.o2o.data.exchange.Ims.Request
{
    public class QueryOrderStatusRequest : ImsRequest<dynamic, QueryOrderStatusResponse>
    {
        public override string GetResourceUri()
        {
            return "gg/Order/QueryOrderStatus";
        }
    }
}
