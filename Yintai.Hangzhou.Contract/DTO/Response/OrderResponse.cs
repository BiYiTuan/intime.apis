using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class OrderResponse:BaseResponse
    {
        [DataMember(Name="orderno")]
        public string OrderNo { get; set; }
         [DataMember(Name = "totalamount")]
        public decimal TotalAmount { get; set; }

         [DataMember(Name = "paymentcode")]
         public string PaymentMethodCode { get; set; }
         [DataMember(Name = "paymentname")]
         public string PaymentMethodName { get; set; }
        [IgnoreDataMember]
         public string ExOrderNo { get; set; }
    }
}
