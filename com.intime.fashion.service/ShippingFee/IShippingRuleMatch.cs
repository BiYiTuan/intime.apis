using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.ShippingFee
{
    public interface IShippingRuleMatch
    {
        bool Visit(int productId);

        ShippingRuleType RuleType { get; }
        
        int RuleId { get;}
    }
}
