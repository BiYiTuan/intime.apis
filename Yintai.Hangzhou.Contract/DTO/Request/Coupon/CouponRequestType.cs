using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request.Coupon
{
    public enum CouponRequestType:uint
    {
        All = 0,
        UnUsed = 1,
        Used = 2,
        Expired = 3
    }
}
