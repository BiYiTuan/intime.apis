using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.ShippingFee
{
    public static class ShippingFeeRuleFactory
    {
        private static object _lockObject = new object();
        private static List<IShippingRuleMatch> _matchMethods = null;
        public static IShippingRule Create(ShippingRuleType ruleType)
        {
            switch (ruleType)
            { 
                case ShippingRuleType.Fix:
                    return new ShippingRuleFix();
                default:
                    throw new NotSupportedException();
            }
        }
        public static IEnumerable<IShippingRuleMatch> GetMatchMethods()
        {
            return _matchMethods;
        }
        public static void RegisterMatchMethods()
        {
            if (_matchMethods == null)
            {
                lock (_lockObject)
                {
                    if (_matchMethods == null)
                    {
                        _matchMethods = new List<IShippingRuleMatch>();
                        _matchMethods.Add(new StoreMatchMethod());
                    }
                }
            }
        }

    }
}
