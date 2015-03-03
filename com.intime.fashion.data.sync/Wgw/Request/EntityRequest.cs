using Newtonsoft.Json;
using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Request
{
    public abstract class EntityRequest : RequestBase
    {
        protected EntityRequest(IDictionary<string, object> paramsDict)
        {
            if (paramsDict != null && paramsDict.Count > 0)
            {
                foreach (var p in paramsDict)
                {
                    this.Put(p.Key, p.Value);
                }
            }
        }
    }
}