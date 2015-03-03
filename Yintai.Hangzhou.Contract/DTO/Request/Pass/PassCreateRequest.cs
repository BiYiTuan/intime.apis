using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Contract.Request;

namespace Yintai.Hangzhou.Contract.DTO.Request.Pass
{
    public class PassCreateRequest: AuthRequest
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }
    }
}
