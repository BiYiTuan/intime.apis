using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.contract
{
    public interface IShippingFeeService
    {
        decimal Calculate(IEnumerable<Yintai.Hangzhou.Model.Order.OrderItem> enumerable);
    }
}
