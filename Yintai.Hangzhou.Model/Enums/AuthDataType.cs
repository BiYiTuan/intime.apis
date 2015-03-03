using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum AuthDataType
    {
        [Description("商品")]
        Product = 1,
        [Description("促销")]
        Promotion = 2,
        [Description("订单")]
        Order = 3,
    }
}
