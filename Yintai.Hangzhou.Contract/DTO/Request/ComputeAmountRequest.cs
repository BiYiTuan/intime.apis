using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class ComputeAmountRequest:BaseRequest
    {
        public int ProductId { get; set; }
        [Range(1,100,ErrorMessage="商品数量在1-5之间")]
        public int Quantity { get; set; }
    }
}
