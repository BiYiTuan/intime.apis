using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace com.intime.fashion.webapi.domain.Request
{
    public class IMSOrderFillPromotionRequest:BaseRequest
    {
        [Required(ErrorMessage = "订单号为空")]
        [MinLength(2, ErrorMessage = "订单号长度不正确")]
        public string OrderNo { get; set; }
        public IEnumerable<IMSOrderItemFillPromotion> Items { get; set; }
        public string PromotionDesc { get; set; }
        public string PromotionRules { get; set; }
    }

    public class IMSOrderItemFillPromotion
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "销售码为空")]
        [MinLength(2, ErrorMessage = "销售码长度不正确")]
        public string Sales_Code { get; set; }
    }
}
