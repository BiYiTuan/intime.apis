using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Yintai.Architecture.Common.Caching;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Contract.Coupon;
using Yintai.Hangzhou.Contract.DTO.Request.Coupon;
using Yintai.Hangzhou.Contract.DTO.Request.Promotion;
using Yintai.Hangzhou.Contract.DTO.Response.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response.Product;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.Promotion;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class PromotionDataService : BaseService, IPromotionDataService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IShareService _shareService;
        private readonly IFavoriteService _favoriteService;
        private readonly ICouponDataService _couponDataService;
        private readonly IResourceService _resourceService;
        private readonly IPromotionBrandRelationRepository _promotionBrandRelationRepository;
        private readonly ICouponService _couponService;
        private readonly IPromotionService _promotionService;

        public PromotionDataService(IPromotionService promotionService, ICouponService couponService, IPromotionBrandRelationRepository promotionBrandRelationRepository, IPromotionRepository promotionRepository, IFavoriteService favoriteService, IShareService shareService, ICouponDataService couponDataService, IResourceService resourceService)
        {
            _promotionRepository = promotionRepository;
            _favoriteService = favoriteService;
            _shareService = shareService;
            _couponDataService = couponDataService;
            _resourceService = resourceService;
            _promotionBrandRelationRepository = promotionBrandRelationRepository;
            _couponService = couponService;
            _promotionService = promotionService;
        }

        private PromotionInfoResponse IsR(PromotionInfoResponse response, UserModel currentAuthUser, int entityId)
        {
            if (response == null || currentAuthUser == null)
            {
                return response;
            }

            //是否收藏
            var favoriteEntity = _favoriteService.Get(currentAuthUser.Id, entityId, SourceType.Promotion);
            if (favoriteEntity != null)
            {
                response.CurrentUserIsFavorited = true;
            }
            //是否获取过优惠码
            var list = _couponService.Get(currentAuthUser.Id, entityId, SourceType.Promotion);
            if (list != null && list.Count > 0)
            {
                response.CurrentUserIsReceived = true;
            }

            return response;
        }

        #region Implementation of IPromotionService

        public ExecuteResult<PromotionCollectionResponse> GetPromotionForBanner(GetPromotionBannerListRequest request)
        {
            var page = new PagerRequest(request.Page, request.Pagesize, 40);

            int totalCount;
            var entities = _promotionRepository.Get(page, out totalCount, request.SortOrder, null, PromotionFilterMode.NotTheEnd,
                          DataStatus.Normal, true);

            var response = new PromotionCollectionResponse(page, totalCount)
            {
                Promotions = MappingManager.PromotionResponseMapping(entities, request.CoordinateInfo, true)
            };


            var result = new ExecuteResult<PromotionCollectionResponse> { Data = response };

            return result;
        }

        private PromotionCollectionResponse GetList(PagerRequest pagerRequest, Timestamp timestamp, PromotionSortOrder sortOrder, CoordinateInfo coordinateInfo)
        {

            int totalCount;
            var entitys = Get(pagerRequest, timestamp, sortOrder, coordinateInfo, out totalCount);

            var response = new PromotionCollectionResponse(pagerRequest, totalCount)
            {
                Promotions = MappingManager.PromotionResponseMapping(entitys, coordinateInfo)
            };


            return response;
        }

        private List<PromotionEntity> Get(PagerRequest pageRequest, Timestamp timestamp, PromotionSortOrder sortOrder, CoordinateInfo coordinateInfo, out int totalCount)
        {
            List<PromotionEntity> entitys;

            switch (sortOrder)
            {
                case PromotionSortOrder.Near:
                    //先找 店铺地理位置，找到并且有促销的店铺
                    //根据店铺筛出商品
                    entitys = _promotionRepository.GetPagedList(pageRequest.PageIndex, pageRequest.PageSize, out totalCount,
                                                           (int)sortOrder, coordinateInfo.Longitude, coordinateInfo.Latitude, timestamp);
                    break;
                case PromotionSortOrder.New:
                    /*查询逻辑
                     * 1.今天开始的活动
                     * 2.以前开始，今天还自进行的活动
                     * 3.即将开始的活动，时间升序 24小时内的
                     * 
                     * logic 例 size40 
                     */

                    //1
                    entitys = _promotionRepository.GetPagedList(pageRequest, out totalCount, PromotionSortOrder.New, new DateTimeRangeInfo
                    {
                        StartDateTime = DateTime.Now,
                        EndDateTime = DateTime.Now

                    }, coordinateInfo, timestamp, null, PromotionFilterMode.New);

                    var t = pageRequest.PageIndex * pageRequest.PageSize;

                    var e2Size = 0;
                    var e2Index = 1;
                    List<PromotionEntity> e2;
                    var e2Count = 0;
                    var c = t - totalCount;
                    int? skipCount = null;
                    if (c <= 0)
                    {
                        e2Index = 1;
                        e2Size = 0;
                    }
                    else if (c > 0 && c < pageRequest.PageSize)
                    {
                        //1
                        e2Index = 1;
                        e2Size = c;
                    }
                    else
                    {
                        e2Index = (int)Math.Ceiling(c / (double)pageRequest.PageSize);
                        e2Size = pageRequest.PageSize;

                        if (e2Index > 1)
                        {
                            skipCount = c - (e2Index - 1) * e2Size + (pageRequest.PageSize * (e2Index - 2));
                        }
                    }

                    var p2 = new PagerRequest(e2Index, e2Size);

                    e2 = _promotionRepository.GetPagedList(p2, out e2Count, PromotionSortOrder.New, new DateTimeRangeInfo
                    {
                        StartDateTime = DateTime.Now,
                        EndDateTime = DateTime.Now
                    }, coordinateInfo, timestamp, null, PromotionFilterMode.BeginStart, skipCount);

                    if (e2.Count != 0 && e2Size != 0)
                    {
                        entitys.AddRange(e2);
                    }

                    //总记录数
                    totalCount = totalCount + e2Count;
                    //entitys = _promotionRepository.GetPagedList(pageRequest.PageIndex, pageRequest.PageSize,
                    //                    out totalCount, (int)request.SortOrder, timestamp);
                    break;
                default:
                    entitys = _promotionRepository.GetPagedList(pageRequest.PageIndex, pageRequest.PageSize,
                                                             out totalCount, (int)sortOrder, timestamp);
                    break;
            }

            return entitys;
        }

        /// <summary>
        /// 注意获取最新接口的调用方式
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<PromotionCollectionResponse> GetPromotionList(GetPromotionListRequest request)
        {
            var pageRequest = new PagerRequest(request.Page, request.Pagesize);
            var timestamp = new Timestamp { TsType = TimestampType.Old, Ts = DateTime.Parse(request.RefreshTs) };

            var response = GetList(pageRequest, timestamp, request.SortOrder, request.CoordinateInfo);

            var result = new ExecuteResult<PromotionCollectionResponse> { Data = response };

            return result;
        }

        /// <summary>
        /// 刷新接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<PromotionCollectionResponse> GetPromotionListForRefresh(GetPromotionListForRefresh request)
        {
            var timestamp = new Timestamp { TsType = TimestampType.New, Ts = DateTime.Parse(request.RefreshTs) };

            var response = GetList(request.PagerRequest, timestamp, request.SortOrder, request.CoordinateInfo);

            var result = new ExecuteResult<PromotionCollectionResponse> { Data = response };

            return result;
        }

        /// <summary>
        /// 获取促销详情信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<PromotionInfoResponse> GetPromotionInfo(GetPromotionInfoRequest request)
        {

            var entity = _promotionRepository.GetItem(request.Promotionid);
            var response = MappingManager.PromotionResponseMapping(entity, request.CoordinateInfo);



            if (request.CurrentAuthUser != null && response != null)
            {
                //是否收藏
                response = IsR(response, request.CurrentAuthUser, response.Id);
            }

            var result = new ExecuteResult<PromotionInfoResponse>(response);

            return result;
        }

        /// <summary>
        /// 创建分享
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<PromotionInfoResponse> CreateShare(PromotionShareCreateRequest request)
        {
            var promotionEntity = _promotionRepository.GetItem(request.Promotionid);

            if (promotionEntity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "活动不存在" };
            }

            using (var ts = new TransactionScope())
            {
                _shareService.Create(new ShareHistoryEntity
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUser = request.AuthUid,
                        Description = request.Description,
                        Name = request.Name,
                        ShareTo = request.OutSiteType,
                        SourceId = request.Promotionid,
                        SourceType = (int)SourceType.Promotion,
                        Stauts = 1,
                        User_Id = request.AuthUid,
                        UpdatedDate = DateTime.Now,
                        UpdatedUser = request.AuthUid
                    });


                //TODO
                promotionEntity = _promotionRepository.SetCount(PromotionCountType.ShareCount, promotionEntity.Id, 1);

                ts.Complete();
            }
            var re = MappingManager.PromotionResponseMapping(promotionEntity, request.CoordinateInfo);

            re = IsR(re, request.AuthUser, request.AuthUser.Id);

            return new ExecuteResult<PromotionInfoResponse> { Data = re };
        }

        /// <summary>
        /// 创建收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<PromotionInfoResponse> CreateFavor(PromotionFavorCreateRequest request)
        {
            var promotionEntity = _promotionRepository.GetItem(request.Promotionid);
            if (promotionEntity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "活动不存在" };
            }
            //检查是否收藏过
            var favorEntity = _favoriteService.Get(request.AuthUid, request.Promotionid, SourceType.Promotion);
            if (favorEntity != null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您已经添加过收藏了" };
            }

            _favoriteService.Create(new FavoriteEntity
                {
                    CreatedDate = DateTime.Now,
                    CreatedUser = request.AuthUid,
                    Description = String.Empty,
                    FavoriteSourceId = promotionEntity.Id,
                    FavoriteSourceType = (int)SourceType.Promotion,
                    Status = 1,
                    User_Id = request.AuthUid,
                    Store_Id = promotionEntity.Store_Id
                });

            //TODO
            promotionEntity = _promotionRepository.SetCount(PromotionCountType.FavoriteCount, promotionEntity.Id, 1);

            var re = MappingManager.PromotionResponseMapping(promotionEntity, request.CoordinateInfo);
            re = IsR(re, request.AuthUser, request.AuthUser.Id);

            return new ExecuteResult<PromotionInfoResponse> { Data = re };
        }

        public ExecuteResult<PromotionInfoResponse> DestroyFavor(PromotionFavorDestroyRequest request)
        {
            var promotionEntity = _promotionRepository.GetItem(request.Promotionid);
            if (promotionEntity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "活动不存在" };
            }
            //检查是否收藏过
            var favorEntity = _favoriteService.Get(request.AuthUid, request.Promotionid, SourceType.Promotion);
            if (favorEntity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "没有找到收藏" };
            }

            _favoriteService.Del(favorEntity);

            //TODO
            promotionEntity = _promotionRepository.SetCount(PromotionCountType.FavoriteCount, promotionEntity.Id, -1);

            var re = MappingManager.PromotionResponseMapping(promotionEntity, request.CoordinateInfo);
            re = IsR(re, request.AuthUser, request.AuthUser.Id);

            return new ExecuteResult<PromotionInfoResponse> { Data = re };
        }

        /// <summary>
        /// 创建优惠卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<PromotionInfoResponse> CreateCoupon(PromotionCouponCreateRequest request)
        {
            
            var str = _promotionService.Verification(request);
            if (!String.IsNullOrEmpty(str))
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = str };
            }

            ExecuteResult<CouponCodeResponse> coupon = null;
            using (var ts = new TransactionScope())
            {
                coupon = _couponDataService.CreateCoupon(new CouponCouponRequest
                    {
                        AuthUid = request.AuthUid,
                        PromotionId = request.PromotionId,
                        ProductId = 0,
                        SourceType = (int)SourceType.Promotion,
                        Token = request.Token,
                        AuthUser = request.AuthUser,
                        Method = request.Method,
                        Client_Version = request.Client_Version
                    });

                if (!coupon.IsSuccess)
                {
                    return new ExecuteResult<PromotionInfoResponse>(null)
                        {
                            Message = coupon.Message,
                            StatusCode = coupon.StatusCode
                        };
                }

                var promotionEntity = _promotionRepository.GetItem(request.PromotionId);
                promotionEntity = _promotionRepository.SetCount(PromotionCountType.InvolvedCount, promotionEntity.Id, 1);

            
                var re = MappingManager.PromotionResponseMapping(promotionEntity);
                re.CouponCodeResponse = coupon.Data;

                re = IsR(re, request.AuthUser, request.AuthUser.Id);
                ts.Complete();
                return new ExecuteResult<PromotionInfoResponse> { Data = re };
            }
        }

        /// <summary>
        /// 创建活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<PromotionInfoResponse> CreatePromotion(CreatePromotionRequest request)
        {
            var promotionSourceType = request.AuthUser.UserLevel == (int)UserLevel.Daren ? RecommendSourceType.Daren : RecommendSourceType.StoreManager;
            //如果是达人，需要上传storeId,如果是店长，那么取店长所属的store
            // promotionSourceType == RecommendSourceType.Daren ? request.StoreId : request.AuthUser.Store_Id;
            var storeId = request.StoreId < 1 ? request.AuthUser.Store_Id : request.StoreId;
            int pid = 0;
            using (var ts = new TransactionScope())
            {
                var promotionEntity = _promotionRepository.Insert(new PromotionEntity
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUser = request.AuthUid,
                        Description = request.Description,
                        EndDate = request.EndDate,
                        FavoriteCount = 0,
                        InvolvedCount = 0,
                        LikeCount = 0,
                        Name = request.Name,
                        RecommendSourceId = request.RecommendUser == null ? request.AuthUid : request.RecommendUser.Value,
                        RecommendSourceType = (int)promotionSourceType,
                        ShareCount = 0,
                        StartDate = request.StartDate,
                        Status = 1,
                        Store_Id = storeId ?? 0,
                        UpdatedDate = DateTime.Now,
                        UpdatedUser = request.AuthUid,
                        RecommendUser = request.RecommendUser == null ? request.AuthUid : request.RecommendUser.Value,
                        Tag_Id = request.TagId ?? 0,
                        IsTop = false
                    });
                pid = promotionEntity.Id;
                //处理 图片
                //处理文件上传
                if (request.Files != null && request.Files.Count > 0)
                {
                    _resourceService.Save(request.Files, request.AuthUid, 0, promotionEntity.Id, SourceType.Promotion);
                }

                //处理品牌关系
                if (request.Brands.Length > 0)
                {
                    var b = request.Brands.Distinct().Where(v => v > 0).ToList();
                    var list = new List<PromotionBrandRelationEntity>(b.Count);
                    foreach (var item in b)
                    {
                        list.Add(new PromotionBrandRelationEntity
                            {
                                Brand_Id = item,
                                CreatedDate = DateTime.Now,
                                Promotion_Id = promotionEntity.Id
                            });
                    }

                    _promotionBrandRelationRepository.BatchInsert(list);
                }
                ts.Complete();

            }

            return GetPromotionInfo(new GetPromotionInfoRequest
            {
                Promotionid = pid,
                CurrentAuthUser = request.AuthUser
            });
        }

        public ExecuteResult<PromotionInfoResponse> UpdatePromotion(UpdatePromotionRequest request)
        {
            if (request == null || request.PromotionId == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = _promotionRepository.GetItem(request.PromotionId ?? 0);

            if (entity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到指定promotion" };
            }
            var promotionSourceType = request.AuthUser.UserLevel == (int)UserLevel.Daren ? RecommendSourceType.Daren : RecommendSourceType.StoreManager;
            // promotionSourceType == RecommendSourceType.Daren ? request.StoreId : request.AuthUser.Store_Id;
            var storeId = request.StoreId < 1 ? request.AuthUser.Store_Id : request.StoreId;

            var source = MappingManager.PromotionEntityMapping(request);
            source.CreatedDate = entity.CreatedDate;
            source.CreatedUser = entity.CreatedUser;
            source.Status = entity.Status;
            source.FavoriteCount = entity.FavoriteCount;
            source.InvolvedCount = entity.InvolvedCount;
            source.LikeCount = entity.LikeCount;
            source.ShareCount = entity.ShareCount;
            source.Store_Id = storeId ?? 0;
            source.RecommendSourceType = (int)promotionSourceType;

            MappingManager.PromotionEntityMapping(source, entity);

            _promotionRepository.Update(entity);
            //品牌关系
            if (request.Brands == null || request.Brands.Length == 0)
            {
                _promotionBrandRelationRepository.Del(entity.Id);
            }
            else
            {
                _promotionBrandRelationRepository.Del(entity.Id);
                var b = request.BrandIds.Distinct().Where(v => v > 0).ToList();
                var list = new List<PromotionBrandRelationEntity>(b.Count);
                foreach (var item in b)
                {
                    list.Add(new PromotionBrandRelationEntity
                    {
                        Brand_Id = item,
                        CreatedDate = DateTime.Now,
                        Promotion_Id = entity.Id
                    });
                }

                _promotionBrandRelationRepository.BatchInsert(list);
            }

            return new ExecuteResult<PromotionInfoResponse>(MappingManager.PromotionResponseMapping(entity));
        }

        public ExecuteResult<PromotionInfoResponse> DestroyPromotion(DestroyPromotionRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = _promotionRepository.GetItem(request.PromotionId);

            if (entity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到指定promotion" };
            }

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedUser = request.AuthUid;
            entity.Status = (int)DataStatus.Deleted;

            _promotionRepository.Delete(entity);

            return new ExecuteResult<PromotionInfoResponse>(MappingManager.PromotionResponseMapping(entity));
        }

        public ExecuteResult<PromotionInfoResponse> CreateResourcePromotion(CreateResourcePromotionRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = _promotionRepository.GetItem(request.PromotionId);

            if (entity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到指定promotion" };
            }

            //Add
            if (request.Files != null && request.Files.Count > 0)
            {
                _resourceService.Save(request.Files, request.AuthUid, request.DefaultNum, entity.Id, SourceType.Promotion);
            }

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedUser = request.AuthUid;
            _promotionRepository.Update(entity);

            return new ExecuteResult<PromotionInfoResponse>(MappingManager.PromotionResponseMapping(entity));
        }

        public ExecuteResult<PromotionInfoResponse> DestroyResourcePromotion(DestroyResourcePromotionRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = _promotionRepository.GetItem(request.PromotionId);

            if (entity == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到指定promotion" };
            }

            var resources = _resourceService.Get(entity.Id, SourceType.Promotion);

            if (resources == null || resources.Count == 0)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到promotion的资源" };
            }

            var resouce = resources.SingleOrDefault(v => v.Id == request.ResourceId);
            if (resouce == null)
            {
                return new ExecuteResult<PromotionInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到promotion的指定资源" };
            }

            _resourceService.Del(resouce.Id);

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedUser = request.AuthUid;

            _promotionRepository.Update(entity);

            return new ExecuteResult<PromotionInfoResponse>(MappingManager.PromotionResponseMapping(entity));
        }

        #endregion
    }
}