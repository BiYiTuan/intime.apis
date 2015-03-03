
using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Order
{
    public class QueryOrderListRequest:RequestBase
    {
        public QueryOrderListRequest(IEnumerable<KeyValuePair<string, string>> paramsDictionary)
        {
            foreach (var kv in paramsDictionary)
            {
                Put(kv.Key,kv.Value);
            }
        }

        public override string Resource
        {
            get { return "/wgwdeal/wgQueryDealList.xhtml"; }
        }
    }
}
