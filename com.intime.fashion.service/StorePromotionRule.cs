using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace com.intime.fashion.service
{
    public class StorePromotionRule
    {
        public static string CreateCode(int proId)
        {
            var code = string.Concat(string.Format("9{0}{1}", proId.ToString().Length, proId)
                        , DateTime.UtcNow.Ticks.ToString().Reverse().Take(8)
                            .Aggregate(new StringBuilder(),(s,e)=>s.Append(e),s=>s.ToString())
                            .PadRight(8,'0'));
            IStoreCouponsRepository couponData = ServiceLocator.Current.Resolve<IStoreCouponsRepository>();
            var existingCodes = couponData.Get(c => c.Code == code && c.StorePromotionId == proId).Count();
            if (existingCodes > 0)
                code = string.Concat(code, (existingCodes + 1).ToString());
            return code;
        }
        public static decimal AmountFromPoints(int proId, int points)
        {
            IPointOrderRuleRepository proRuleRepo = ServiceLocator.Current.Resolve<IPointOrderRuleRepository>();
            var pro = proRuleRepo.Get(r=>r.StorePromotionId==proId && r.Status!=(int)DataStatus.Deleted);
            decimal ratio = 0m;
            foreach (var rule in pro.ToList())
            { 
 
                // hit rule be:
                // 1. rangfrom is null but rangto >points
                // 2. rangfrom is not null, and rangfrom <=points, and rangto is not null, rangto>points
                // 3. rangefrom is not null, and rangefrom <=points, and rangto is null
                if (!rule.RangeFrom.HasValue && rule.RangeTo.HasValue && rule.RangeTo > points)
                {
                    ratio = rule.Ratio.Value;
                    break;
                }
                if (rule.RangeFrom.HasValue && rule.RangeFrom<= points && 
                    (!rule.RangeTo.HasValue || (rule.RangeTo.HasValue && rule.RangeTo>points)))
                {
                    ratio = rule.Ratio.Value;
                    break;
                }
            }
            if (ratio <= 0)
                throw new ApplicationException("规则设置有误!");
            return points / 100 * ratio;

        }
    }
}
