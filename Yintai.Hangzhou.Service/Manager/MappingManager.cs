using com.intime.fashion.common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Yintai.Architecture.Common.Helper;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Architecture.Framework.Utility;
using Yintai.Hangzhou.Contract.DTO.Request.Brand;
using Yintai.Hangzhou.Contract.DTO.Request.Product;
using Yintai.Hangzhou.Contract.DTO.Request.Promotion;
using Yintai.Hangzhou.Contract.DTO.Request.Store;
using Yintai.Hangzhou.Contract.DTO.Request.Tag;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Brand;
using Yintai.Hangzhou.Contract.DTO.Response.Card;
using Yintai.Hangzhou.Contract.DTO.Response.Comment;
using Yintai.Hangzhou.Contract.DTO.Response.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Device;
using Yintai.Hangzhou.Contract.DTO.Response.Favorite;
using Yintai.Hangzhou.Contract.DTO.Response.Like;
using Yintai.Hangzhou.Contract.DTO.Response.Point;
using Yintai.Hangzhou.Contract.DTO.Response.Product;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.DTO.Response.SpecialTopic;
using Yintai.Hangzhou.Contract.DTO.Response.Store;
using Yintai.Hangzhou.Contract.DTO.Response.Tag;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.Filters;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Service.Manager
{
    public abstract class BaseMappingManager
    {
        #region fields

        protected IResourceRepository _resourceRepository;
        protected IPromotionProductRelationRepository _pprRepository;
        protected ISpecialTopicProductRelationRepository _stprRepository;
        protected IStoreRepository _storeRepository;
        protected ITagRepository _tagRepository;
        protected ISpecialTopicRepository _specialTopicRepository;
        protected IPromotionRepository _promotionRepository;
        protected IBrandRepository _brandRepository;
        protected ICustomerRepository _customerRepository;
        protected IProductRepository _productRepository;

        private static readonly DateTime Min = new DateTime(1900, 1, 1);
        private static readonly DateTime Max = new DateTime(2079, 1, 1);

        #endregion

        #region .ctor

        protected BaseMappingManager()
        {
            _resourceRepository = ServiceLocator.Current.Resolve<IResourceRepository>();
            _pprRepository = ServiceLocator.Current.Resolve<IPromotionProductRelationRepository>();
            _stprRepository = ServiceLocator.Current.Resolve<ISpecialTopicProductRelationRepository>();
            _storeRepository = ServiceLocator.Current.Resolve<IStoreRepository>();
            _tagRepository = ServiceLocator.Current.Resolve<ITagRepository>();
            _specialTopicRepository = ServiceLocator.Current.Resolve<ISpecialTopicRepository>();
            _promotionRepository = ServiceLocator.Current.Resolve<IPromotionRepository>();
            _brandRepository = ServiceLocator.Current.Resolve<IBrandRepository>();
            _customerRepository = ServiceLocator.Current.Resolve<ICustomerRepository>();
            _productRepository = ServiceLocator.Current.Resolve<IProductRepository>();
        }

        #endregion

        #region methods

        /// <summary>
        ///  datetime 1753-01-01到9999-12-31 00:00:00 到 23:59:59.997 3.33毫秒
        ///smalldatetime 1900-01-01 到 2079-06-06 00:00:00 到 23:59:59 分钟
        ///date 0001-01-01 到 9999-12-31 天
        ///time 00:00:00.0000000 到 23:59:59.9999999 100 纳秒
        ///datetime2 0001-01-01 到 9999-12-31 00:00:00 到 23:59:59.9999999 100 纳秒
        ///datetimeoffset 0001-01-01 到 9999-12-31 00:00:00 到 23:59:59.9999999 -14:00 到 +14:00 100 纳秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        protected static DateTime EntityDateTime(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return DateTime.Now;
            }

            if (dateTime < Min)
            {
                return Min;
            }

            if (dateTime > Max)
            {
                return Max;
            }

            return dateTime.Value;
        }

        protected static string CheckString(string t)
        {
            return String.IsNullOrWhiteSpace(t) ? String.Empty : t;
        }

        /// <summary>
        /// M
        /// </summary>
        /// <param name="coordinateInfo1"></param>
        /// <param name="coordinateInfo2"></param>
        /// <returns></returns>
        protected static decimal Distance(CoordinateInfo coordinateInfo1, CoordinateInfo coordinateInfo2)
        {
            var d = CoordinatePositioningHelper.GetDistance(coordinateInfo1,
                                                    coordinateInfo2);

            return Convert.ToDecimal(d * 1000);
        }

        protected List<ResourceEntity> GetListResourceEntities(SourceType sourceType, int sourceId)
        {
            return _resourceRepository.GetList((int)sourceType, sourceId);
        }

        protected List<ResourceEntity> GetListResourceEntities(SourceType sourceType, List<int> ids)
        {
            return _resourceRepository.GetList((int)sourceType, ids);
        }

        protected List<int> GetPromotionidsForRelation(List<int> productids)
        {
            var entities = _pprRepository.GetList4Product(productids);

            if (entities == null || entities.Count == 0)
            {
                return new List<int>(0);
            }

            return entities.Select(v => v.ProId ?? 0).Distinct().ToList();
        }

        protected List<Promotion2ProductEntity> GetPromotionForRelation(List<int> productids)
        {
            var entities = _pprRepository.GetList4Product(productids);

            if (entities == null || entities.Count == 0)
            {
                return new List<Promotion2ProductEntity>(0);
            }

            return entities;
        }

        protected List<SpecialTopicProductRelationEntity> GetTopicRelationByProduct4Entities(List<int> productids)
        {
            if (productids == null)
            {
                return new List<SpecialTopicProductRelationEntity>(0);
            }

            var entities = _stprRepository.GetListByProduct4Linq(productids);

            if (entities == null)
            {
                return new List<SpecialTopicProductRelationEntity>(0);
            }

            return entities.ToList();
        }

        #endregion
    }

    /// <summary>
    /// customer
    /// </summary>
    public class MappingManagerV2 : BaseMappingManager
    {
        #region fields
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IVUserRoleRepository _vUserRoleRepository;
        private readonly IPromotionBrandRelationRepository _promotionBrandRelationRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IPointRepository _pointRepository;

        #endregion

        #region .ctor

        public MappingManagerV2()
        {
            //注意 只能 查询不能有修改操作
            _userAccountRepository = ServiceLocator.Current.Resolve<IUserAccountRepository>();
            _vUserRoleRepository = ServiceLocator.Current.Resolve<IVUserRoleRepository>();
            _promotionBrandRelationRepository = ServiceLocator.Current.Resolve<IPromotionBrandRelationRepository>();
            _likeRepository = ServiceLocator.Current.Resolve<ILikeRepository>();
            _couponRepository = ServiceLocator.Current.Resolve<ICouponRepository>();
            _favoriteRepository = ServiceLocator.Current.Resolve<IFavoriteRepository>();
            _pointRepository = ServiceLocator.Current.Resolve<IPointRepository>();
        }

        #endregion

        #region properties

        #endregion

        #region methods

        private void Init()
        {
            //TODO:MAPPING需要处理为非静态
        }

        private string GetToken(UserModel userModel)
        {
            return SessionKeyHelper.Encrypt(userModel.Id.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region customer

    

        public UserEntity UserEntityMapping(UserEntity source, UserEntity target)
        {
            var result = Mapper.Map(source, target);

            return UserEntityCheck(result);
        }

        private static UserEntity UserEntityCheck(UserEntity source)
        {
            if (source == null)
            {
                return null;
            }

            source.Description = source.Description ?? String.Empty;
            source.EMail = source.EMail ?? String.Empty;
            source.Logo = source.Logo ?? String.Empty;
            source.Mobile = source.Mobile ?? String.Empty;
            source.Name = source.Name ?? String.Empty;
            source.Nickname = source.Nickname ?? String.Empty;
            source.Password = source.Password ?? String.Empty;

            source.CreatedDate = EntityDateTime(source.CreatedDate);
            source.UpdatedDate = EntityDateTime(source.UpdatedDate);

            return source;
        }

        public IEnumerable<ShowCustomerInfoResponse> ShowCustomerInfoResponseMapping(List<UserEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<ShowCustomerInfoResponse>(0);
            }

            var userModel = UserModelMapping(source);
            if (userModel == null)
            {
                return null;
            }

            return ShowCustomerInfoResponseMapping(userModel.ToList());
        }

        public ShowCustomerInfoResponse ShowCustomerInfoResponseMapping(UserEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var model = UserModelMapping(source);

            if (model == null)
            {
                return null;
            }

            return ShowCustomerInfoResponseMapping(model);
        }

        /// <summary>
        /// DTO  转换 重新获取COUNT 并且更新 ACCOUNT
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public CustomerInfoResponse CustomerInfoResponseMappingForReadCount(UserModel source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<UserModel, CustomerInfoResponse>(source);
            if (target == null)
            {
                return null;
            }

            //Read data
            //1.关注
            //2.粉丝
            //3.优惠券
            //4.收藏的商品
            //5.分享到外站的商品
            //6.积点

            var ilikeCount = _likeRepository.GetILikeCount(source.Id);
            var likeMeCount = _likeRepository.GetLikeMeCount(source.Id);
            var couponCount = _couponRepository.GetUserCouponCount(source.Id, CouponBusinessStatus.UnExpired);
            var favorCount = _favoriteRepository.GetUserFavorCount(source.Id, null);
            //var shareCount = 
            //目前无扣分 有扣分时候再说
            var pointSumZ = _pointRepository.GetUserPointSum(source.Id, new List<PointType>
                {
                    PointType.BeConsumption,
                    PointType.Default,
                    PointType.InviteConsumption,
                    PointType.Register,
                    PointType.Reward
                });
            var pointSumF = _pointRepository.GetUserPointSum(source.Id, new List<PointType>
                {
                    PointType.Consumption
                });

            //注意是否为负的可能
            var pointSum = pointSumZ - pointSumF;

            if (target.ILikeCount != ilikeCount)
            {
                target.ILikeCount = ilikeCount;
                //up
                _userAccountRepository.SetAmount(source.Id, AccountType.IlikeCount, ilikeCount);
            }

            if (target.LikeMeCount != likeMeCount)
            {
                target.LikeMeCount = likeMeCount;
                _userAccountRepository.SetAmount(source.Id, AccountType.LikeMeCount, likeMeCount);
            }

            if (target.CouponCount != couponCount)
            {
                target.CouponCount = couponCount;
                _userAccountRepository.SetAmount(source.Id, AccountType.Coupon, couponCount);
            }

            if (target.FavorCount != favorCount)
            {
                target.FavorCount = favorCount;
                _userAccountRepository.SetAmount(source.Id, AccountType.FavorCount, favorCount);
            }

            if (target.PointCount != pointSum)
            {
                target.PointCount = pointSum;
                _userAccountRepository.SetAmount(source.Id, AccountType.Point, pointSum);
            }

            target.Token = GetToken(source);
            target.AppId = ConfigManager.GetAppleAppId();
              
            return target;
        }

        /// <summary>
        /// DTO  转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public CustomerInfoResponse CustomerInfoResponseMapping(UserModel source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<UserModel, CustomerInfoResponse>(source);

            target.Token = GetToken(source);
            target.AppId = ConfigManager.GetAppleAppId();

            return target;
        }

        public ShowCustomerInfoResponse ShowCustomerInfoResponseMapping(UserModel source)
        {
            var target = Mapper.Map<UserModel, ShowCustomerInfoResponse>(source);

            return target;
        }

        public IEnumerable<ShowCustomerInfoResponse> ShowCustomerInfoResponseMapping(List<UserModel> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<ShowCustomerInfoResponse>(0);
            }

            var result = new List<ShowCustomerInfoResponse>(source.Count);

            foreach (var item in source)
            {
                var target = ShowCustomerInfoResponseMapping(item);

                if (target != null)
                {
                    result.Add(target);
                }
            }

            return result;
        }

        public IEnumerable<UserModel> UserModelMapping(List<UserEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<UserModel>(0);
            }

            var userIds = source.Select(v => v.Id).Distinct().ToList();
            var storeIds = source.Select(v => v.Store_Id).Where(v => v != 0).Distinct().ToList();

            var stores = StoreModelMapping(_storeRepository.GetListByIds(storeIds)).ToList();
            var accounts = UserAccountMapping(_userAccountRepository.GetListByUserIds(userIds)).ToList();
            var userRoles = _vUserRoleRepository.GetList(userIds);

            var result = new List<UserModel>(source.Count);

            foreach (var item in source)
            {
                var s = stores.FirstOrDefault(v => v.Id == item.Store_Id);
                var a = accounts.Where(v => v.User_Id == item.Id).ToList();
                var r = userRoles.Where(v => v.User_Id == item.Id).ToList();
                var target = UserModelMapping(item, s, a, UserRolesMapping(r));

                if (target != null)
                {
                    result.Add(target);
                }

            }

            return result;
        }

        private static UserModel UserModelMapping(UserEntity source, StoreModel storeModel,
                                          List<UserAccountModel> userAccountModels, List<int> userRoles)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<UserEntity, UserModel>(source);

            //这步可以判断
            target.Store = storeModel;
            //modelAccount
            target.Accounts = userAccountModels;
            //roles
            target.UserRoles = userRoles;
            //favorcount
            //ilikecount
            //likemecount

            /*
            if (!String.IsNullOrWhiteSpace(target.Logo))
            {
                if (!target.Logo.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    target.Logo = ConfigManager.GetHttpApiImagePath() + target.Logo;
                }
            }
            */
            if (target.Accounts == null || target.Accounts.Count == 0)
            {
                return target;
            }

            foreach (var item in target.Accounts)
            {
                switch (item.AType)
                {
                    case AccountType.ConsumptionCount:
                        target.ConsumptionCount = (int)item.Amount;
                        break;
                    case AccountType.Coupon:
                        target.CouponCount = (int)item.Amount;
                        break;
                    case AccountType.FavorCount:
                        target.FavorCount = (int)item.Amount;
                        break;
                    case AccountType.IlikeCount:
                        target.ILikeCount = (int)item.Amount;
                        break;
                    case AccountType.LikeMeCount:
                        target.LikeMeCount = (int)item.Amount;
                        break;
                    case AccountType.Point:
                        target.PointCount = (int)item.Amount;
                        break;
                    case AccountType.ShareCount:
                        target.ShareCount = (int)item.Amount;
                        break;
                }
            }

            return target;
        }


        public UserModel UserModelMapping(UserEntity source)
        {
            if (source == null)
            {
                return null;
            }

            //var target = Mapper.Map<UserEntity, UserModel>(source);

            //这步可以判断
            StoreModel store = null;
            if (source.Store_Id > 0)
            {
                store = StoreModelMapping(_storeRepository.GetItem(source.Store_Id));
            }
            //modelAccount
            var accounts = UserAccountMapping(_userAccountRepository.GetUserAccount(source.Id)).ToList();
            //roles
            var userRoles = UserRolesMapping(_vUserRoleRepository.GetList(source.Id));

            return UserModelMapping(source, store, accounts, userRoles);
        }

        public UserEntity UserEntityMapping(UserModel source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<UserModel, UserEntity>(source);

            return CheckUserEntity(target);
        }

        private static UserEntity CheckUserEntity(UserEntity source)
        {
            if (source == null)
            {
                return null;
            }

            source.Name = CheckString(source.Name);
            source.Description = CheckString(source.Description);
            source.EMail = CheckString(source.EMail);
            source.Logo = CheckString(source.Logo);
            source.Mobile = CheckString(source.Mobile);
            source.Nickname = CheckString(source.Nickname);
            source.Password = CheckString(source.Password);

            return source;
        }

        #endregion

        #region useraccount

        public static UserAccountModel UserAccountMapping(UserAccountEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<UserAccountEntity, UserAccountModel>(source);

            return target;
        }

        public static IEnumerable<UserAccountModel> UserAccountMapping(List<UserAccountEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<UserAccountModel>(0);
            }

            var list = new List<UserAccountModel>(source.Count);

            foreach (var item in source)
            {
                var target = UserAccountMapping(item);

                if (target == null)
                {
                    continue;
                }
                list.Add(target);
            }

            return list;
        }

        //public static CustomerAccountResponse UserAccountResponseMapping(UserAccountModel source)
        //{
        //    var target = Mapper.Map<UserAccountModel, CustomerAccountResponse>(source);

        //    return target;
        //}

        //public static IEnumerable<CustomerAccountResponse> UserAccountResponseMapping(List<UserAccountModel> source)
        //{
        //    foreach (var userAccountModel in source)
        //    {
        //        yield return UserAccountResponseMapping(userAccountModel);
        //    }
        //}

        #endregion

        #region vuserRoles

        public static List<int> UserRolesMapping(List<VUserRoleEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<int>(0);
            }

            var list = new List<int>(source.Count);
            //var dic = new Dictionary<UserRole, bool>(source.Count);
            foreach (var item in source)
            {
                var target = item.Role_Val;

                list.Add(target);
            }

            return list;
        }



        #endregion

        #region store

        public StoreEntity StoreEntityMapping(StoreEntity source, StoreEntity target)
        {
            // result.Equery(target) = true;
            var result = Mapper.Map(source, target);

            return result;
        }

        public StoreEntity StoreEntityMapping(StoreInfoRequest source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<StoreInfoRequest, StoreEntity>(source);
            target.CreatedDate = DateTime.Now;
            target.CreatedUser = source.AuthUid;
            target.UpdatedDate = DateTime.Now;
            target.UpdatedUser = source.AuthUid;
            target.Status = (int)DataStatus.Normal;

            return target;
        }

        public StoreModel StoreModelMapping(StoreEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<StoreEntity, StoreModel>(source);

            return target;
        }

        public IEnumerable<StoreModel> StoreModelMapping(List<StoreEntity> source)
        {
            foreach (var storeEntity in source)
            {
                yield return StoreModelMapping(storeEntity);
            }
        }

        public StoreEntity StoreEntityMapping(StoreModel source)
        {
            var target = Mapper.Map<StoreModel, StoreEntity>(source);

            return target;
        }

        public StoreInfoResponse StoreResponseMapping(StoreEntity source)
        {
            var target = Mapper.Map<StoreEntity, StoreInfoResponse>(source);

            return target;
        }

        public StoreInfoResponse StoreResponseMapping(StoreEntity source, CoordinateInfo coordinateInfo)
        {
            if (coordinateInfo == null)
            {
                return StoreResponseMapping(source);
            }

            var target = Mapper.Map<StoreEntity, StoreInfoResponse>(source);

            target.Distance = Distance(coordinateInfo, new CoordinateInfo((double)target.Longitude, (double)target.Latitude));

            return target;
        }

        public StoreInfoResponse StoreResponseMapping(StoreInfoResponse source, CoordinateInfo coordinateInfo)
        {
            if (coordinateInfo == null)
            {
                return source;
            }

            source.Distance = Distance(coordinateInfo, new CoordinateInfo((double)source.Longitude, (double)source.Latitude));

            return source;
        }

        public IEnumerable<StoreInfoResponse> StoreResponseMapping(List<StoreEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<StoreInfoResponse>(0);
            }

            var list = new List<StoreInfoResponse>(source.Count);

            foreach (var item in source)
            {
                var target = StoreResponseMapping(item);
                if (target == null)
                {
                    continue;
                }

                list.Add(target);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="coordinateInfo"></param>
        /// <returns></returns>
        public IEnumerable<StoreInfoResponse> StoreResponseMapping(List<StoreEntity> source, CoordinateInfo coordinateInfo)
        {
            if (coordinateInfo == null)
            {
                foreach (var storeEntity in source)
                {
                    yield return StoreResponseMapping(storeEntity);
                }
            }
            else
            {
                foreach (var storeEntity in source)
                {
                    yield return StoreResponseMapping(storeEntity, coordinateInfo);
                }
            }
        }

        #endregion

        #region resource

        public IEnumerable<ResourceInfoResponse> ResourceInfoResponsesMapping(List<ResourceEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<ResourceInfoResponse>(0);
            }

            var list = new List<ResourceInfoResponse>(source.Count);

            foreach (var item in source)
            {
                var target = ResourceInfoResponsesMapping(item);

                if (target == null)
                {
                    continue;
                }

                list.Add(target);
            }

            return list;
        }

        public ResourceInfoResponse ResourceInfoResponsesMapping(ResourceEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<ResourceEntity, ResourceInfoResponse>(source);
            //zhuyi
            if (String.IsNullOrWhiteSpace(target.Domain))
            {
                switch (target.ResourceType)
                {
                    case ResourceType.Image:
                        target.Domain = ConfigManager.GetHttpApiImagePath();
                        break;
                    case ResourceType.Sound:
                        target.Domain = ConfigManager.GetHttpApiSoundPath();
                        break;
                    case ResourceType.Video:
                        target.Domain = ConfigManager.GetHttpApivideoPath();
                        break;
                    case ResourceType.Default:
                        target.Domain = ConfigManager.GetHttpApidefPath();
                        break;
                    default:
                        break;

                }
            }

            return target;
        }

        #endregion

        #region promotion

        public PromotionEntity PromotionEntityMapping(PromotionEntity source, PromotionEntity target)
        {
            var result = Mapper.Map(source, target);

            return result;
        }

        public PromotionEntity PromotionEntityMapping(PromotionInfoRequest source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<PromotionInfoRequest, PromotionEntity>(source);
            target.CreatedDate = DateTime.Now;
            target.CreatedUser = source.AuthUid;
            target.UpdatedDate = DateTime.Now;
            target.UpdatedUser = source.AuthUid;
            target.Status = (int)DataStatus.Normal;
            target.RecommendSourceId =
                target.RecommendUser = source.RecommendUser == null ? source.AuthUid : source.RecommendUser.Value;

            return target;
        }

        public PromotionInfo PromotionInfoMapping(PromotionEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<PromotionEntity, PromotionInfo>(source);

            return target;
        }

        public PromotionInfo PromotionInfoMapping(PromotionInfoResponse source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<PromotionInfoResponse, PromotionInfo>(source);

            return target;
        }

        public PromotionInfoResponse PromotionInfoResponseMapping(PromotionEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<PromotionEntity, PromotionInfoResponse>(source);

            return target;
        }

        public PromotionInfoResponse PromotionResponseMapping(PromotionEntity source, CoordinateInfo coordinateInfo, List<int> brandIds, List<ResourceInfoResponse> resourceInfoResponses, StoreInfoResponse storeInfoResponse, ShowCustomerInfoResponse showCustomerInfoResponse)
        {
            if (source == null)
            {
                return null;
            }

            var target = PromotionInfoResponseMapping(source);

            if (showCustomerInfoResponse != null)
            {
                target.ShowCustomer = showCustomerInfoResponse;
            }

            if (storeInfoResponse != null)
            {
                target.StoreInfoResponse = storeInfoResponse;
            }

            if (resourceInfoResponses != null)
            {
                target.ResourceInfoResponses = resourceInfoResponses;
            }

            if (brandIds != null)
            {
                target.BrandIds = brandIds;
            }

            return target;
        }

        public List<PromotionInfoResponse> PromotionResponseMapping(IQueryable<PromotionEntity> source,
                                                                    CoordinateInfo coordinateInfo, bool? isBanner)
        {
            //JOIN DIANPU
            if (source == null)
            {
                return new List<PromotionInfoResponse>(0);
            }

            var linq = source;

            var storeRepository = ServiceLocator.Current.Resolve<IStoreRepository>();
            var resouceRepository = ServiceLocator.Current.Resolve<IResourceRepository>();
            var sp = linq.Join(storeRepository.Get(DataStatus.Normal), p => p.Store_Id, f => f.Id, (p, f) =>
                    new
                        {
                            P = p,
                            S = f
                        }
                );

            var sourceTypes = new List<int>(2);

            if (isBanner != null && isBanner.Value)
            {
                sourceTypes.Add((int)SourceType.BannerPromotion);
            }
            else
            {
                sourceTypes.Add((int)SourceType.Promotion);
            }

            var rsp = sp.GroupJoin(resouceRepository.Get(v => v.Status == (int)DataStatus.Normal && sourceTypes.Any(s => s == v.SourceType)),
                              p => p.P.Id, f => f.SourceId, (p, f) => new
                                  {
                                      P = p.P,
                                      S = p.S,
                                      R = f.OrderByDescending(rf=>rf.SortOrder).FirstOrDefault()
                                  });

            var target = new Dictionary<int, PromotionInfoResponse>();

            foreach (var item in rsp)
            {
                if (item == null)
                {
                    continue;
                }

                var r = ResourceInfoResponsesMapping(item.R);

                if (target.Keys.Contains(item.P.Id))
                {
                    if (r == null)
                    {
                        continue;
                    }

                    target[item.P.Id].ResourceInfoResponses.Add(r);
                }
                else
                {
                    var store = StoreResponseMapping(item.S, coordinateInfo);

                    var t = PromotionResponseMapping(item.P, coordinateInfo, null,r==null?null:new List<ResourceInfoResponse> { r }, store, null);
                    if (t.ResourceInfoResponses == null)
                    {
                        t.ResourceInfoResponses = new List<ResourceInfoResponse>();
                    }

                    target.Add(item.P.Id, t);
                }

            }

            return target.Values.ToList();
        }

        /// <summary>
        /// 需要计算 距离
        /// </summary>
        /// <param name="source"></param>
        /// <param name="coordinateInfo">坐标</param>
        /// <returns></returns>
        public PromotionInfoResponse PromotionResponseMapping(PromotionEntity source, CoordinateInfo coordinateInfo)
        {
            if (source == null)
            {
                return null;
            }

            ShowCustomerInfoResponse showCustomer = null;
            StoreInfoResponse storeInfoResponse = null;
            List<ResourceInfoResponse> resourceInfoResponses = null;
            List<int> brandIds = null;
            if (source.RecommendUser > 0)
            {
                var userEntity = _customerRepository.GetItem(source.RecommendUser);
                if (userEntity != null)
                {
                    showCustomer = ShowCustomerInfoResponseMapping(userEntity);
                }
            }

            if (source.Store_Id > 0)
            {
                var store = _storeRepository.GetItem(source.Store_Id);
                storeInfoResponse = StoreResponseMapping(store, coordinateInfo);
            }

            var resource =
                _resourceRepository.Get(
                    v =>
                    v.Status == (int)DataStatus.Normal && v.SourceId == source.Id &&
                    (int)SourceType.Promotion == v.SourceType).OrderByDescending(r=>r.SortOrder).ToList();

            resourceInfoResponses = ResourceInfoResponsesMapping(resource).ToList();

            var brandids = _promotionBrandRelationRepository.GetList(source.Id).Select(v => v.Brand_Id).ToList();

            if (brandids.Count > 0)
            {
                brandIds = brandids;
            }

            return PromotionResponseMapping(source, coordinateInfo, brandIds, resourceInfoResponses, storeInfoResponse,
                                            showCustomer);
        }

        public PromotionInfoResponse PromotionResponseMapping(PromotionEntity source)
        {
            return PromotionResponseMapping(source, null);
        }

        public List<PromotionInfoResponse> PromotionResponseMapping(List<PromotionEntity> source, CoordinateInfo coordinateInfo)
        {
            // var userIds = source.Select(v => v.RecommendUser).Where(v => v > 0).Distinct();

            // var users = _customerRepository.GetListByIds(userIds.ToList());
            //  var responseUser = ShowCustomerInfoResponseMapping(users).ToList();
            var storeids = source.Select(v => v.Store_Id).Where(v => v > 0).Distinct();
            var stores = _storeRepository.GetListByIds(storeids.ToList());
            var storeResponse = StoreResponseMapping(stores, coordinateInfo).ToList();
            var list = new List<PromotionInfoResponse>();
            var ids = source.Select(v => v.Id).ToList();

            var resource = GetListResourceEntities(SourceType.Promotion, ids);
            //  var brandR = _promotionBrandRelationRepository.GetList(ids);

            List<ResourceInfoResponse> resourceResponse = null;
            if (resource != null)
            {
                resourceResponse = ResourceInfoResponsesMapping(resource).ToList();
            }

            foreach (var entity in source)
            {
                var entity1 = entity;
                // var b = brandR.Where(v => v.Promotion_Id == entity1.Id).Select(v => v.Brand_Id).ToList();
                var r = resourceResponse == null
                            ? new List<ResourceInfoResponse>(0)
                            : resourceResponse.Where(v => v.SourceId == entity1.Id).ToList();
                var s = storeResponse.FirstOrDefault(v => v.Id == entity.Store_Id);
                // var u = responseUser.FirstOrDefault(v => v.Id == entity.RecommendUser);
                // var target = PromotionResponseMapping(entity, coordinateInfo, b, r, s, u);
                var target = PromotionResponseMapping(entity, coordinateInfo, null, r, s, null);
                if (target == null)
                {
                    continue;
                }

                list.Add(target);
            }

            return list;
        }

        public IEnumerable<PromotionInfoResponse> PromotionResponseMapping(List<PromotionEntity> source)
        {
            return PromotionResponseMapping(source, null);
        }

        /// <summary>
        /// 验证正常的活动
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private List<PromotionInfo> GetPromotionInfos4V(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<PromotionInfo>(0);
            }

            var entities = _promotionRepository.GetList(ids, DataStatus.Normal, PromotionFilterMode.InProgress);

            if (entities == null || entities.Count == 0)
            {
                return new List<PromotionInfo>(0);
            }

            var n = new List<PromotionEntity>(entities.Count);
            foreach (var entity in entities)
            {
                if (entity.StartDate > DateTime.Now)
                {
                    continue;
                }

                if (entity.PublicationLimit != null && entity.PublicationLimit > 0)
                {
                    if (entity.PublicationLimit <= entity.InvolvedCount)
                    {
                        continue;
                    }
                }

                n.Add(entity);
            }

            return PromotionInfoMapping(n);
        }

        private List<PromotionInfo> PromotionInfoMapping(ICollection<PromotionEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<PromotionInfo>(0);
            }

            var result = new List<PromotionInfo>(source.Count);

            foreach (var item in source)
            {
                var target = PromotionInfoMapping(item);

                if (target != null)
                {
                    result.Add(target);
                }

            }

            return result;
        }

        #endregion

        #region coupon

        public IEnumerable<CouponInfoResponse> CouponInfoResponseMapping(List<CouponHistoryEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<CouponInfoResponse>(0);
            }

            var promotionIds = source.Select(v => v.FromPromotion).Distinct().Where(v => v != 0);
            var productIds = source.Select(v => v.FromProduct).Distinct().Where(v => v != 0);

            var promotions = PromotionResponseMapping(_promotionRepository.GetList(promotionIds.ToList())).ToList();
            var products = ProductInfoResponseMapping(_productRepository.GetList(productIds.ToList())).ToList();

            var result = new List<CouponInfoResponse>(source.Count);

            foreach (var item in source)
            {
                var target = Mapper.Map<CouponHistoryEntity, CouponInfoResponse>(item);

                CouponInfoResponseMapping(target, products.SingleOrDefault(v => v.Id == item.FromProduct),
                                                       promotions.SingleOrDefault(v => v.Id == item.FromPromotion));

                if (target != null)
                {
                    result.Add(target);
                }
            }

            return result;
        }

        public CouponInfoResponse CouponInfoResponseMapping(CouponHistoryEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<CouponHistoryEntity, CouponInfoResponse>(source);

            if (source.FromPromotion > 0)
            {
                var promotion = PromotionResponseMapping(_promotionRepository.GetItem(source.FromPromotion));

                return CouponInfoResponseMapping(target, null, promotion);
            }

            if (source.FromProduct > 0)
            {
                var product = ProductInfoResponseMapping(_productRepository.GetItem(source.FromProduct));

                return CouponInfoResponseMapping(target, product, null);
            }

            return target;
        }

        public static CouponInfoResponse CouponInfoResponseMapping(CouponInfoResponse source, ProductInfoResponse product,
                                                          PromotionInfoResponse promotion)
        {
            if (source == null)
            {
                return null;
            }

            var target = source;

            var productname = String.Empty;
            var producttype = 0;
            var productid = 0;
            var productDescription = String.Empty;
            if (promotion != null)
            {
                productname = promotion.Name;
                producttype = (int)SourceType.Promotion;
                productid = promotion.Id;
                productDescription =
                promotion.Description;
                target.PromotionInfoResponse = promotion;
                target.Stype = SourceType.Promotion;
            }
            else
            {
                if (product != null)
                {
                    productname = product.Name;
                    producttype = (int)SourceType.Product;
                    productid = product.Id;
                    productDescription =
                    product.Description;
                    target.ProductInfoResponse = product;
                    target.Stype = SourceType.Product;
                }
            }

            target.ProductId = productid;
            target.ProductName = productname;
            target.ProductType = producttype;
            target.ProductDescription = productDescription;



            return target;
        }

        public static CouponCodeResponse CouponCodeResponseMapping(CouponHistoryEntity source, ProductInfoResponse product,
                                                                  PromotionInfoResponse promotion)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<CouponHistoryEntity, CouponCodeResponse>(source);

            var productname = String.Empty;
            var producttype = 0;
            var productid = 0;
            var productDescription = String.Empty;
            if (promotion != null)
            {
                productname = promotion.Name;
                producttype = (int)SourceType.Promotion;
                productid = promotion.Id;
                productDescription =
                promotion.Description;
            }
            else
            {
                if (product != null)
                {
                    productname = product.Name;
                    producttype = (int)SourceType.Product;
                    productid = product.Id;
                    productDescription =
                    product.Description;
                }
            }

            target.ProductId = productid;
            target.ProductName = productname;
            target.ProductType = producttype;
            target.ProductDescription = productDescription;

            return target;
        }

        public static CouponCodeResponse CouponCodeResponseMapping(CouponHistoryEntity source, ProductEntity product,
                                                                   PromotionEntity promotion)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<CouponHistoryEntity, CouponCodeResponse>(source);

            var productname = String.Empty;
            var producttype = 0;
            var productid = 0;
            var productDescription = String.Empty;
            if (promotion != null)
            {
                productname = promotion.Name;
                producttype = (int)SourceType.Promotion;
                productid = promotion.Id;
                productDescription =
                promotion.Description;
                target.Stype = SourceType.Promotion;
            }
            else
            {
                if (product != null)
                {
                    productname = product.Name;
                    producttype = (int)SourceType.Product;
                    productid = product.Id;
                    productDescription =
                    product.Description;
                    target.Stype = SourceType.Product;
                }
            }

            target.ProductId = productid;
            target.ProductName = productname;
            target.ProductType = producttype;
            target.ProductDescription = productDescription;


            return target;
        }
        /// <summary>
        /// 优惠码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public CouponCodeResponse CouponCodeResponseMapping(CouponHistoryEntity source)
        {
            if (source == null)
            {
                return null;
            }

            if (source.FromPromotion > 0)
            {
                var promotion = _promotionRepository.GetItem(source.FromPromotion);

                return CouponCodeResponseMapping(source, null, promotion);
            }

            if (source.FromProduct > 0)
            {
                var product = _productRepository.GetItem(source.FromProduct);

                return CouponCodeResponseMapping(source, product, null);
            }

            return Mapper.Map<CouponHistoryEntity, CouponCodeResponse>(source);
        }

        /// <summary>
        /// 优惠码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<CouponCodeResponse> CouponCodeResponseMapping(List<CouponHistoryEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<CouponCodeResponse>(0);
            }

            var promotionIds = source.Select(v => v.FromPromotion).Distinct().Where(v => v != 0);
            var productIds = source.Select(v => v.FromProduct).Distinct().Where(v => v != 0);

            var promotions = _promotionRepository.GetList(promotionIds.ToList());
            var products = _productRepository.GetList(productIds.ToList());

            var result = new List<CouponCodeResponse>(source.Count);

            foreach (var item in source)
            {
                var target = CouponCodeResponseMapping(item, products.SingleOrDefault(v => v.Id == item.FromProduct),
                                                       promotions.SingleOrDefault(v => v.Id == item.FromPromotion));

                if (target != null)
                {
                    result.Add(target);
                }
            }

            return result;
        }

        #endregion

        #region favorite

        public FavoriteInfoResponse FavoriteInfoResponseMapping(FavoriteEntity source, PromotionEntity promotionEntity)
        {
            var target = Mapper.Map<FavoriteEntity, FavoriteInfoResponse>(source);
            target.FavoriteSourceName = promotionEntity == null ? String.Empty : promotionEntity.Name;

            return target;
        }

        public FavoriteInfoResponse FavoriteInfoResponseMapping(FavoriteEntity source, ProductEntity entity)
        {
            var target = Mapper.Map<FavoriteEntity, FavoriteInfoResponse>(source);
            target.FavoriteSourceName = entity == null ? String.Empty : entity.Name;

            return target;
        }

        public List<FavoriteInfoResponse> FavoriteCollectionResponseMapping(IQueryable<FavoriteEntity> source,
                                                                                   CoordinateInfo coordinateInfo)
        {
            if (source == null)
            {
                return new List<FavoriteInfoResponse>(0);
            }

            var linq = source;

            var prolist = _promotionRepository.Get(new PromotionFilter
                {
                    DataStatus = DataStatus.Normal,
                });
            var prodList = _productRepository.Get(null, new ProductFilter
                {
                    DataStatus = DataStatus.Normal,
                });

            var linqPro = linq.Where(v => v.FavoriteSourceType == (int)SourceType.Promotion)
                              .Join(prolist, p => p.FavoriteSourceId,
                                    f => f.Id,
                                    (p, f) =>
                                    new
                                        {
                                            Favor = p,
                                            Pro = f
                                        }
                );
            var linqProd = linq.Where(v => v.FavoriteSourceType == (int)SourceType.Product)
                              .Join(prodList, p => p.FavoriteSourceId,
                                    f => f.Id,
                                    (p, f) =>
                                                                        new
                                                                        {
                                                                            Favor = p,
                                                                            Prod = f
                                                                        }
                );

            var itemRepoonse = ItemsInfoResponseMapping(linqProd.Select(v => v.Prod), linqPro.Select(v => v.Pro), coordinateInfo, false);

            //sort
            //itemRepoonse = itemRepoonse.OrderByDescending(v => v.CreatedDate);
            var s = source.ToDictionary(v => String.Concat(v.FavoriteSourceId, "_", v.FavoriteSourceType), v => v);
            var list = FavoriteInfoResponseMapping(itemRepoonse, s);

            return list.OrderByDescending(v => v.CreatedDate).ToList();
        }

        public List<FavoriteInfoResponse> FavoriteInfoResponseMapping(IEnumerable<ItemsInfoResponse> source, Dictionary<string, FavoriteEntity> favoriteEntities)
        {
            if (source == null)
            {
                return new List<FavoriteInfoResponse>(0);
            }

            var result = new List<FavoriteInfoResponse>();

            foreach (var item in source)
            {
                var target = new FavoriteInfoResponse
                    {
                        CreatedDate = item.CreatedDate,
                        CreatedUser = item.CreatedUser,
                        Description = String.Empty,
                        FavoriteSourceId = item.Id,
                        FavoriteSourceName = item.Name,
                        FavoriteSourceType = item.SourceType,
                        Id = -1,
                        Promotions = item.Promotions,
                        Resources = item.Resources,
                        Status = (int)DataStatus.Normal,
                        Store = item.Store,
                        StoreId = item.Store_Id,
                        User_Id = item.User_Id
                    };

                FavoriteEntity entity;
                favoriteEntities.TryGetValue(String.Concat(target.FavoriteSourceId, "_", target.FavoriteSourceType),
                                             out entity);
                if (entity != null)
                {
                    target.Id = entity.Id;
                    target.CreatedDate = entity.CreatedDate;
                    target.CreatedUser = entity.CreatedUser;
                    target.Description = entity.Description;
                }

                result.Add(target);
            }

            return result;
        }

        public FavoriteCollectionResponse FavoriteCollectionResponseMapping(List<FavoriteEntity> source,
                                                                                   CoordinateInfo coordinateInfo)
        {
            if (source == null || source.Count == 0)
            {
                return new FavoriteCollectionResponse(new PagerRequest(1, 1)) { Favorites = new List<FavoriteInfoResponse>(0) };
            }

            //var storeids = source.Select(v => v.Store_Id).Distinct().Where(v => v != 0);
            //var dic = new Dictionary<int, List<int>>();
            var ms = source.Where(v => v.FavoriteSourceType == (int)SourceType.Promotion).Select(s => s.FavoriteSourceId).Distinct().ToList();
            var ps = source.Where(v => v.FavoriteSourceType == (int)SourceType.Product).Select(s => s.FavoriteSourceId).Distinct().ToList();

            //var stores = StoreResponseMapping(_storeRepository.GetListByIds(storeids.ToList()), coordinateInfo).ToList();
            var promotions = _promotionRepository.GetList(ms);
            var products = _productRepository.GetList(ps);

            var items = ItemsInfoResponseMapping(products, promotions).ToList();

            var t = new List<FavoriteInfoResponse>(items.Count);
            foreach (var i in items)
            {
                var a = Mapper.Map<ItemsInfoResponse, FavoriteInfoResponse>(i);
                a.FavoriteSourceId = i.Id;
                a.FavoriteSourceName = i.Name;
                a.FavoriteSourceType = i.SourceType;
                var r = source.FirstOrDefault(v => v.FavoriteSourceId == i.Id && v.FavoriteSourceType == i.SourceType);

                a.Id = r == null ? 0 : r.Id;
                a.StoreId = i.Store_Id;
                t.Add(a);
            }


            //TODO: 产品这个需要优化
            var result = new FavoriteCollectionResponse(new PagerRequest(1, source.Count), source.Count)
            {
                Favorites = t
            };

            return result;

            ////pp rs

            //var productResource = ResourceInfoResponsesMapping(GetListResourceEntities(SourceType.Product, ps)).ToList();
            //var msresource = ResourceInfoResponsesMapping(GetListResourceEntities(SourceType.Promotion, ms)).ToList();

            ////TODO: 产品这个需要优化
            //var result = new FavoriteCollectionResponse(new PagerRequest(1, source.Count), source.Count)
            //{
            //    Favorites = new List<FavoriteInfoResponse>(source.Count)
            //};
            //foreach (var s in source)
            //{
            //    //var n = String.Empty;
            //    FavoriteInfoResponse response = null;
            //    switch (s.FavoriteSourceType)
            //    {
            //        case (int)SourceType.Promotion:
            //            var p = promotions.FirstOrDefault(v => v.Id == s.FavoriteSourceId);
            //            response = FavoriteInfoResponseMapping(s, p);

            //            response.Resources = p == null ? new List<ResourceInfoResponse>(0) : msresource.Where(v => v.SourceId == p.Id && v.SourceType == (int)SourceType.Promotion).ToList();
            //            break;
            //        case (int)SourceType.Product:
            //            var t = products.FirstOrDefault(v => v.Id == s.FavoriteSourceId);
            //            response = FavoriteInfoResponseMapping(s, t);

            //            response.Resources = t == null ? new List<ResourceInfoResponse>(0) :
            //                productResource.Where(v => v.SourceId == t.Id && v.SourceType == (int)SourceType.Product).ToList();

            //            break;
            //    }

            //    if (response != null)
            //    {
            //        var store = stores.SingleOrDefault(v => v.Id == s.Store_Id);
            //        response.Store = store;
            //        response.StoreId = store == null ? 0 : store.Id;

            //        result.Favorites.Add(response);
            //    }
            //}

            //return result;
        }

        /// <summary>
        /// 还没做产品的需求
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public FavoriteCollectionResponse FavoriteCollectionResponseMapping(List<FavoriteEntity> source)
        {
            return FavoriteCollectionResponseMapping(source, null);
        }

        #endregion

        #region brand

        public BrandEntity BrandEntityMapping(BrandEntity source, BrandEntity target)
        {
            //不变的
            //不变的要映射
            var result = Mapper.Map(source, target);

            return result;
        }

        public BrandInfoResponse BrandInfoResponseMapping(BrandEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<BrandEntity, BrandInfoResponse>(source);

            if (!String.IsNullOrWhiteSpace(target.Logo))
            {
                if (!target.Logo.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    target.Logo = ConfigManager.GetHttpApiImagePath() + target.Logo;
                }
            }

            return target;
        }

        public IEnumerable<BrandInfoResponse> BrandInfoResponseMapping(List<BrandEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<BrandInfoResponse>(0);
            }

            var list = new List<BrandInfoResponse>(source.Count);
            foreach (var s in source)
            {
                var r = BrandInfoResponseMapping(s);
                if (r != null)
                {
                    list.Add(r);
                }
            }

            return list;
        }

        public GroupStructInfoResponse<BrandInfoResponse> BrandInfoResponse4GroupMapping(List<BrandEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            var list = new GroupStructInfoResponse<BrandInfoResponse>();
            foreach (var s in source)
            {
                var r = BrandInfoResponseMapping(s);

                if (r != null)
                {
                    var key = String.IsNullOrWhiteSpace(r.Group) ? "#" : r.Group.ToUpper();
                    if (list.ContainsKey(key))
                    {
                        list[key].Add(r);
                    }
                    else
                    {
                        list.Add(key, new List<BrandInfoResponse> { r });
                    }
                }
            }

            return list;
        }

        public BrandEntity BrandEntityMapping(BrandInfoRequest source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<BrandInfoRequest, BrandEntity>(source);
            target.CreatedDate = DateTime.Now;
            target.CreatedUser = source.AuthUid;
            target.UpdatedDate = DateTime.Now;
            target.UpdatedUser = source.AuthUid;
            target.Status = (int)DataStatus.Normal;

            return target;
        }

        #endregion

        #region devicetoken

        public DeviceInfoResponse DeviceInfoResponseMapping(DeviceTokenEntity source)
        {
            var target = Mapper.Map<DeviceTokenEntity, DeviceInfoResponse>(source);

            return target;
        }

        public DeviceLogInfoResponse DeviceLogInfoResponseMapping(DeviceLogEntity source)
        {
            var target = Mapper.Map<DeviceLogEntity, DeviceLogInfoResponse>(source);

            return target;
        }

        #endregion

        #region comment

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="userEntities"></param>
        /// <returns></returns>
        public CommentInfoResponse CommentInfoResponseMapping(CommentEntity source, List<UserEntity> userEntities, List<ResourceInfoResponse> resource, float currentVer)
        {
            var target = Mapper.Map<CommentEntity, CommentInfoResponse>(source);
            //找两个用户，当前发评论的用户，被回复的用户
            var userResponses = ShowCustomerInfoResponseMapping(userEntities).ToList();

            target.ResourceInfoResponses = resource;

            if (currentVer < 2.1)
            {
                //
                if (target.ResourceInfoResponses != null && target.ResourceInfoResponses.Count > 0)
                {
                    target.Content = "系统提示：“下载最新版本，参与语音互动！”";
                }
            }

            var user = userResponses.FirstOrDefault(v => v.Id == target.User_Id);
            var replyUser = userResponses.FirstOrDefault(v => v.Id == target.ReplyUser);

            target.Customer = user;
            target.ReplyUserNickname = (replyUser == null) ? String.Empty : (String.IsNullOrEmpty(replyUser.Nickname) ? replyUser.Name : replyUser.Nickname);

            return target;
        }

        public CommentInfoResponse CommentInfoResponseMapping(CommentEntity source, float currentVer)
        {
            if (source == null)
            {
                return null;
            }

            //找两个用户，当前发评论的用户，被回复的用户
            var uids = new List<int>(2) { source.User_Id, source.ReplyUser };

            var users = _customerRepository.GetListByIds(uids.Where(v => v != 0).Distinct().ToList());
            var resourec = ResourceInfoResponsesMapping(GetListResourceEntities(SourceType.CommentAudio, source.Id));

            return CommentInfoResponseMapping(source, users, resourec.ToList(), currentVer);
        }

        public IEnumerable<CommentInfoResponse> CommentInfoResponseMapping(List<CommentEntity> source, float currentVer)
        {
            if (source == null || source.Count == 0)
            {
                return new List<CommentInfoResponse>(0);
            }

            var users = new List<int>(source.Count * 2);

            users.AddRange(source.Select(v => v.User_Id));
            users.AddRange(source.Select(v => v.ReplyUser));

            var uids = users.Distinct().Where(v => v != 0);
            var resources = ResourceInfoResponsesMapping(base.GetListResourceEntities(SourceType.CommentAudio, source.Select(v => v.Id).ToList()));
            var userEntities = _customerRepository.GetListByIds(uids.ToList());

            var target = new List<CommentInfoResponse>(source.Count);

            foreach (var s in source)
            {
                var t = CommentInfoResponseMapping(s, userEntities, resources.Where(v => v.SourceId == s.Id).ToList(), currentVer);

                if (t != null)
                {
                    target.Add(t);
                }
            }

            return target;
        }

        #endregion

        #region tag

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public TagEntity TagInfoRequestMapping(TagInfoRequest source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<TagInfoRequest, TagEntity>(source);

            return target;
        }

        public TagInfoResponse TagInfoResponseMapping(TagEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<TagEntity, TagInfoResponse>(source);

            return target;
        }

        public IEnumerable<TagInfoResponse> TagInfoResponseMapping(List<TagEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<TagInfoResponse>(0);
            }

            var list = new List<TagInfoResponse>(source.Count);
            foreach (var s in source)
            {
                var target = TagInfoResponseMapping(s);
                if (target == null)
                {
                    continue;
                }

                list.Add(target);
            }

            return list;
        }

        #endregion

        #region product

        public ProductEntity ProductEntityMapping(ProductEntity source, ProductEntity target)
        {
            var result = Mapper.Map(source, target);

            return result;
        }

        public ProductEntity ProductEntityMapping(ProductInfoRequest source)
        {
            if (source == null)
            {
                return null;
            }
            
            var target = Mapper.Map<ProductInfoRequest, ProductEntity>(source);
            target.CreatedDate = DateTime.Now;
            target.CreatedUser = source.AuthUid;
            target.UpdatedDate = DateTime.Now;
            target.UpdatedUser = source.AuthUid;
            target.Status = (int)DataStatus.Normal;
            target.Name = CheckString(target.Name);
            target.Description = CheckString(target.Description);
            target.Favorable = CheckString(target.Favorable);
            target.RecommendedReason = CheckString(target.RecommendedReason);
            

            return target;
        }

        public IEnumerable<ProductEntity> ProductEntityMapping(List<ProductInfoRequest> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<ProductEntity>(0);
            }

            var list = new List<ProductEntity>(source.Count);
            foreach (var s in source)
            {
                var target = ProductEntityMapping(s);
                if (target == null)
                {
                    continue;
                }

                list.Add(target);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="brandInfo"></param>
        /// <param name="storeInfo"></param>
        /// <param name="showCustomerInfo"></param>
        /// <param name="tagInfoResponse"></param>
        /// <param name="resourceInfoResponses"></param>
        /// <param name="promotions"></param>
        /// <returns></returns>
        public ProductInfoResponse ProductInfoResponseMapping(ProductEntity source, BrandInfoResponse brandInfo,
                                                                     StoreInfoResponse storeInfo,
                                                                     ShowCustomerInfoResponse showCustomerInfo,
                                                                     TagInfoResponse tagInfoResponse, List<ResourceInfoResponse> resourceInfoResponses, List<PromotionInfo> promotions = null)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<ProductEntity, ProductInfoResponse>(source);

            target.BrandInfoResponse = brandInfo;
            target.StoreInfoResponse = storeInfo;
            target.RecommendUserInfoResponse = showCustomerInfo;
            target.TagInfoResponse = tagInfoResponse;
            target.ResourceInfoResponses = resourceInfoResponses;
            target.Promotions = promotions;

            return target;
        }

        public ProductInfoResponse ProductInfoResponseMapping(ProductEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var brand = BrandInfoResponseMapping(_brandRepository.GetItem(source.Brand_Id));
            var store = StoreResponseMapping(_storeRepository.GetItem(source.Store_Id));
            var ruser = ShowCustomerInfoResponseMapping(_customerRepository.GetItem(source.RecommendUser));
            var tag = TagInfoResponseMapping(_tagRepository.GetItem(source.Tag_Id));
            var resources = ResourceInfoResponsesMapping(GetListResourceEntities(SourceType.Product, source.Id));
            var pprs = GetPromotionForRelation(new List<int>(1) { source.Id });

            var promotions = GetPromotionInfos4V(pprs.Select(v => v.ProId ?? 0).ToList());

            return ProductInfoResponseMapping(source, brand, store, ruser, tag, resources.ToList(), promotions);
        }

        public IEnumerable<ProductInfoResponse> ProductInfoResponseMapping(IQueryable<ProductEntity> source)
        {
            if (source == null)
            {
                return new List<ProductInfoResponse>(0);
            }

            var linq = source;

            var rp = linq.GroupJoin(_resourceRepository.Get(DataStatus.Normal, SourceType.Product),
                  p => p.Id, f => f.SourceId, (p, f) => new
                  {
                      P = p,
                      R = f.OrderByDescending(rf=>rf.SortOrder).FirstOrDefault()
                  }).ToList();
            var rl = linq.Join(_pprRepository.Get(DataStatus.Normal), p => p.Id, f => f.ProdId, (p, f) => f);
            var pp = rl.Join(_promotionRepository.Get(new PromotionFilter
                {
                    DataStatus = DataStatus.Normal,
                    FilterMode = PromotionFilterMode.InProgress,
                    HasProduct = true
                }), p => p.ProId, f => f.Id, (p, f) =>
               new
                   {
                       Pro = f,
                       Rel = p
                   }
        ).ToList();

            var ppR = pp.Select(v => new
                {
                    Pro = PromotionInfoMapping(v.Pro),
                    v.Rel
                }).ToList();

            var dic = new Dictionary<int, ProductInfoResponse>();

            foreach (var item in rp)
            {
                if (item == null)
                {
                    continue;
                }

                ProductInfoResponse target;
                var targetResource = ResourceInfoResponsesMapping(item.R);
                if (dic.TryGetValue(item.P.Id, out target))
                {
                    if (item.R != null)
                    {
                        target.ResourceInfoResponses.Add(targetResource);
                    }
                }
                else
                {
                    var pros = ppR.Where(v => v.Rel.ProdId == item.P.Id).Select(v => v.Pro).ToList();

                    target = ProductInfoResponseMapping(item.P, null, null, null, null,targetResource==null?null:new List<ResourceInfoResponse> { targetResource }, pros);
                    dic.Add(item.P.Id, target);
                }
            }

            var result = dic.Values.ToList();

            foreach (var item in result)
            {
                if (item.ResourceInfoResponses!=null && item.ResourceInfoResponses.Count > 1)
                {
                    item.ResourceInfoResponses = item.ResourceInfoResponses.OrderByDescending(v => v.SortOrder).ToList();
                }
            }

            return result;
        }

        public IEnumerable<ProductInfoResponse> ProductInfoResponseMapping(List<ProductEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<ProductInfoResponse>(0);
            }

            var list = new List<ProductInfoResponse>(source.Count);

            var brands = BrandInfoResponseMapping(_brandRepository.GetListByIds(source.Select(v => v.Brand_Id).Distinct().ToList())).ToList();
            var stores = StoreResponseMapping(_storeRepository.GetListByIds(source.Select(v => v.Store_Id).Distinct().ToList())).ToList();
            var rusers = ShowCustomerInfoResponseMapping(_customerRepository.GetListByIds(source.Select(v => v.RecommendUser).Distinct().ToList())).ToList();
            // var tags = TagInfoResponseMapping(_tagRepository.GetListByIds(source.Select(v => v.Tag_Id).Distinct().ToList())).ToList();
            var ids = source.Select(v => v.Id).ToList();
            var resources = ResourceInfoResponsesMapping(GetListResourceEntities(SourceType.Product, ids)).ToList();

            var pprs = GetPromotionForRelation(ids);

            var promotions = GetPromotionInfos4V(pprs.Select(v => v.ProId ?? 0).ToList());

            foreach (var item in source)
            {
                if (item == null)
                {
                    continue;
                }

                var pids = pprs.Where(v => v.ProdId == item.Id).Select(v => v.ProId).Distinct().ToList();
                var ps = promotions.Where(v => pids.Any(s => s == v.Id)).ToList();

                var target = ProductInfoResponseMapping(item, brands.FirstOrDefault(v => v.Id == item.Brand_Id), stores.FirstOrDefault(v => v.Id == item.Store_Id), rusers.FirstOrDefault(v => v.Id == item.RecommendUser), null, resources.Where(v => v.SourceId == item.Id && v.Type==1).ToList(), ps);
                //var target = ProductInfoResponseMapping(item, null, null, null, null, resources.Where(v => v.SourceId == item.Id).ToList(), ps);
                if (target == null)
                {
                    continue;
                }

                list.Add(target);
            }

            return list;
        }

        #endregion

        //TODO: 没有对返回的user列表和当前用户做 互相关注判断。
        #region like

        public LikeCoutomerResponse LikeInfoResponseMapping(LikeEntity source, LikeType likeType)
        {
            if (source == null)
            {
                return null;
            }

            var t = LikeInfoResponseMapping(new List<LikeEntity> { source }, likeType);

            if (t == null || t.Count == 0)
            {
                return null;
            }

            return new LikeCoutomerResponse { CustomerInfoResponse = t[0], Id = source.Id };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="likeType">true 是喜欢（返回的喜欢人的列表） false 是被喜欢（返回的是被喜欢人的列表）</param>
        /// <returns></returns>
        public List<ShowCustomerInfoResponse> LikeInfoResponseMapping(List<LikeEntity> source, LikeType likeType)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }
            List<int> mpUserList = null;

            if (likeType == LikeType.ILike)
            {
                mpUserList = source.Select(v => v.LikedUserId).Distinct().ToList();
            }
            else
            {
                if (likeType == LikeType.LikeMe)
                {
                    mpUserList = source.Select(v => v.LikeUserId).Distinct().ToList();
                }
            }

            List<ShowCustomerInfoResponse> users = null;

            if (mpUserList != null)
            {
                users = ShowCustomerInfoResponseMapping(_customerRepository.GetListByIds(mpUserList.ToList())).ToList();
            }

            return users ?? new List<ShowCustomerInfoResponse>(0);
        }

        #endregion

        #region point

        public PointInfoResponse PointInfoResponseMapping(PointHistoryEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<PointHistoryEntity, PointInfoResponse>(source);

            return target;
        }

        public List<PointInfoResponse> PointInfoResponseMapping(List<PointHistoryEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<PointInfoResponse>(0);
            }

            var result = new List<PointInfoResponse>(source.Count);
            foreach (var item in source)
            {
                var target = PointInfoResponseMapping(item);

                if (target == null)
                {
                    continue;
                }

                result.Add(target);
            }

            return result;
        }

        #endregion

        #region items

        public IEnumerable<ItemsInfoResponse> ItemsInfoResponseMapping(List<ProductEntity> productEntities,
                                                                       List<PromotionEntity> promotionEntities)
        {
            if (productEntities == null && promotionEntities == null)
            {
                return new List<ItemsInfoResponse>(0);
            }

            var storeIds = new List<int>();

            var dresources = new List<ResourceInfoResponse>();
            var mresources = new List<ResourceInfoResponse>();
            //promotionids
            List<Promotion2ProductEntity> pprs = null;

            if (productEntities != null)
            {
                storeIds.AddRange(productEntities.Select(v => v.Store_Id));
                var productIds = productEntities.Select(v => v.Id).Distinct().ToList();

                //pp rs
                pprs = GetPromotionForRelation(productIds);

                dresources.AddRange(ResourceInfoResponsesMapping(GetListResourceEntities(SourceType.Product, productIds)));
            }
            else
            {
                pprs = new List<Promotion2ProductEntity>(0);
            }

            if (promotionEntities != null)
            {
                storeIds.AddRange(promotionEntities.Select(v => v.Store_Id));
                var promotionIds = promotionEntities.Select(v => v.Id).Distinct().ToList();

                mresources.AddRange(ResourceInfoResponsesMapping(GetListResourceEntities(SourceType.Promotion, promotionIds)));
            }

            storeIds = storeIds.Distinct().ToList();

            var store = StoreResponseMapping(_storeRepository.GetListByIds(storeIds)).ToList();

            //查 p
            var tt = pprs.Select(v => v.ProId ?? 0).Distinct().ToList();
            var pinfo = GetPromotionInfos4V(tt);

            var result = new List<ItemsInfoResponse>();

            if (productEntities != null)
            {
                foreach (var item in productEntities)
                {
                    var t = pprs.Where(v => v.ProdId == item.Id).Select(v => v.ProId);
                    var p = pinfo.Where(v => t.Any(s => s == v.Id)).ToList();

                    var target = ItemsInfoResponseMapping(item, store.SingleOrDefault(v => v.Id == item.Store_Id),
                                                          dresources.Where(v => v.SourceId == item.Id).ToList(), p);

                    result.Add(target);
                }
            }

            if (promotionEntities != null)
            {
                foreach (var item in promotionEntities)
                {
                    var target = ItemsInfoResponseMapping(item, store.SingleOrDefault(v => v.Id == item.Store_Id),
                                                          mresources.Where(v => v.SourceId == item.Id).ToList());

                    result.Add(target);
                }
            }

            return result;
        }

        private static ItemsInfoResponse ItemsInfoResponseMapping(ProductEntity source, StoreInfoResponse store, List<ResourceInfoResponse> resources)
        {
            return ItemsInfoResponseMapping(source, store, resources, null);
        }

        private static ItemsInfoResponse ItemsInfoResponseMapping(ProductEntity source, StoreInfoResponse store, List<ResourceInfoResponse> resources, List<PromotionInfo> promotionInfos)
        {
            var target = Mapper.Map<ProductEntity, ItemsInfoResponse>(source);

            target.SType = SourceType.Product;
            target.Store = store;
            target.Resources = resources;
            target.Promotions = promotionInfos;

            return target;
        }

        private static ItemsInfoResponse ItemsInfoResponseMapping(ProductInfoResponse productInfoResponse)
        {
            var target = Mapper.Map<ProductInfoResponse, ItemsInfoResponse>(productInfoResponse);

            target.SType = SourceType.Product;
            target.Store = productInfoResponse.StoreInfoResponse;
            target.Resources = productInfoResponse.ResourceInfoResponses;
            target.Promotions = productInfoResponse.Promotions;

            return target;
        }

        private ItemsInfoResponse ItemsInfoResponseMapping(PromotionInfoResponse source)
        {
            var target = Mapper.Map<PromotionInfoResponse, ItemsInfoResponse>(source);

            target.SType = SourceType.Promotion;
            target.Store = source.StoreInfoResponse;
            target.Resources = source.ResourceInfoResponses;

            var info = PromotionInfoMapping(source);
            target.Promotions = new List<PromotionInfo> { info };

            return target;
        }

        private ItemsInfoResponse ItemsInfoResponseMapping(PromotionEntity source, StoreInfoResponse store, List<ResourceInfoResponse> resources)
        {
            return ItemsInfoResponseMapping(source, store, resources, null);
        }

        private static ItemsInfoResponse ItemsInfoResponseMapping(PromotionEntity source, StoreInfoResponse store, List<ResourceInfoResponse> resources, PromotionInfo info)
        {
            var target = Mapper.Map<PromotionEntity, ItemsInfoResponse>(source);

            target.SType = SourceType.Promotion;
            target.Store = store;
            target.Resources = resources;

            if (info != null)
            {
                target.Promotions = new List<PromotionInfo> { info };
            }

            return target;
        }

        public IEnumerable<ItemsInfoResponse> ItemsInfoResponseMapping(IQueryable<ProductEntity> productEntities,
                                                                        IQueryable<PromotionEntity> promotionEntities, CoordinateInfo coordinateInfo, bool? hasBanner)
        {
            if (productEntities == null && promotionEntities == null)
            {
                return new List<ItemsInfoResponse>(0);
            }

            var linqProResponse = PromotionResponseMapping(promotionEntities, coordinateInfo, hasBanner).ToList();
            var linqProdResponse = ProductInfoResponseMapping(productEntities).ToList();


            var result = new List<ItemsInfoResponse>();

            if (productEntities != null)
            {
                foreach (var item in linqProdResponse)
                {
                    var target = ItemsInfoResponseMapping(item);
                    result.Add(target);
                }
            }

            if (promotionEntities != null)
            {
                foreach (var item in linqProResponse)
                {
                    var target = ItemsInfoResponseMapping(item);

                    result.Add(target);
                }
            }

            //排序
            return result;
        }


        #endregion

        #region specialtopic

        public SpecialTopicInfoResponse SpecialTopicInfoResponseMapping(SpecialTopicEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var entitys = base.GetListResourceEntities(SourceType.SpecialTopic, source.Id);
            var res = ResourceInfoResponsesMapping(entitys).ToList();

            return SpecialTopicInfoResponseMapping(source, res);
        }

        private static SpecialTopicInfoResponse SpecialTopicInfoResponseMapping(SpecialTopicEntity source, List<ResourceInfoResponse> resourceInfoResponses)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<SpecialTopicEntity, SpecialTopicInfoResponse>(source);
            target.ResourceInfoResponses = resourceInfoResponses;

            return target;
        }

        public IEnumerable<SpecialTopicInfoResponse> SpecialTopicInfoResponseMapping(List<SpecialTopicEntity> source)
        {
            if (source == null || source.Count == 0)
            {
                return new List<SpecialTopicInfoResponse>(0);
            }

            var result = new List<SpecialTopicInfoResponse>(source.Count);
            var ids = source.Select(v => v.Id).Distinct().ToList();
            var res = ResourceInfoResponsesMapping(base.GetListResourceEntities(SourceType.SpecialTopic, ids)).ToList();

            foreach (var item in source)
            {
                if (item == null)
                {
                    continue;
                }

                var r = res.Where(v => v.SourceId == item.Id).ToList();

                var target = SpecialTopicInfoResponseMapping(item, r);

                if (target != null)
                {
                    result.Add(target);
                }
            }

            return result;
        }

        #endregion

        #region card

        public CardInfoResponse CardInfoResponseMapping(CardEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var target = Mapper.Map<CardEntity, CardInfoResponse>(source);

            return target;
        }

        #endregion
    }
    // */ 
}