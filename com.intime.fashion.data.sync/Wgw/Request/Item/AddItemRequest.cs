using System.Collections;
using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request.Item
{
    public sealed class AddItemRequest : EntityRequest
    {
        public AddItemRequest():this(null)
        {
        }

        public AddItemRequest(IDictionary<string,object> entity) : base(entity)
        {
        }

        public override string Resource
        {
            get { return "/wgwitem/wgAddItem.xhtml"; }
        }
    }
}