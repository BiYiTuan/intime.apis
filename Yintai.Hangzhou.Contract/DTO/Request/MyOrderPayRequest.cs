using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class MyOrderPayRequest:BaseRequest
    {
        public string OrderNo { get; set; }
        public decimal Amount { get; set; }
        public string PayTransNo { get; set; }
    }
}
