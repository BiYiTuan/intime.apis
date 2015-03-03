using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.message
{
    [Flags]
    public enum MessageAction
    {
        
        CreateEntity = 1,
        UpdateEntity = 2,
        DeleteEntity = 4,
        Paid = 8,
        Approved = 16,
    }
}
