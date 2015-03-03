using com.intime.fashion.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Request.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.Group;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Repository.Impl;
using Yintai.Hangzhou.Service.Contract.Apis;
using com.intime.fashion.service;
using Yintai.Hangzhou.Service.Manager;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class StorePromotionCouponController:RestfulController
    {
        private ICardRepository _cardRepo;
        private IGroupCardService _groupData;
        private IStoreCouponsRepository _storecouponRepo;
        private IStorePromotionRepository _storeproRepo;
        private IPointRepository _pointRepo;
        private IStorePromotionScopeRepository _storeproscopeRepo;
        private ICouponLogRepository _couponlogRepo;
        public StorePromotionCouponController(ICardRepository cardRepo,
            IGroupCardService groupData,
            IStoreCouponsRepository storecouponRepo,
            IStorePromotionRepository storeproRepo,
            IPointRepository pointRepo,
            IStorePromotionScopeRepository storeproscopeRepo,
            ICouponLogRepository couponlogRepo)
        {
            _cardRepo = cardRepo;
            _groupData = groupData;
            _storecouponRepo = storecouponRepo;
            _storeproRepo = storeproRepo;
            _pointRepo = pointRepo;
            _storeproscopeRepo = storeproscopeRepo;
            _couponlogRepo = couponlogRepo;
        }
        [HttpPost]
        [RestfulAuthorize]
        public ActionResult Exchange(ExchangeStoreCouponRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
           
            var storepromotion = _storeproRepo.Get(sp => sp.Id == request.StorePromotionId && sp.ActiveStartDate <= DateTime.Now && sp.ActiveEndDate >= DateTime.Now).FirstOrDefault();
            if (storepromotion == null)
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "促销活动无效!" }

                };
            if (storepromotion.MinPoints > request.Points)
            {
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "兑换积点需大于最小积点限制!" }

                }; 
            }
            if (storepromotion.MinPoints.HasValue && storepromotion.UnitPerPoints.HasValue && storepromotion.UnitPerPoints.Value > 0 &&
                (request.Points % storepromotion.UnitPerPoints) != 0)
            {
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = string.Format("积点需为{0}的整数倍!",storepromotion.UnitPerPoints) }

                };  
            }
            var storescope = _storeproscopeRepo.Get(s => s.StorePromotionId == request.StorePromotionId && s.StoreId == request.StoreId && s.Status != (int)DataStatus.Deleted).FirstOrDefault();
            if (storescope == null)
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "门店无效!" }

                };
            var cardInfo = _cardRepo.Get(c => c.User_Id == authUser.Id && c.Status != (int)DataStatus.Deleted).FirstOrDefault();
            if (cardInfo == null)
                return new RestfulResult { 
                    Data =   new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "没有绑定卡！" }
                };
            var blackcard = _storeproRepo.Context.Set<CardBlackEntity>().Where(c => c.CardNo == cardInfo.CardNo && c.Status != (int)DataStatus.Deleted).FirstOrDefault();
            if (blackcard != null)
            {
                Logger.Info(string.Format("black card blocked:{0}",cardInfo.CardNo));
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "网络错误，请到实体店进行兑换!" }

                };
            }
            var pointResult =_groupData.GetPoint(new GroupCardPointRequest() { CardNo = cardInfo.CardNo });
            if (pointResult == null)
                return new RestfulResult
                {
                    Data = new ExecuteResult {  StatusCode = StatusCode.InternalServerError,Message="会员卡信息错误!"}

                };
            if (pointResult.Point < request.Points)
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "积点不足!" }

                };
            StoreCouponEntity newCoupon = null;
            using (var ts = new TransactionScope())
            { 
                // step1: create coupon code
               newCoupon= _storecouponRepo.Insert(new StoreCouponEntity()
                {
                    Amount = StorePromotionRule.AmountFromPoints(request.StorePromotionId, request.Points),
                    CreateDate = DateTime.Now,
                    CreateUser = authUser.Id,
                    Points = request.Points,
                    Status = (int)CouponStatus.Normal,
                    StorePromotionId = request.StorePromotionId,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authUser.Id,
                    UserId = authUser.Id,
                    ValidStartDate = storepromotion.CouponStartDate,
                    ValidEndDate = storepromotion.CouponEndDate,
                    VipCard = cardInfo.CardNo,
                    StoreId = request.StoreId,
                    Code = StorePromotionRule.CreateCode(request.StorePromotionId)
                });
                // step2: deduce points
               var exchangeResult = _groupData.Exchange(new GroupExchangeRequest(){
                     CardNo = cardInfo.CardNo,
                     IdentityNo = request.IdentityNo
                });
                
               string groupErr;
               bool isGroupExchangeSuccess = GroupServiceHelper.SendHttpMessage(ConfigManager.GroupHttpUrlExchange,
                    ConfigManager.GroupHttpPublicKey,
                    ConfigManager.GroupHttpPrivateKey,
                    new { 
                        cardno = cardInfo.CardNo,
                        amount = request.Points,
                        identityno = request.IdentityNo.Trim(),
                        storeno = ConfigManager.AppStoreNoInGroup
                     },
                    out groupErr);
                
                // step3: commit
                 if (exchangeResult.Success)
                    ts.Complete();
                else
                {
                    Logger.Info(groupErr);
                     return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError,
                        Message = string.IsNullOrEmpty(groupErr)?"积点扣减异常!":groupErr }

                };
                }
               
            }
            return new RestfulResult { Data = new ExecuteResult<ExchangeStoreCouponResponse>(
                                new ExchangeStoreCouponResponse().FromEntity<ExchangeStoreCouponResponse>(newCoupon,
                                            s=>{
                                                s.StoreName = storescope.StoreName;
                                                s.Exclude = storepromotion.Notice;

                                            })) };
        }
        [RestfulAuthorize]
        public ActionResult List(StoreCouponListRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            if (request == null)
                return new RestfulResult { Data = new ExecuteResult<StoreCouponListRequest>(null) };
            var linq = _storecouponRepo.Get(c => c.UserId == authUser.Id && c.Status != (int)CouponStatus.Deleted);
            if (request.Type.HasValue)
            {
                switch (request.Type.Value)
                {
                    case CouponRequestType.Used:
                        linq = linq.Where(c => c.Status == (int)CouponStatus.Used);
                        break;
                    case CouponRequestType.Expired:
                        linq = linq.Where(c => c.Status != (int)CouponStatus.Used && c.ValidEndDate.HasValue && c.ValidEndDate.Value < DateTime.Now);
                        break;
                    case CouponRequestType.UnUsed:
                        linq = linq.Where(c => c.Status != (int)CouponStatus.Used && c.ValidEndDate.HasValue && c.ValidEndDate.Value >= DateTime.Now);
                        break;
                }
            }
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(c => c.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var linq2 = linq.Join(_storeproRepo.GetAll(), o => o.StorePromotionId, i => i.Id, (o, i) => new { SC = o, SP = i });
                          
            var responseData = from l in linq2.ToList()
                               select new StoreCouponDetailResponse().FromEntity<StoreCouponDetailResponse>(l.SC,
                                            c =>
                                            {
                                                c.Promotion = new StorePromotionDetailResponse().FromEntity<StorePromotionDetailResponse>(l.SP);
                                              
                                            });
            var response = new PagerInfoResponse<StoreCouponDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = responseData.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<StoreCouponDetailResponse>>(response) };
        }
        [RestfulAuthorize]
        public ActionResult Detail(StoreCouponDetailRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
            if (request == null)
                return new RestfulResult { Data = new ExecuteResult<StoreCouponListRequest>(null) };
            var linq = _storecouponRepo.Get(c => c.Id == request.StoreCouponId && c.UserId== authUser.Id && c.Status != (int)CouponStatus.Deleted);
            var linq2 = linq.Join(_storeproRepo.GetAll(), o => o.StorePromotionId, i => i.Id, (o, i) => new { SC = o, SP = i })
                            .Join(_storeproscopeRepo.Get(s => s.Status != (int)DataStatus.Deleted), o => new { o.SC.StorePromotionId, o.SC.StoreId }, i => new { i.StorePromotionId, i.StoreId }, (o, i) => new { SC=o.SC,SP=o.SP,SS=i}); ;
                           
            var responseData = from l in linq2.ToList()
                               select new StoreCouponDetailResponse().FromEntity<StoreCouponDetailResponse>(l.SC,
                                            c =>
                                            {
                                                c.Promotion = new StorePromotionDetailResponse().FromEntity<StorePromotionDetailResponse>(l.SP, sp => {
                                                    sp.InScopeNotice_S = new[] { new StorePromotionScopeDetailResponse(){
                                                         Excludes = l.SS.Excludes,
                                                         StoreName = l.SS.StoreName
                                                    }};
                                                });
                                                c.StoreName = l.SS.StoreName;
                                                c.Exclude = l.SS.Excludes;

                                            });
            return new RestfulResult { Data = new ExecuteResult<StoreCouponDetailResponse>(responseData.FirstOrDefault()) };
        }

        [RestfulAuthorize]
        public ActionResult Void(StoreCouponDetailRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;
            var coupon = _storecouponRepo.Get(sp => sp.Id == request.StoreCouponId && sp.UserId.Value == authUser.Id && sp.Status!=(int)CouponStatus.Used && sp.ValidEndDate >= DateTime.Now).FirstOrDefault();
            if (coupon == null)
                return new RestfulResult
                {
                    Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "代金券已失效或使用!" }

                };
           
            using (var ts = new TransactionScope())
            {
                // step1: void coupon
                coupon.Status = (int)CouponStatus.Deleted;
                _storecouponRepo.Update(coupon);
                //step2: rebate points of app

               var newPoint = _pointRepo.Insert(new PointHistoryEntity()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUser = authUser.Id,
                    Description = "取消代金券退回积点",
                    Name = string.Format("取消代金券,返回{0}",ConfigManager.Point2GroupRatio * coupon.Points.Value),
                    PointSourceId = request.StoreCouponId,
                    PointSourceType = (int)PointSourceType.Group,
                    Status = (int)DataStatus.Normal,
                    UpdatedDate = DateTime.Now,
                    UpdatedUser = authUser.Id,
                    User_Id = authUser.Id,
                    Type = (int)PointType.VoidCoupon,
                    Amount = ConfigManager.Point2GroupRatio * coupon.Points.Value

                });

                //step3: insert coupon log
               _couponlogRepo.Insert(new CouponLogEntity()
               {
                   ActionType = (int)CouponActionType.Void,
                   Code = coupon.Code,
                   CreateDate = DateTime.Now,
                   CreateUser = authUser.Id,
                   Type = (int)CouponType.StorePromotion
               });
               
                 // step4: void action should call aws service directly to check the real time coupon status
               string message = string.Empty;
                bool isVoidSuccess = HttpClientUtil.SendHttpMessage(ConfigManager.AwsHttpUrlVoidCoupon,
                        new {
                          code = coupon.Code  
                        },
                        ConfigManager.AwsHttpPublicKey,
                        ConfigManager.AwsHttpPrivateKey,
                        r => message = r.message,
                        null);

                // step3: commit
               if (newPoint != null && isVoidSuccess)
               {
                   ts.Complete();
               }
               else
               {
                   return new RestfulResult
                   {
                       Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "取消代金券异常!" }

                   };
               }
            }
            return new RestfulResult
            {
                Data = new ExecuteResult { StatusCode = StatusCode.Success, Message = "取消代金券成功，积点将于1个工作日后返回会员卡账户!" }

            };
        }
    }
}

 