using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
   public class WxGetPay4AppTokenRequest
    {
       [Required(ErrorMessage="订单号必须")]
       public string OrderNo { get; set; }
    }
}
