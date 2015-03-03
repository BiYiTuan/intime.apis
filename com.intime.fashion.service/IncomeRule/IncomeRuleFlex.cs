using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.service.IncomeRule
{
    public class IncomeRuleFlex:IIncomeRule
    {
        public decimal Compute(int ruleId, decimal price, int quantity)
        {
            var benchAmount = price * quantity;
            var rule = Context.Set<IMS_AssociateIncomeRuleFlexEntity>().Where(iar => iar.RuleId == ruleId && iar.BenchFrom <= benchAmount
                                && iar.BenchTo > benchAmount).FirstOrDefault();
            if (rule == null)
                return 0m;
            return benchAmount * rule.Percentage.Value;
        }
        private static DbContext Context
        {
            get { return ServiceLocator.Current.Resolve<DbContext>(); }
        }
    }
}
