
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Order
{
    public class PaymentMethod:BaseModel
    {
        public string PaymentCode { get; set; }
        public string PaymentName { get; set; }
    }
}
