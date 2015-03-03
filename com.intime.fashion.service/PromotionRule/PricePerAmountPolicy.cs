using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.service.PromotionRule
{
    class PricePerAmountPolicy : IPromotionSharePolicy
    {
        public OrderEntity SourceOrder
        {
            get;
            set;
        }

        public decimal ComputeActualPrice(OrderItemEntity item)
        {
            if (SourceOrder.TotalAmount <= 0)
                return 0m;
            var discountAmount = SourceOrder.DiscountAmount ?? 0m;
            var baseAmount = SourceOrder.TotalAmount + discountAmount;
            var priceZquantity = (item.ItemPrice * item.Quantity) / baseAmount;
            return priceZquantity / item.Quantity;
        }
    }
}
