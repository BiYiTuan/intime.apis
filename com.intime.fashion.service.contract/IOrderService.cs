using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Order;

namespace com.intime.fashion.service.contract
{
    public interface IOrderService
    {
        bool CanChangePro(OrderEntity order);

        bool IsAssociateOrder(int authuid, OrderEntity order);

        BusinessResult<OrderCreateResult> Create(OrderCreate request, UserModel authUser, bool needShippingFee = true);

        OrderPreCalculateResult PreCalculate(OrderPreCalculate request);
    }
}
