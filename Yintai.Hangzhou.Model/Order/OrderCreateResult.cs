using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Order
{
    public class OrderCreateResult:BaseModel
    {
        public string OrderNo { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethodCode { get; set; }

        public string PaymentMethodName { get; set; }

        public string ExOrderNo { get; set; }
    }
}
