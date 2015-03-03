using System.Collections.Specialized;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Repository.Impl;

namespace Yintai.Hangzhou.WebSupport.Ioc
{
    internal class RepositoryIocRegister : BaseIocRegister
    {
        #region Overrides of BaseIocRegister

        public override void Register()
        {
            Current.Register<ICustomerRepository, CustomerRepository>();
            Current.Register<IOutSiteCustomerRepository, OutsiteCustomerRepository>();
            Current.Register<IUserAccountRepository, UserAccountRepository>();
            Current.Register<IStoreRepository, StoreRepository>();
            Current.Register<IVerifyCodeRepository, VerifyCodeRepository>();
            Current.Register<IPromotionRepository, PromotionRepository>();
            Current.Register<IResourceRepository, ResourceRepository>();
            Current.Register<IFavoriteRepository, FavoriteRepository>();

            Current.Register<IShareRepository, ShareRepository>();
            Current.Register<ICouponRepository, CouponRepository>();
            Current.Register<ITimeSeedRepository, TimeSeedRepository>();



            Current.Register<IBrandRepository, BrandRepository>();
            Current.Register<IDeviceTokenRepository, DeviceTokenRepository>();
            Current.Register<IDeviceLogsRepository, DeviceLogsRepository>();

            Current.Register<ICommentRepository, CommentRepository>();
            Current.Register<IRemindRepository, RemindRepository>();
            Current.Register<IProductRepository, ProductRepository>();
            Current.Register<ITagRepository, TagRepository>();

            Current.Register<IUserRoleRepository, UserRoleRepository>();
            Current.Register<IRoleRepository, RoleRepository>();
            Current.Register<IVUserRoleRepository, VUserRoleRepository>();

            Current.Register<ILikeRepository, LikeRepository>();
            Current.Register<IPointRepository, PointRepository>();

            Current.Register<IPromotionBrandRelationRepository, PromotionBrandRelationRepository>();
            Current.Register<IFeedbackRepository, FeedbackRepository>();

            Current.Register<ISpecialTopicProductRelationRepository, SpecialTopicProductRelationRepository>();
            Current.Register<ISpecialTopicRepository, SpecialTopicRepository>();

            Current.Register<ISeedRepository, SeedRepository>();

            Current.Register<IPromotionProductRelationRepository, PromotionProductRelationRepository>();
            Current.Register<IProductBulkRepository, ProductBulkRepository>();
            Current.Register<IAdminAccessRightRepository, AdminAccessRightRepository>();

            Current.Register<ICardRepository, CardRepository>();

            Current.Register<IBannerRepository, BannerRepository>();

            Current.Register<IHotWordRepository, HotWordRepository>();
            Current.Register<IUserAuthRepository, UserAuthRepository>();
            Current.Register<IStoreCouponsRepository, StoreCouponsRepository>();
            Current.Register<IStorePromotionRepository, StorePromotionRepository>();
            Current.Register<IPointOrderRuleRepository, PointOrderRuleRepository>();
            Current.Register<IStorePromotionScopeRepository, StorePromotionScopeRepository>();
            Current.Register<ICouponLogRepository, CouponLogRepository>();
            Current.Register<IOrderItemRepository, OrderItemRepository>();
            Current.Register<IOrderRepository, OrderRepository>();
            Current.Register<IOrderLogRepository, OrderLogRepository>();
            Current.Register<IOutboundRepository, OutboundRepository>();
            Current.Register<IOutboundItemRepository, OutboundItemRepository>();
            Current.Register<ISectionRepository, SectionRepository>();
            Current.Register<IInboundPackRepository, InboundPackageRepository>();
            Current.Register<IStoreRealRepository, StoreRealRepository>();
            Current.Register<ICategoryPropertyRepository, CategoryPropertyRepository>();
            Current.Register<ICategoryPropertyValueRepository, CategoryPropertyValueRepository>();
            Current.Register<IProductPropertyRepository, ProductPropertyRepository>();
            Current.Register<IProductPropertyValueRepository, ProductPropertyValueRepository>();
            Current.Register<IWXReplyRepository, WXReplyRepository>();
            Current.Register<IShippingAddressRepository, ShippingAddressRepository>();
            Current.Register<IShippViaRepository, ShipViaRepository>();
            Current.Register<IRMARepository, RMARepository>();
            Current.Register<IRMALogRepository, RMALogRepository>();
            Current.Register<IRMAItemRepository, RMAItemRepository>();
            Current.Register<IPMessageRepository, PMessageRepository>();
            Current.Register<IProductCode2StoreCodeRepository, ProductCode2StoreCodeRepository>();
            Current.Register<ICategoryRepository, CategoryRepository>();
            Current.Register<IOrder2ExRepository, Order2ExRepository>();
            Current.Register<IRMA2ExRepository, RMA2ExRepository>();
            Current.Register<IInventoryRepository, InventoryRepository>();

            Current.Register<IEFRepository<OrderTransactionEntity>, OrderTransactionRepository>();
            Current.Register<IEFRepository<PaymentNotifyLogEntity>, EFRepository<PaymentNotifyLogEntity>>();
            Current.Register<IEFRepository<ExOrderEntity>, EFRepository<ExOrderEntity>>();
            Current.Register<IEFRepository<IMS_AssociateEntity>, EFRepository<IMS_AssociateEntity>>();
            Current.Register<IEFRepository<IMS_AssociateBrandEntity>, EFRepository<IMS_AssociateBrandEntity>>();
            Current.Register<IEFRepository<IMS_AssociateSaleCodeEntity>, EFRepository<IMS_AssociateSaleCodeEntity>>();
            Current.Register<IEFRepository<IMS_AssociateItemsEntity>, EFRepository<IMS_AssociateItemsEntity>>();
            Current.Register<IEFRepository<IMS_AssociateIncomeEntity>, EFRepository<IMS_AssociateIncomeEntity>>();
            Current.Register<IEFRepository<IMS_AssociateIncomeRequestEntity>, EFRepository<IMS_AssociateIncomeRequestEntity>>();
            Current.Register<IEFRepository<IMS_AssociateIncomeHistoryEntity>, EFRepository<IMS_AssociateIncomeHistoryEntity>>();
            Current.Register<IEFRepository<ProductCode2StoreCodeEntity>, EFRepository<ProductCode2StoreCodeEntity>>();
            Current.Register<IEFRepository<IMS_ComboEntity>, ComboRepository>();
            Current.Register<IEFRepository<IMS_Combo2ProductEntity>, EFRepository<IMS_Combo2ProductEntity>>();

            Current.Register<IEFRepository<IMS_GiftCardEntity>, EFRepository<IMS_GiftCardEntity>>();
            Current.Register<IEFRepository<IMS_GiftCardItemEntity>, EFRepository<IMS_GiftCardItemEntity>>();
            Current.Register<IEFRepository<IMS_GiftCardOrderEntity>, EFRepository<IMS_GiftCardOrderEntity>>();
            Current.Register<IEFRepository<IMS_GiftCardRechargeEntity>, EFRepository<IMS_GiftCardRechargeEntity>>();
            Current.Register<IEFRepository<IMS_GiftCardTransfersEntity>, EFRepository<IMS_GiftCardTransfersEntity>>();
            Current.Register<IEFRepository<IMS_GiftCardUserEntity>, EFRepository<IMS_GiftCardUserEntity>>();
            Current.Register<IEFRepository<IMS_InviteCodeEntity>, EFRepository<IMS_InviteCodeEntity>>();
            Current.Register<IEFRepository<IMS_InviteCodeRequestEntity>, EFRepository<IMS_InviteCodeRequestEntity>>();

            Current.Register<IEFRepository<WX_MenuEntity>, EFRepository<WX_MenuEntity>>();
            Current.Register<IEFRepository<Product2IMSTagEntity>, EFRepository<Product2IMSTagEntity>>();
            Current.Register<IEFRepository<SectionEntity>, EFRepository<SectionEntity>>();
        }

        #endregion
    }
}
