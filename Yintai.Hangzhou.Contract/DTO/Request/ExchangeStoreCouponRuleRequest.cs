using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class ExchangeStoreCouponRuleRequest:AuthRequest
    {
         [DataMember(Name = "storepromotionid")]
        public int StorePromotionId { get; set; }
        [DataMember(Name = "points")]
        public int Points { get; set; }
    }
}
