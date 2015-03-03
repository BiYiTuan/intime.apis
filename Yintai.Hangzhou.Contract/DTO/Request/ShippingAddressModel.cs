using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
   public class ShippingAddressModel
    {
       [Required(ErrorMessage = "联系人不能为空")]
       public string ShippingContactPerson { get; set; }
       [Required(ErrorMessage = "联系电话不能为空")]
       public string ShippingContactPhone { get; set; }
       [Required(ErrorMessage="邮编不能为空")]
       public string ShippingZipCode { get; set; }
       [Required(ErrorMessage = "送货地址不能为空")]
       public string ShippingAddress { get; set; }
    }
}
