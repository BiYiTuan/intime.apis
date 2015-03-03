using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum OutboundType
    {
        [Description("订单出库")]
        Order = 1,
        [Description("换货出库")]
        RMA = 2
    }
}
