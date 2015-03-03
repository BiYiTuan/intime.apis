namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public class UpdateItemStockRequest:EntityRequest
    {
        public UpdateItemStockRequest(string itemId, long skuId) : base(null)
        {
            Put(ParamName.Param_ItemId,itemId);
            Put("skuId", skuId.ToString("D"));
        }

        public override string Resource
        {
            get { return "/wgwitem/wgUpdateItemStock.xhtml"; }
        }
    }
}
