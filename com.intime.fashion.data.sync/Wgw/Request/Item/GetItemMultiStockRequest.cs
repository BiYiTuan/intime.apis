namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public sealed class GetItemMultiStockRequest : RequestBase
    {
        public GetItemMultiStockRequest(string itemId)
        {
            Put(ParamName.Param_ItemId, itemId);
        }

        public override string Resource
        {
            get { return "/wgwitem/wgGetItemMultiStock.xhtml"; }
        }
    }
}