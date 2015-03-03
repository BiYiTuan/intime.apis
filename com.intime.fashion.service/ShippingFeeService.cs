using com.intime.fashion.service.contract;
using com.intime.fashion.service.ShippingFee;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service
{
    public class ShippingFeeService:BusinessServiceBase,IShippingFeeService
    {
        
        public ShippingFeeService()
        {
            RegisterMatchMethods();
        }

        private void RegisterMatchMethods()
        {
            ShippingFeeRuleFactory.RegisterMatchMethods();
            
        }
        public decimal Calculate(IEnumerable<Yintai.Hangzhou.Model.Order.OrderItem> items)
        {
            if (items.Count() == 0)
                return 0m;
            var item = items.First();
            var matchMethod = ShippingFeeRuleFactory.GetMatchMethods()
                 .FirstOrDefault(m => {
                    return m.Visit(item.ProductId);
                 });
            if (matchMethod == null)
                return 0m;
            var rule = ShippingFeeRuleFactory.Create(matchMethod.RuleType);
            return rule.Calculate(matchMethod.RuleId);
        }
    }
}
