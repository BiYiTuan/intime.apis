using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.service.ShippingFee
{
    class StoreMatchMethod:IShippingRuleMatch
    {
        private ShippingRuleType? _ruleType = null;
        private int? _ruleId ;
        public bool Visit(int productId)
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            var productEntity = db.Set<ProductEntity>().Find(productId);
           var matchEntity= db.Set<ShippingRuleEntity>().Where(s => s.Status == (int)DataStatus.Normal &&
                    s.FromDate <= DateTime.Now &&
                    s.EndDate > DateTime.Now &&
                    s.MatchMethod == (int)ShippingRuleMatchMethod.Store &&
                    s.MatchId == productEntity.Store_Id).FirstOrDefault();
           if (matchEntity == null)
               return false;
            _ruleType = (ShippingRuleType)matchEntity.RuleType;
            _ruleId = matchEntity.Id;
            return true;
        }

        public ShippingRuleType RuleType
        {
            get { return _ruleType.Value; }
        }

        public int RuleId
        {
            get { return _ruleId.Value; }
        }
    }
}
