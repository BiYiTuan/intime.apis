using System.Collections.Generic;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public List<CouponHistoryEntity> Get(int userId, int sourceId, SourceType sourceType)
        {
            return _couponRepository.GetListByUserIdSourceId(userId, sourceId, sourceType);
        }

        public int GetCouponCount(int sourceId, SourceType sourceType)
        {
            return _couponRepository.Get4Count(sourceId, sourceType);
        }
    }
}
