using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.service.ShippingFee
{
    class ShippingRuleFix:IShippingRule
    {
        public decimal Calculate( int ruleId)
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            var ruleEntity = db.Set<ShippingRuleFixEntity>().Where(sr=>sr.RuleId == ruleId).FirstOrDefault();
            if (ruleEntity == null)
                return 0m;
            return ruleEntity.Amount;
        }
    }
}
