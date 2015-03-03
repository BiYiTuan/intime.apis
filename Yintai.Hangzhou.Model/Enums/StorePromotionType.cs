using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum StorePromotionType
    {
        [Description("代金券")]
        Cachable = 1
    }

    public enum StorePromotionPointType
    { 
        [Description("积点兑换累进")]
        Progressive = 1
    }
}
