using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
   public class RMAReceivedRequest:BaseRequest
    {
       [Required(ErrorMessage="RMANo 必须")]
       public string RMANo { get; set; }
       public DateTime UpdateTime { get; set; }
    }
}
