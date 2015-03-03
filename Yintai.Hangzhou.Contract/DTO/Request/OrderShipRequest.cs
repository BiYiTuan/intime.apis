using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
   public class OrderShipRequest:BaseRequest,IValidatableObject
    {
       public string OrderNo { get; set; }
       public string Sales_Sid { get; set; }
       public int ShipVia { get; set; }
       [Required(ErrorMessage = "ShipViaName 不能为空")]
       public string ShipViaName { get; set; }
       public string ShipNo { get; set; }
       public IEnumerable<OrderProductDetailRequest> Products { get; set; }
        [Required(ErrorMessage = "UpdateTime 不能为空")]
       public DateTime UpdateTime { get; set; }
       public int StoreId { get; set; }
       public bool ForceShip { get; set; }

       public bool IsOrderShip { get {
           return !string.IsNullOrEmpty(OrderNo);
       } }


       public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
       {
           if (string.IsNullOrEmpty(OrderNo) && string.IsNullOrEmpty(Sales_Sid))
               yield return new ValidationResult("订单号或销售sid为空");
       }
    }
}
