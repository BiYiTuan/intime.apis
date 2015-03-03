using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public sealed class UpdateItemMultiStockRequest : EntityRequest
    {
        public UpdateItemMultiStockRequest(IDictionary<string, object> paramsDict) : base(paramsDict)
        {
        }

        public override string Resource
        {
            get { return "/wgwitem/wgUpdateItemMultiStock.xhtml"; }
        }
    }
}