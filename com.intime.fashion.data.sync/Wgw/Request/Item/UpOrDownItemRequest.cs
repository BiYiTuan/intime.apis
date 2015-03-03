using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public abstract class UpOrDownItemRequest : RequestBase
    {
        protected UpOrDownItemRequest()
        {
        }

        protected UpOrDownItemRequest(IEnumerable<string> itemIds)
        {
            Put(ParamName.Param_ItemList, string.Join("|", itemIds));
        }
    }
}
