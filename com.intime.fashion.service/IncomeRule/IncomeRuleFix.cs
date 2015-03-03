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
    internal class IncomeRuleFix:IIncomeRule
    {
        public decimal Compute(int ruleId, decimal price, int quantity)
        {
            var rule = Context.Set<IMS_AssociateIncomeRuleFixEntity>().Where(iar => iar.RuleId == ruleId).FirstOrDefault();
            if (rule == null)
                return 0m;
            return rule.FixAmount;
        }
        private static DbContext Context
        {
            get { return ServiceLocator.Current.Resolve<DbContext>(); }
        }
    }
}
