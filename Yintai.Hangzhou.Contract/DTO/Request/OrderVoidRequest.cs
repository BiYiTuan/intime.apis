using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class OrderVoidRequest:BaseRequest
    {
        [Required(ErrorMessage = "OrderNo 不能为空")]
        public string OrderNo { get; set; }
       
        public DateTime? UpdateTime { get; set; }
    }
}
