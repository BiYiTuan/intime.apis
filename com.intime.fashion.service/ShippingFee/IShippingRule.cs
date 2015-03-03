using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.intime.fashion.service.ShippingFee
{
    public interface IShippingRule
    {
        decimal Calculate(int ruleId);
    }
}
