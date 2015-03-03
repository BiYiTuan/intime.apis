using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.intime.fashion.service.search
{
    public abstract class ESServiceBase
    {
        public abstract bool IndexSingle(int entityId);
    }
}
