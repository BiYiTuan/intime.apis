using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class ExchangeStoreCouponRequest:AuthRequest
    {
        public int StorePromotionId { get; set; }
        public int Points { get; set; }
        public string IdentityNo { get; set; }
        public int StoreId { get; set; }

    }
}
