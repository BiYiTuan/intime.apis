using com.intime.fashion.service.contract;
using com.intime.fashion.service.PromotionRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service
{
   public class PromotionService:BusinessServiceBase
    {
       public  IPromotionSharePolicy GetDefaultSharePolicy()
        {
            return new PricePerAmountPolicy();
        }
    }
}
