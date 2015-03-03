namespace com.intime.fashion.data.sync.Wgw.Request.Order
{
    public class QueryOrderDetailRequest : RequestBase
    {
        public QueryOrderDetailRequest(string dealId)
        {
            Put("dealId", dealId);
        }

        public override string Resource
        {
            get { return "/wgwdeal/wgQueryDealDetail.xhtml"; }
        }
    }
}
