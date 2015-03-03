using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public sealed class DownItemRequest : UpOrDownItemRequest
    {
        public DownItemRequest()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="itemIds">需要下架的商品id</param>
        public DownItemRequest(IEnumerable<string> itemIds) : base(itemIds)
        {
           
        }

        public override string Resource
        {
            get { return "/wgwitem/wgDownItem.xhtml"; }
        }
    }
}