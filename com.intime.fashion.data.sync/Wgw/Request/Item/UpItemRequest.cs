using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public sealed class UpItemRequest : UpOrDownItemRequest
    {
        public UpItemRequest()
        {
        }

        public UpItemRequest(IEnumerable<string> itemIds):base(itemIds)
        {
        }
        public override string Resource
        {
            get { return "/wgwitem/wgUpItem.xhtml"; }
        }
    }
}