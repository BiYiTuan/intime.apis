using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
   public class WxGetPay4HtmlUrlRequest
    {
       public string OrderNo { get; set; }
       public string ClientIp { get; set; }
       public string ReturnUrl { get; set; }
    }
}
