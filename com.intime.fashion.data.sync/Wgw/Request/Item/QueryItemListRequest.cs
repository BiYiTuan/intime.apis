using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public sealed class QueryItemListRequest:RequestBase
    {
        public override string Resource
        {
            get { return "/wgwitem/wgQueryItemList.xhtml"; }
        }
    }
}
