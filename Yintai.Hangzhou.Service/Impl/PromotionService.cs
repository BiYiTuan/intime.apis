using System;
using System.Collections.Generic;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using System.Linq;
using Yintai.Hangzhou.Contract.DTO.Request.Promotion;

namespace Yintai.Hangzhou.Service.Impl
{
    public class PromotionService : BaseService, IPromotionService
    {
        private readonly IPromotionProductRelationRepository _pprRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly ICouponService _couponService;
        private ICouponRepository _couponRepo;

        public PromotionService(ICouponService couponService, IPromotionRepository promotionRepository, IPromotionProductRelationRepository pprRepository
            ,ICouponRepository couponRepo)
        {
            _pprRepository = pprRepository;
            _promotionRepository = promotionRepository;
            _couponService = couponService;
            _couponRepo = couponRepo;
        }

        public bool Exists(int promotionid, int productid)
        {
            return _pprRepository.Exists(promotionid, productid);
        }

        public PromotionEntity Get(int promotionid)
        {
            return _promotionRepository.GetItem(promotionid);
        }

        public string Verification(PromotionEntity promotionEntity)
        {
            if (promotionEntity == null)
            {
                return "活动不存在";
            }

            if (promotionEntity.StartDate > DateTime.Now)
            {
                return "活动还没有开始";
            }

            if (promotionEntity.EndDate < DateTime.Now)
            {
                return "活动已经结束";
            }

            if (promotionEntity.PublicationLimit != null && promotionEntity.PublicationLimit > 0)
            {
                //验证优惠券领取数
                var c = _couponService.GetCouponCount(promotionEntity.Id, SourceType.Promotion);
                if (promotionEntity.PublicationLimit <= c)
                {
                    return "活动优惠码已售罄";
                }
            }

            return null;
        }

        public string Verification(int promotionId)
        {
            return Verification(Get(promotionId));
        }

        public string Verification(PromotionCouponCreateRequest request)
        {
            var promotionEntity = _promotionRepository.GetItem(request.PromotionId);
            string err = Verification(promotionEntity);
            if (!string.IsNullOrEmpty(err))
                return err;
            if (promotionEntity.IsLimitPerUser.HasValue &&
                promotionEntity.IsLimitPerUser.Value)
            {
                var existCoupon = _couponRepo.Get(c => c.FromPromotion == request.PromotionId && c.User_Id == request.AuthUser.Id && c.Status != (int)DataStatus.Deleted).FirstOrDefault();
                if (existCoupon != null)
                    return "该优惠活动每人限领一次，您已经领过了。";
            }
            return null;
        }
        public PromotionEntity GetFristNormalForProductId(int productId)
        {
            var es = this._pprRepository.GetList4Product(new List<int>(1) {productId});

            if (es == null || es.Count == 0)
            {
                return null;
            }

            var entities = _promotionRepository.GetList(es.Select(v => v.ProId ?? 0).Distinct().ToList(),
                                                        DataStatus.Normal, PromotionFilterMode.InProgress);

            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            foreach (var entity in entities)
            {
                var t = Verification(entity);
                if (String.IsNullOrEmpty(t))
                {
                    return entity;
                }
            }

            return null;
        }
    }
}
