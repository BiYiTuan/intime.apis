using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum CouponActionType
    {
        [Description("兑换礼券")]
        Create = 1,
        [Description("消费礼券")]
        Consume =2,
        [Description("作废礼券")]
        Void = 3,
        [Description("退回礼券")]
        Rebate = 4
    }
    public enum CouponType
    {
      StorePromotion = 1,
      Promotion = 2
    }
}
