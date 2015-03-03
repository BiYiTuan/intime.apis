using com.intime.fashion.data.sync.Wgw.Request.Order;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public class QueryOrderRequestBuilder : RequestParamsBuilder
    {
        public QueryOrderRequestBuilder(ISyncRequest request) : base(request)
        {
        }

        public override ISyncRequest BuildParameters(object entity)
        {
            Request.Put("historyDeal","0");
            //Request.Put("timeType","PAY");
            Request.Put("listItem","0");
            Request.Put("orderDesc","0");
            //Request.Put("dealState", OrderStatusConst.STATE_WG_PAY_OK);
            return Request;
        }
    }
}
