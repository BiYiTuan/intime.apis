using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public class GetBrandListRequest:RequestBase
    {
        public override string Resource
        {
            get { return "/brand/getBrandList.xhtml"; }
        }
    }
}
