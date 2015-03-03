using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.DTO.Request.Coupon;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class StoreCouponListRequest:AuthPagerInfoRequest
    {
        [DataMember(Name = "type")]
        public CouponRequestType? Type { get; set; }
    }
}
