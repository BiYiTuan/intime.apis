using com.intime.fashion.common.message;
using com.intime.fashion.common.message.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.Filters;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class ProductRepository : RepositoryBase<ProductEntity, int>, IProductRepository
    {
        private readonly ISpecialTopicProductRelationRepository _specialTopicProductRelationRepository;
        private readonly IPromotionProductRelationRepository _promotionProductRelationRepository;
        private IUserAuthRepository _userAuthRepo;

        public ProductRepository(IPromotionProductRelationRepository promotionProductRelationRepository
            , ISpecialTopicProductRelationRepository specialTopicProductRelationRepository
            ,IUserAuthRepository userAuthRepo)
        {
            _specialTopicProductRelationRepository = specialTopicProductRelationRepository;
            _promotionProductRelationRepository = promotionProductRelationRepository;
            _userAuthRepo = userAuthRepo;
        }

        #region methods

        /// <summary>
        /// 获取商品Ids
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<int> GetTopicRelationIds(int? id)
        {
            if (id == null || id.Value < 1)
            {
                return new List<int>(0);
            }

            var s = new List<int> { id.Value };

            return GetTopicRelationIds(s);
        }

        /// <summary>
        /// 获取商品Ids
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        private List<int?> GetPromotionRelationIds(int? promotionId)
        {
            if (promotionId == null || promotionId.Value < 1)
            {
                return new List<int?>(0);
            }

            var entities = _promotionProductRelationRepository.GetList(promotionId.Value);

            return entities.Select(v => v.ProdId).Distinct().ToList();
        }

        private IEnumerable<Promotion2ProductEntity> GetListByPromotion4Linq(int id)
        {
            return _promotionProductRelationRepository.GetListByPromotionLinq(id);
        }

        private IEnumerable<SpecialTopicProductRelationEntity> GetTopicRelationIds4Linq(int id)
        {
            var s = new List<int> { id };

            return _specialTopicProductRelationRepository.GetList4Linq(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private List<int> GetTopicRelationIds(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<int>(0);
            }

            var entitys = _specialTopicProductRelationRepository.GetList(ids);




            if (entitys == null || entitys.Count == 0)
            {
                return new List<int>(0);
            }

            var result = entitys.Select(v => v.Product_Id).Distinct().ToList();

            return result;
        }

        private static Expression<Func<ProductEntity, bool>> Filter(DataStatus? dataStatus, Timestamp timestamp,
                                                                       ICollection<int> tagid,
                                                                       int? recommendUser, int? brandId = null, List<int> productIds = null, string productName = null)
        {
            var filter = PredicateBuilder.True<ProductEntity>();
            //always exclude deleted 
            filter = filter.And(v => v.Status != (int)DataStatus.Deleted);
            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus);
            }

            if (timestamp != null)
            {
                switch (timestamp.TsType)
                {
                    case TimestampType.New:
                        filter = filter.And(v => v.UpdatedDate > timestamp.Ts);
                        break;
                    case TimestampType.Old:
                    default:
                        filter = filter.And(v => v.UpdatedDate <= timestamp.Ts);
                        break;
                }
            }
            if (tagid != null && tagid.Count > 0)
            {
                filter = filter.And(v => tagid.Any(s => s == v.Tag_Id));
            }

            if (recommendUser != null)
            {
                filter = filter.And(v => v.RecommendUser == recommendUser.Value);
            }

            if (brandId != null)
            {
                filter = filter.And(v => v.Brand_Id == brandId);
            }

            if (productIds != null && productIds.Count > 0)
            {
                filter = filter.And(v => productIds.Any(s => s == v.Id));
            }

            if (productName != null)
            {
                filter = filter.And(v => v.Name.StartsWith(productName));
            }

            return filter;
        }

        private Expression<Func<ProductEntity, bool>> Filter(ProductFilter productFilter)
        {
            //if (productFilter == null)
            //{
            //    return null;
            //}

            List<int> pids = null;
            //if (productFilter.TopicId != null && productFilter.TopicId > 0)
            //{
            //    pids = GetTopicRelationIds(productFilter.TopicId);
            //    //特殊处理处理
            //    if (pids == null || pids.Count == 0)
            //    {
            //        pids = new List<int> { -1 };
            //    }
            //}

            //if (productFilter.PromotionId != null && productFilter.PromotionId > 0)
            //{
            //    var ids = GetPromotionRelationIds(productFilter.PromotionId);

            //    if (ids != null && ids.Count > 0)
            //    {
            //        if (pids == null)
            //        {
            //            pids = new List<int>(ids.Count);
            //        }

            //        foreach (var id in ids)
            //        {
            //            pids.Add(id ?? 0);
            //        }
            //    }
            //    else
            //    {
            //        //特殊处理处理
            //        if (pids == null || pids.Count == 0)
            //        {
            //            pids = new List<int> { -2 };
            //        }
            //    }
            //}

            //if (pids != null)
            //{
            //    pids = pids.Distinct().ToList();
            //}

            return Filter(productFilter.DataStatus, productFilter.Timestamp, productFilter.TagIds,
                          productFilter.RecommendUser, productFilter.BrandId, pids, productFilter.ProductName);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        private static Func<IQueryable<ProductEntity>, IOrderedQueryable<ProductEntity>> GetOrder(ProductSortOrder sort)
        {
            Func<IQueryable<ProductEntity>, IOrderedQueryable<ProductEntity>> order = null;
            switch (sort)
            {
                case ProductSortOrder.CreatedDateDesc:
                    order = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
                case ProductSortOrder.Default:
                default:
                    order = v => v.OrderByDescending(s => s.SortOrder).ThenByDescending(s => s.CreatedDate);
                    break;
            }

            return order;
        }

        #endregion

        public IQueryable<ProductEntity> Search(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, Timestamp timestamp,
                                  string productName, string brandName, int? recommendUser, List<int> tagids, int? brandId, DataStatus? dataStatus)
        {
            var filter = Filter(dataStatus, timestamp, tagids, recommendUser, brandId);

            if (!String.IsNullOrWhiteSpace(productName))
            {
                filter = filter.And(v => v.Name.Contains(productName));
            }

            totalCount = 0;

            var p1 = base.Get(filter).Take(pagerRequest.PageSize);

            if (!String.IsNullOrEmpty(brandName))
            {
                var p2 = base.Get(Filter(dataStatus, timestamp, tagids, recommendUser, brandId));
                var b = (ServiceLocator.Current.Resolve<IBrandRepository>().Get(DataStatus.Normal) as IQueryable<BrandEntity>).Where(v => v.Name.Contains(brandName) || v.EnglishName.Contains(brandName));
                var r = p2.Join(b, v => v.Brand_Id, j => j.Id, (v, j) => v).Take(pagerRequest.PageSize);

                return r.Union(p1).Take(pagerRequest.PageSize);
            }

            return p1;
        }

        public List<ProductEntity> GetPagedListForSearch(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, Timestamp timestamp,
                                        string productName, int? recommendUser, List<int> tagids, int? brandId)
        {
            var filter = Filter(DataStatus.Normal, timestamp, tagids, recommendUser, brandId);

            if (!String.IsNullOrWhiteSpace(productName))
            {
                filter = filter.And(v => v.Name.StartsWith(productName));
            }

            return
                base.Get(filter, out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, GetOrder(sortOrder))
                    .ToList();
        }

        public List<ProductEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, Timestamp timestamp,
                                 int? tagId, int? recommendUser, int? brandId)
        {
            List<int> tags;
            if (tagId == null)
            {
                tags = null;
            }
            else
            {
                tags = new List<int>(1) { tagId.Value };
            }

            return
                base.Get(Filter(DataStatus.Normal, timestamp, tags, recommendUser, brandId), out totalCount,
                         pagerRequest.PageIndex, pagerRequest.PageSize, GetOrder(sortOrder)).ToList();
        }

        public List<ProductEntity> GetList(List<int> ids)
        {
            return base.Get(v => ids.Any(s => s == v.Id) && v.Status == (int)DataStatus.Normal).ToList();
        }

        public ProductEntity SetSortOrder(ProductEntity entity, int sortOrder, int updateUser)
        {
            var parames = new List<SqlParameter>
                {
                    new SqlParameter("@SortOrder", sortOrder),
                    new SqlParameter("@Id", entity.Id),
                };


            const string sql = "UPDATE [dbo].[Product] SET [SortOrder] = @SortOrder WHERE Id = @Id;";

            var i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sql, parames.ToArray());

            if (i > 0)
            {
                return GetItem(entity.Id);
            }
            else
            {
                return null;
            }
        }

        public ProductEntity SetIsHasImage(int entityId, bool hasImage, DataStatus dataStatus, int updateUser, string des)
        {
            var parames = new List<SqlParameter>
                {
                    new SqlParameter("@IsHasImage", hasImage),
                    new SqlParameter("@Id", entityId),
                    new SqlParameter("@UpdatedDate", DateTime.Now),
                    new SqlParameter("@UpdatedUser", updateUser),
                    new SqlParameter("@Status", (int)dataStatus),
                };

            const string sql = "UPDATE [dbo].[Product] SET [IsHasImage] = @IsHasImage, [UpdatedUser] = @UpdatedUser ,[UpdatedDate] = @UpdatedDate , [Status]= @Status WHERE Id = @Id;";

            var i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sql, parames.ToArray());

            if (i > 0)
            {
                return GetItem(entityId);
            }

            return null;
        }

        public List<ProductEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount,
                                                ProductSortOrder sortOrder, ProductFilter productFilter)
        {
            return Get(pagerRequest, out totalCount, sortOrder, productFilter).ToList();
        }

        public IQueryable<ProductEntity> Get(PagerRequest pagerRequest, out int totalCount, ProductSortOrder sortOrder, ProductFilter productFilter)
        {
            var linq = base.Get(Filter(productFilter));
            //var linq = base.Get(Filter(productFilter), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize,
            //                    GetOrder(sortOrder));

            if (productFilter.TopicId != null)
            {
                linq = linq.Join(GetTopicRelationIds4Linq(productFilter.TopicId.Value), r => r.Id, v => v.Product_Id,
                          (r, v) => r);
            }

            if (productFilter.PromotionId != null)
            {

                linq = linq.Join(GetListByPromotion4Linq(productFilter.PromotionId.Value), r => r.Id, v => v.ProdId,
          (r, v) => r);
            }

            var resetSet = linq;
            var orderBy = GetOrder(sortOrder);
            resetSet = orderBy != null ? orderBy(resetSet).AsQueryable() : resetSet.AsQueryable();
            totalCount = resetSet.Count();

            var skipCount = (pagerRequest.PageIndex - 1) * pagerRequest.PageSize;

            resetSet = skipCount == 0 ? resetSet.Take(pagerRequest.PageSize) : resetSet.Skip(skipCount).Take(pagerRequest.PageSize);

            return resetSet;
        }

        public IQueryable<ProductEntity> Get(ProductSortOrder? sortOrder, ProductFilter productFilter)
        {
            var linq = base.Get(Filter(productFilter));

            if (sortOrder != null)
            {
                var orderBy = GetOrder(sortOrder.Value);
                linq = orderBy != null ? orderBy(linq).AsQueryable() : linq.AsQueryable();
            }

            return linq;
        }

        public override ProductEntity GetItem(int key)
        {
            return base.Find(key);
        }

    

        public ProductEntity SetCount(ProductCountType countType, int id, int count)
        {
            var productEntity = Find(id);
            switch (countType)
            {
                case ProductCountType.FavoriteCount:
                    productEntity.FavoriteCount++;
                    break;
                case ProductCountType.InvolvedCount:
                    productEntity.InvolvedCount++;
                    break;
                case ProductCountType.ShareCount:
                    productEntity.ShareCount++;
                    break;
            }
            productEntity.UpdatedDate = DateTime.Now;
            Update(productEntity);
            return productEntity;
        }

        public override Yintai.Hangzhou.Data.Models.ProductEntity Insert(Yintai.Hangzhou.Data.Models.ProductEntity entity)
        {
            var newEntity = base.Insert(entity);

            this.NotifyMessage<ProductEntity>(() =>
            {
                var messageProvider = ServiceLocator.Current.Resolve<IMessageCenterProvider>();
                messageProvider.GetSender().SendMessageReliable(new CreateMessage()
                {
                    SourceType = (int)MessageSourceType.Product,
                    EntityId = newEntity.Id
                });
            });
           
            return newEntity;
        }
        public override void Update(ProductEntity entity)
        {
            base.Update(entity);

            this.NotifyMessage<ProductEntity>(() => {
                var messageProvider = ServiceLocator.Current.Resolve<IMessageCenterProvider>();
                messageProvider.GetSender().SendMessageReliable(new UpdateMessage()
                {
                    SourceType = (int)MessageSourceType.Product,
                    EntityId = entity.Id
                });
            });

           
        }
    }
}
