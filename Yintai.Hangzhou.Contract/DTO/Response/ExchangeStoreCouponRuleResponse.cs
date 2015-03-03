using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class ExchangeStoreCouponRuleResponse:BaseResponse
    {
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }
    }
}
