using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Objects;
using EFCachingProvider;
using EFCachingProvider.Caching;
using EFTracingProvider;
using EFProviderWrapperToolkit;
using Yintai.Hangzhou.Data.Models.Mapping;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class YintaiHangzhouContext : DbContext
    {
        private static readonly Architecture.Common.Logger.ILog _log;

        static YintaiHangzhouContext()
        {
            Database.SetInitializer<YintaiHangzhouContext>(null);
            _log = Architecture.Framework.ServiceLocation.ServiceLocator.Current.Resolve<Architecture.Common.Logger.ILog>();
        }

		/// <summary>
        /// 正式环境使用，无跟踪
        /// </summary>
        public YintaiHangzhouContext()
            : this("Name=YintaiHangzhouContext", "v1")
        {
        }

        /// <summary>
        /// 正式环境使用，无跟踪
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        /// <param name="version"></param>
        public YintaiHangzhouContext(string nameOrConnectionString, string version)
            : base(nameOrConnectionString)
        {
        }

        #region ef tracing

		public YintaiHangzhouContext(string nameOrConnectionString)
            : this(nameOrConnectionString, new InMemoryCache(512), CachingPolicy.CacheAll)
        {
        }

        public YintaiHangzhouContext(string nameOrConnectionString, ICache cacheProvider, CachingPolicy cachingPolicy)
            : base(Architecture.Common.Data.EF.EFTracingUtil.GetConnection(nameOrConnectionString), true)
        {
			var ctx = ((IObjectContextAdapter)this).ObjectContext;

            this.ObjectContext = ctx;

            EFTracingConnection tracingConnection;
            if (ObjectContext.TryUnwrapConnection(out tracingConnection))
            {
                ctx.GetTracingConnection().CommandExecuting += (s, e) => _log.Debug(e.ToTraceString());
            }

            EFCachingConnection cachingConnection;
            if (ObjectContext.TryUnwrapConnection(out cachingConnection))
            {
                Cache = cacheProvider;
                CachingPolicy = cachingPolicy;
            }
        }

        #endregion

		#region Tracing Extensions

        private ObjectContext ObjectContext { get; set; }

        private EFTracingConnection TracingConnection
        {
            get { return ObjectContext.UnwrapConnection<EFTracingConnection>(); }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandExecuting
        {
            add { this.TracingConnection.CommandExecuting += value; }
            remove { this.TracingConnection.CommandExecuting -= value; }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandFinished
        {
            add { this.TracingConnection.CommandFinished += value; }
            remove { this.TracingConnection.CommandFinished -= value; }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandFailed
        {
            add { this.TracingConnection.CommandFailed += value; }
            remove { this.TracingConnection.CommandFailed -= value; }
        }

        #endregion

        #region Caching Extensions

        private EFCachingConnection CachingConnection
        {
            get { return ObjectContext.UnwrapConnection<EFCachingConnection>(); }
        }

        public ICache Cache
        {
            get { return CachingConnection.Cache; }
            set { CachingConnection.Cache = value; }
        }

        public CachingPolicy CachingPolicy
        {
            get { return CachingConnection.CachingPolicy; }
            set { CachingConnection.CachingPolicy = value; }
        }

        #endregion

		#region code reverse
        public DbSet<AdminAccessRightEntity> AdminAccessRights { get; set; }
        public DbSet<BannerEntity> Banners { get; set; }
        public DbSet<BrandEntity> Brands { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<CardBlackEntity> CardBlacks { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CategoryMapEntity> CategoryMaps { get; set; }
        public DbSet<CategoryPropertyEntity> CategoryProperties { get; set; }
        public DbSet<CategoryPropertyValueEntity> CategoryPropertyValues { get; set; }
        public DbSet<ChannelEntity> Channels { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<ConfigMsgEntity> ConfigMsgs { get; set; }
        public DbSet<CouponHistoryEntity> CouponHistories { get; set; }
        public DbSet<CouponLogEntity> CouponLogs { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<DeviceLogEntity> DeviceLogs { get; set; }
        public DbSet<DeviceTokenEntity> DeviceTokens { get; set; }
        public DbSet<ExOrderEntity> ExOrders { get; set; }
        public DbSet<FavoriteEntity> Favorites { get; set; }
        public DbSet<FeedbackEntity> Feedbacks { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<Group_AliKeysEntity> Group_AliKeys { get; set; }
        public DbSet<Group_WeixinKeysEntity> Group_WeixinKeys { get; set; }
        public DbSet<HotWordEntity> HotWords { get; set; }
        public DbSet<IMS_AssociateEntity> IMS_Associate { get; set; }
        public DbSet<IMS_AssociateBrandEntity> IMS_AssociateBrand { get; set; }
        public DbSet<IMS_AssociateIncomeEntity> IMS_AssociateIncome { get; set; }
        public DbSet<IMS_AssociateIncomeHistoryEntity> IMS_AssociateIncomeHistory { get; set; }
        public DbSet<IMS_AssociateIncomeRequestEntity> IMS_AssociateIncomeRequest { get; set; }
        public DbSet<IMS_AssociateIncomeRuleEntity> IMS_AssociateIncomeRule { get; set; }
        public DbSet<IMS_AssociateIncomeRuleFixEntity> IMS_AssociateIncomeRuleFix { get; set; }
        public DbSet<IMS_AssociateIncomeRuleFlattenEntity> IMS_AssociateIncomeRuleFlatten { get; set; }
        public DbSet<IMS_AssociateIncomeRuleFlexEntity> IMS_AssociateIncomeRuleFlex { get; set; }
        public DbSet<IMS_AssociateIncomeRuleMultipleEntity> IMS_AssociateIncomeRuleMultiple { get; set; }
        public DbSet<IMS_AssociateIncomeTran2ReqEntity> IMS_AssociateIncomeTran2Req { get; set; }
        public DbSet<IMS_AssociateIncomeTransferEntity> IMS_AssociateIncomeTransfer { get; set; }
        public DbSet<IMS_AssociateItemsEntity> IMS_AssociateItems { get; set; }
        public DbSet<IMS_AssociateSaleCodeEntity> IMS_AssociateSaleCode { get; set; }
        public DbSet<IMS_BankEntity> IMS_Bank { get; set; }
        public DbSet<IMS_ComboEntity> IMS_Combo { get; set; }
        public DbSet<IMS_Combo2ProductEntity> IMS_Combo2Product { get; set; }
        public DbSet<IMS_GiftCardEntity> IMS_GiftCard { get; set; }
        public DbSet<IMS_GiftCardItemEntity> IMS_GiftCardItem { get; set; }
        public DbSet<IMS_GiftCardOrderEntity> IMS_GiftCardOrder { get; set; }
        public DbSet<IMS_GiftCardRechargeEntity> IMS_GiftCardRecharge { get; set; }
        public DbSet<IMS_GiftCardTransfersEntity> IMS_GiftCardTransfers { get; set; }
        public DbSet<IMS_GiftCardUserEntity> IMS_GiftCardUser { get; set; }
        public DbSet<IMS_InviteCodeEntity> IMS_InviteCode { get; set; }
        public DbSet<IMS_InviteCodeRequestEntity> IMS_InviteCodeRequest { get; set; }
        public DbSet<IMS_SalesCodeEntity> IMS_SalesCode { get; set; }
        public DbSet<IMS_SectionBrandEntity> IMS_SectionBrand { get; set; }
        public DbSet<IMS_SectionOperatorEntity> IMS_SectionOperator { get; set; }
        public DbSet<IMS_TagEntity> IMS_Tag { get; set; }
        public DbSet<InboundPackageEntity> InboundPackages { get; set; }
        public DbSet<InventoryEntity> Inventories { get; set; }
        public DbSet<JobSuccessHistoryEntity> JobSuccessHistories { get; set; }
        public DbSet<LikeEntity> Likes { get; set; }
        public DbSet<Map4BrandEntity> Map4Brand { get; set; }
        public DbSet<Map4CategoryEntity> Map4Category { get; set; }
        public DbSet<Map4InventoryEntity> Map4Inventory { get; set; }
        public DbSet<Map4OrderEntity> Map4Order { get; set; }
        public DbSet<Map4ProductEntity> Map4Product { get; set; }
        public DbSet<Map4StoreEntity> Map4Store { get; set; }
        public DbSet<MappedProductBackupEntity> MappedProductBackups { get; set; }
        public DbSet<NotificationLogEntity> NotificationLogs { get; set; }
        public DbSet<OPC_AuthMenuEntity> OPC_AuthMenu { get; set; }
        public DbSet<OPC_AuthRoleEntity> OPC_AuthRole { get; set; }
        public DbSet<OPC_AuthRoleMenuEntity> OPC_AuthRoleMenu { get; set; }
        public DbSet<OPC_AuthRoleUserEntity> OPC_AuthRoleUser { get; set; }
        public DbSet<OPC_AuthUserEntity> OPC_AuthUser { get; set; }
        public DbSet<OPC_CategoryMapEntity> OPC_CategoryMap { get; set; }
        public DbSet<OPC_ChannelMapEntity> OPC_ChannelMap { get; set; }
        public DbSet<OPC_ChannelProductEntity> OPC_ChannelProduct { get; set; }
        public DbSet<OPC_OrderCommentEntity> OPC_OrderComment { get; set; }
        public DbSet<OPC_OrderSplitLogEntity> OPC_OrderSplitLog { get; set; }
        public DbSet<OPC_OrgInfoEntity> OPC_OrgInfo { get; set; }
        public DbSet<OPC_RMAEntity> OPC_RMA { get; set; }
        public DbSet<OPC_RMACommentEntity> OPC_RMAComment { get; set; }
        public DbSet<OPC_RMADetailEntity> OPC_RMADetail { get; set; }
        public DbSet<OPC_RMALogEntity> OPC_RMALog { get; set; }
        public DbSet<OPC_RMANotificationLogEntity> OPC_RMANotificationLog { get; set; }
        public DbSet<OPC_SaleEntity> OPC_Sale { get; set; }
        public DbSet<OPC_SaleCommentEntity> OPC_SaleComment { get; set; }
        public DbSet<OPC_SaleDetailEntity> OPC_SaleDetail { get; set; }
        public DbSet<OPC_SaleLogEntity> OPC_SaleLog { get; set; }
        public DbSet<OPC_SaleOrderNotificationLogEntity> OPC_SaleOrderNotificationLog { get; set; }
        public DbSet<OPC_SaleRMAEntity> OPC_SaleRMA { get; set; }
        public DbSet<OPC_SaleRMACommentEntity> OPC_SaleRMAComment { get; set; }
        public DbSet<OPC_ShippingSaleEntity> OPC_ShippingSale { get; set; }
        public DbSet<OPC_ShippingSaleCommentEntity> OPC_ShippingSaleComment { get; set; }
        public DbSet<OPC_SKUEntity> OPC_SKU { get; set; }
        public DbSet<OPC_StockEntity> OPC_Stock { get; set; }
        public DbSet<OPC_StockPropertyValueRawEntity> OPC_StockPropertyValueRaw { get; set; }
        public DbSet<OPC_StorePriorityEntity> OPC_StorePriority { get; set; }
        public DbSet<OPC_SupplierInfoEntity> OPC_SupplierInfo { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<Order2ExEntity> Order2Ex { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<OrderLogEntity> OrderLogs { get; set; }
        public DbSet<OrderTransactionEntity> OrderTransactions { get; set; }
        public DbSet<OutboundEntity> Outbounds { get; set; }
        public DbSet<OutboundItemEntity> OutboundItems { get; set; }
        public DbSet<OutsiteUserEntity> OutsiteUsers { get; set; }
        public DbSet<PaymentMethodEntity> PaymentMethods { get; set; }
        public DbSet<PaymentNotifyLogEntity> PaymentNotifyLogs { get; set; }
        public DbSet<PKeyEntity> PKeys { get; set; }
        public DbSet<PMessageEntity> PMessages { get; set; }
        public DbSet<PointHistoryEntity> PointHistories { get; set; }
        public DbSet<PointOrderRuleEntity> PointOrderRules { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<Product2IMSTagEntity> Product2IMSTag { get; set; }
        public DbSet<ProductCode2StoreCodeEntity> ProductCode2StoreCode { get; set; }
        public DbSet<ProductMapEntity> ProductMaps { get; set; }
        public DbSet<ProductPoolEntity> ProductPools { get; set; }
        public DbSet<ProductPropertyEntity> ProductProperties { get; set; }
        public DbSet<ProductPropertyStageEntity> ProductPropertyStages { get; set; }
        public DbSet<ProductPropertyValueEntity> ProductPropertyValues { get; set; }
        public DbSet<ProductStageEntity> ProductStages { get; set; }
        public DbSet<ProductUploadJobEntity> ProductUploadJobs { get; set; }
        public DbSet<PromotionEntity> Promotions { get; set; }
        public DbSet<Promotion2ProductEntity> Promotion2Product { get; set; }
        public DbSet<PromotionBrandRelationEntity> PromotionBrandRelations { get; set; }
        public DbSet<RemindEntity> Reminds { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<ResourceStageEntity> ResourceStages { get; set; }
        public DbSet<RMAEntity> RMAs { get; set; }
        public DbSet<RMA2ExEntity> RMA2Ex { get; set; }
        public DbSet<RMAItemEntity> RMAItems { get; set; }
        public DbSet<RMALogEntity> RMALogs { get; set; }
        public DbSet<RMAReasonEntity> RMAReasons { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<RoleAccessRightEntity> RoleAccessRights { get; set; }
        public DbSet<SectionEntity> Sections { get; set; }
        public DbSet<SectionBrandImportStageEntity> SectionBrandImportStages { get; set; }
        public DbSet<SectionBrandImportStageOutputEntity> SectionBrandImportStageOutputs { get; set; }
        public DbSet<SeedEntity> Seeds { get; set; }
        public DbSet<ShareHistoryEntity> ShareHistories { get; set; }
        public DbSet<ShippingAddressEntity> ShippingAddresses { get; set; }
        public DbSet<ShippingRuleEntity> ShippingRules { get; set; }
        public DbSet<ShippingRuleFixEntity> ShippingRuleFixes { get; set; }
        public DbSet<ShipViaEntity> ShipVias { get; set; }
        public DbSet<SpecialTopicEntity> SpecialTopics { get; set; }
        public DbSet<SpecialTopicProductRelationEntity> SpecialTopicProductRelations { get; set; }
        public DbSet<StoreEntity> Stores { get; set; }
        public DbSet<StoreCouponEntity> StoreCoupons { get; set; }
        public DbSet<StorePromotionEntity> StorePromotions { get; set; }
        public DbSet<StorePromotionScopeEntity> StorePromotionScopes { get; set; }
        public DbSet<StoreRealEntity> StoreReals { get; set; }
        public DbSet<Supplier_BrandEntity> Supplier_Brand { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<testtableEntity> testtables { get; set; }
        public DbSet<TimeSeedEntity> TimeSeeds { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserAccountEntity> UserAccounts { get; set; }
        public DbSet<UserAuthEntity> UserAuths { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<VerifyCodeEntity> VerifyCodes { get; set; }
        public DbSet<WXReplyEntity> WXReplies { get; set; }
        public DbSet<VUserEntity> VUsers { get; set; }
        public DbSet<VUserRoleEntity> VUserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
			Configuration.AutoDetectChangesEnabled = false;
            // 移除复数表名的契约
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // 防止黑幕交易 要不然每次都要访问 EdmMetadata这个表 EF4.1后可注释
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Configurations.Add(new AdminAccessRightEntityMap());
            modelBuilder.Configurations.Add(new BannerEntityMap());
            modelBuilder.Configurations.Add(new BrandEntityMap());
            modelBuilder.Configurations.Add(new CardEntityMap());
            modelBuilder.Configurations.Add(new CardBlackEntityMap());
            modelBuilder.Configurations.Add(new CategoryEntityMap());
            modelBuilder.Configurations.Add(new CategoryMapEntityMap());
            modelBuilder.Configurations.Add(new CategoryPropertyEntityMap());
            modelBuilder.Configurations.Add(new CategoryPropertyValueEntityMap());
            modelBuilder.Configurations.Add(new ChannelEntityMap());
            modelBuilder.Configurations.Add(new CityEntityMap());
            modelBuilder.Configurations.Add(new CommentEntityMap());
            modelBuilder.Configurations.Add(new ConfigMsgEntityMap());
            modelBuilder.Configurations.Add(new CouponHistoryEntityMap());
            modelBuilder.Configurations.Add(new CouponLogEntityMap());
            modelBuilder.Configurations.Add(new DepartmentEntityMap());
            modelBuilder.Configurations.Add(new DeviceLogEntityMap());
            modelBuilder.Configurations.Add(new DeviceTokenEntityMap());
            modelBuilder.Configurations.Add(new ExOrderEntityMap());
            modelBuilder.Configurations.Add(new FavoriteEntityMap());
            modelBuilder.Configurations.Add(new FeedbackEntityMap());
            modelBuilder.Configurations.Add(new GroupEntityMap());
            modelBuilder.Configurations.Add(new Group_AliKeysEntityMap());
            modelBuilder.Configurations.Add(new Group_WeixinKeysEntityMap());
            modelBuilder.Configurations.Add(new HotWordEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateBrandEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeHistoryEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeRequestEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeRuleEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeRuleFixEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeRuleFlattenEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeRuleFlexEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeRuleMultipleEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeTran2ReqEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateIncomeTransferEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateItemsEntityMap());
            modelBuilder.Configurations.Add(new IMS_AssociateSaleCodeEntityMap());
            modelBuilder.Configurations.Add(new IMS_BankEntityMap());
            modelBuilder.Configurations.Add(new IMS_ComboEntityMap());
            modelBuilder.Configurations.Add(new IMS_Combo2ProductEntityMap());
            modelBuilder.Configurations.Add(new IMS_GiftCardEntityMap());
            modelBuilder.Configurations.Add(new IMS_GiftCardItemEntityMap());
            modelBuilder.Configurations.Add(new IMS_GiftCardOrderEntityMap());
            modelBuilder.Configurations.Add(new IMS_GiftCardRechargeEntityMap());
            modelBuilder.Configurations.Add(new IMS_GiftCardTransfersEntityMap());
            modelBuilder.Configurations.Add(new IMS_GiftCardUserEntityMap());
            modelBuilder.Configurations.Add(new IMS_InviteCodeEntityMap());
            modelBuilder.Configurations.Add(new IMS_InviteCodeRequestEntityMap());
            modelBuilder.Configurations.Add(new IMS_SalesCodeEntityMap());
            modelBuilder.Configurations.Add(new IMS_SectionBrandEntityMap());
            modelBuilder.Configurations.Add(new IMS_SectionOperatorEntityMap());
            modelBuilder.Configurations.Add(new IMS_TagEntityMap());
            modelBuilder.Configurations.Add(new InboundPackageEntityMap());
            modelBuilder.Configurations.Add(new InventoryEntityMap());
            modelBuilder.Configurations.Add(new JobSuccessHistoryEntityMap());
            modelBuilder.Configurations.Add(new LikeEntityMap());
            modelBuilder.Configurations.Add(new Map4BrandEntityMap());
            modelBuilder.Configurations.Add(new Map4CategoryEntityMap());
            modelBuilder.Configurations.Add(new Map4InventoryEntityMap());
            modelBuilder.Configurations.Add(new Map4OrderEntityMap());
            modelBuilder.Configurations.Add(new Map4ProductEntityMap());
            modelBuilder.Configurations.Add(new Map4StoreEntityMap());
            modelBuilder.Configurations.Add(new MappedProductBackupEntityMap());
            modelBuilder.Configurations.Add(new NotificationLogEntityMap());
            modelBuilder.Configurations.Add(new OPC_AuthMenuEntityMap());
            modelBuilder.Configurations.Add(new OPC_AuthRoleEntityMap());
            modelBuilder.Configurations.Add(new OPC_AuthRoleMenuEntityMap());
            modelBuilder.Configurations.Add(new OPC_AuthRoleUserEntityMap());
            modelBuilder.Configurations.Add(new OPC_AuthUserEntityMap());
            modelBuilder.Configurations.Add(new OPC_CategoryMapEntityMap());
            modelBuilder.Configurations.Add(new OPC_ChannelMapEntityMap());
            modelBuilder.Configurations.Add(new OPC_ChannelProductEntityMap());
            modelBuilder.Configurations.Add(new OPC_OrderCommentEntityMap());
            modelBuilder.Configurations.Add(new OPC_OrderSplitLogEntityMap());
            modelBuilder.Configurations.Add(new OPC_OrgInfoEntityMap());
            modelBuilder.Configurations.Add(new OPC_RMAEntityMap());
            modelBuilder.Configurations.Add(new OPC_RMACommentEntityMap());
            modelBuilder.Configurations.Add(new OPC_RMADetailEntityMap());
            modelBuilder.Configurations.Add(new OPC_RMALogEntityMap());
            modelBuilder.Configurations.Add(new OPC_RMANotificationLogEntityMap());
            modelBuilder.Configurations.Add(new OPC_SaleEntityMap());
            modelBuilder.Configurations.Add(new OPC_SaleCommentEntityMap());
            modelBuilder.Configurations.Add(new OPC_SaleDetailEntityMap());
            modelBuilder.Configurations.Add(new OPC_SaleLogEntityMap());
            modelBuilder.Configurations.Add(new OPC_SaleOrderNotificationLogEntityMap());
            modelBuilder.Configurations.Add(new OPC_SaleRMAEntityMap());
            modelBuilder.Configurations.Add(new OPC_SaleRMACommentEntityMap());
            modelBuilder.Configurations.Add(new OPC_ShippingSaleEntityMap());
            modelBuilder.Configurations.Add(new OPC_ShippingSaleCommentEntityMap());
            modelBuilder.Configurations.Add(new OPC_SKUEntityMap());
            modelBuilder.Configurations.Add(new OPC_StockEntityMap());
            modelBuilder.Configurations.Add(new OPC_StockPropertyValueRawEntityMap());
            modelBuilder.Configurations.Add(new OPC_StorePriorityEntityMap());
            modelBuilder.Configurations.Add(new OPC_SupplierInfoEntityMap());
            modelBuilder.Configurations.Add(new OrderEntityMap());
            modelBuilder.Configurations.Add(new Order2ExEntityMap());
            modelBuilder.Configurations.Add(new OrderItemEntityMap());
            modelBuilder.Configurations.Add(new OrderLogEntityMap());
            modelBuilder.Configurations.Add(new OrderTransactionEntityMap());
            modelBuilder.Configurations.Add(new OutboundEntityMap());
            modelBuilder.Configurations.Add(new OutboundItemEntityMap());
            modelBuilder.Configurations.Add(new OutsiteUserEntityMap());
            modelBuilder.Configurations.Add(new PaymentMethodEntityMap());
            modelBuilder.Configurations.Add(new PaymentNotifyLogEntityMap());
            modelBuilder.Configurations.Add(new PKeyEntityMap());
            modelBuilder.Configurations.Add(new PMessageEntityMap());
            modelBuilder.Configurations.Add(new PointHistoryEntityMap());
            modelBuilder.Configurations.Add(new PointOrderRuleEntityMap());
            modelBuilder.Configurations.Add(new ProductEntityMap());
            modelBuilder.Configurations.Add(new Product2IMSTagEntityMap());
            modelBuilder.Configurations.Add(new ProductCode2StoreCodeEntityMap());
            modelBuilder.Configurations.Add(new ProductMapEntityMap());
            modelBuilder.Configurations.Add(new ProductPoolEntityMap());
            modelBuilder.Configurations.Add(new ProductPropertyEntityMap());
            modelBuilder.Configurations.Add(new ProductPropertyStageEntityMap());
            modelBuilder.Configurations.Add(new ProductPropertyValueEntityMap());
            modelBuilder.Configurations.Add(new ProductStageEntityMap());
            modelBuilder.Configurations.Add(new ProductUploadJobEntityMap());
            modelBuilder.Configurations.Add(new PromotionEntityMap());
            modelBuilder.Configurations.Add(new Promotion2ProductEntityMap());
            modelBuilder.Configurations.Add(new PromotionBrandRelationEntityMap());
            modelBuilder.Configurations.Add(new RemindEntityMap());
            modelBuilder.Configurations.Add(new ResourceEntityMap());
            modelBuilder.Configurations.Add(new ResourceStageEntityMap());
            modelBuilder.Configurations.Add(new RMAEntityMap());
            modelBuilder.Configurations.Add(new RMA2ExEntityMap());
            modelBuilder.Configurations.Add(new RMAItemEntityMap());
            modelBuilder.Configurations.Add(new RMALogEntityMap());
            modelBuilder.Configurations.Add(new RMAReasonEntityMap());
            modelBuilder.Configurations.Add(new RoleEntityMap());
            modelBuilder.Configurations.Add(new RoleAccessRightEntityMap());
            modelBuilder.Configurations.Add(new SectionEntityMap());
            modelBuilder.Configurations.Add(new SectionBrandImportStageEntityMap());
            modelBuilder.Configurations.Add(new SectionBrandImportStageOutputEntityMap());
            modelBuilder.Configurations.Add(new SeedEntityMap());
            modelBuilder.Configurations.Add(new ShareHistoryEntityMap());
            modelBuilder.Configurations.Add(new ShippingAddressEntityMap());
            modelBuilder.Configurations.Add(new ShippingRuleEntityMap());
            modelBuilder.Configurations.Add(new ShippingRuleFixEntityMap());
            modelBuilder.Configurations.Add(new ShipViaEntityMap());
            modelBuilder.Configurations.Add(new SpecialTopicEntityMap());
            modelBuilder.Configurations.Add(new SpecialTopicProductRelationEntityMap());
            modelBuilder.Configurations.Add(new StoreEntityMap());
            modelBuilder.Configurations.Add(new StoreCouponEntityMap());
            modelBuilder.Configurations.Add(new StorePromotionEntityMap());
            modelBuilder.Configurations.Add(new StorePromotionScopeEntityMap());
            modelBuilder.Configurations.Add(new StoreRealEntityMap());
            modelBuilder.Configurations.Add(new Supplier_BrandEntityMap());
            modelBuilder.Configurations.Add(new TagEntityMap());
            modelBuilder.Configurations.Add(new testtableEntityMap());
            modelBuilder.Configurations.Add(new TimeSeedEntityMap());
            modelBuilder.Configurations.Add(new UserEntityMap());
            modelBuilder.Configurations.Add(new UserAccountEntityMap());
            modelBuilder.Configurations.Add(new UserAuthEntityMap());
            modelBuilder.Configurations.Add(new UserRoleEntityMap());
            modelBuilder.Configurations.Add(new VerifyCodeEntityMap());
            modelBuilder.Configurations.Add(new WXReplyEntityMap());
            modelBuilder.Configurations.Add(new VUserEntityMap());
            modelBuilder.Configurations.Add(new VUserRoleEntityMap());
        }

		#endregion
    }
}
