using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.service.PromotionRule
{
   public interface IPromotionSharePolicy
    {
        OrderEntity SourceOrder { get; set; }

        decimal ComputeActualPrice(OrderItemEntity item);
    }
}
