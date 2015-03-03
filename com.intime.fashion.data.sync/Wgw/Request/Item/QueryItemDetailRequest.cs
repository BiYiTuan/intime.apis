using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public class QueryItemDetailRequest:RequestBase
    {
        public override string Resource
        {
            get { return "/wgwitem/wgQueryItemDetail.xhtml"; }
        }
    }
}
