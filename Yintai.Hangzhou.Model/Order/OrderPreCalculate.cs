using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model.Order
{
    public class OrderPreCalculate : BaseModel
    {
        public IEnumerable<OrderPreCalculateItem> Products { get; set; }
        public int ComboId { get; set; }
        public OrderPreCalculateType CalculateType { get; set; }
    }

    public class OrderPreCalculateItem : BaseModel
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
