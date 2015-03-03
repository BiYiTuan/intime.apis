using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Order
{
    public class OrderPreCalculateResult
    {
        public int TotalQuantity { get; set; }
        public int TotalPoints { get; set; }
        public decimal TotalFee { get; set; }
        public decimal ExtendPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
