using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.intime.fashion.service.search;
using com.intime.o2o.data.exchange.Tmall.Product.Models;
using Common.Logging;
using Nest;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.o2o.data.exchange.Tmall.Product.Services.Support
{
    /// <summary>
    /// 提取服务
    /// </summary>
    public class ProductPoolService : IProductPoolService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private const int ChannelId = 1008;
        /**
         * 说明：ProductPool status:
         * 
         * 100: 待处理产品
         * 200：已经成功添加的商品
         * 500：上传商品出错
         * 800：上传产品完成，并完成商品的添加
         * */
        #region 获取待处理的新产品

        public IEnumerable<int> GetPendingProductIds()
        {
            /**
            *   获取ProductPool表中的，待处理产品数据中默认商品的ProductId进行添加商品
             *  默认一次获取10条数据
            */
            using (var db = new YintaiHangzhouContext())
            {
                var list = db.ProductPools.Where(
                    p =>
                        p.Status == 100
                        && p.ChannelId == ChannelId
                        && p.IsDefault == true)
                    .OrderBy(p => p.Id)
                   .Skip(0)
                   .Take(10).ToList();

                return list.ConvertAll(p => p.ProductId);
            }
        }

        #endregion

        #region 获取单品相关方法

        public ESProduct GetProductByProductId(int productId)
        {
            /**===============================================
             * :: 聚合商品信息
             * 1. 根据productId获取聚合后的商品列表
             * 2. 根据ProductIds,获取所有商品
             * 3. 选取默认商品进行商品组装
            ================================================== */

            var productPoolEnitys = GetGroupedProductIdsByProductId(productId);

            if (productPoolEnitys == null)
            {
                Log.ErrorFormat("获取ProductPool列表失败,{0}", productId);
                return null;
            }

            var productIds = productPoolEnitys.Select(p => p.ProductId.ToString(CultureInfo.InvariantCulture));

            /**=================================================================
            * 聚合商品列表
             ===================================================================== */
            var products = GetDefaultElasticClient().Search<ESProduct>(
                body =>
                    body.Filter(q =>
                        q.Terms(p => p.Id, productIds))
                        .Skip(0).
                        Size(5000))
                    .Documents;

            if (products == null)
            {
                Log.ErrorFormat("根据Id获取商品列表出错,productIds", string.Join(",", productIds.ToArray()));
                return null;
            }

            var esProducts = products as IList<ESProduct> ?? products.ToList();

            /**=================================================================
             * 默认商品Id
            ===================================================================== */
            var defaultProductPoolEntity = productPoolEnitys.FirstOrDefault(p => p.IsDefault == true);

            if (defaultProductPoolEntity == null)
            {
                Log.ErrorFormat("ProductPool聚合产品中选取默认商品出错，情进行排查,productId:{0}", productId);
                return null;
            }

            /**=================================================================
             * 获取默认商品
            ===================================================================== */

            var defaultProduct = esProducts.FirstOrDefault(p => p.Id == defaultProductPoolEntity.ProductId);

            if (defaultProduct == null)
            {
                Log.ErrorFormat("ES聚合产品中选取默认商品出错，情进行排查,productId:{0}", productId);
                return null;
            }

            /**=================================================================
            * 进行商品聚合
             * 1. 图片进行聚合
             * 2. 价格获取最低价格
            ===================================================================== */

            var otherProducts = esProducts.Where(p => p.Id != defaultProductPoolEntity.ProductId);

            var resources = new List<ESResource>(defaultProduct.Resource);
            var defaultPrice = defaultProduct.Price;

            foreach (var otherProduct in otherProducts)
            {
                // 聚合图片
                resources.AddRange(otherProduct.Resource);

                // 价格处理
                if (otherProduct.Price < defaultPrice)
                {
                    defaultPrice = otherProduct.Price;
                }
            }

            // 组装默认商品
            defaultProduct.Resource = resources;
            defaultProduct.Price = defaultPrice;

            return defaultProduct;
        }

        private IList<ProductPoolEntity> GetGroupedProductIdsByProductId(int productId)
        {
            using (var db = new YintaiHangzhouContext())
            {
                var extProductPool = db.ProductPools.FirstOrDefault(p => p.ProductId == productId);

                if (extProductPool == null)
                {
                    Log.ErrorFormat("在ProductPool中获取商品失败,productId:{0}", productId);
                    return null;
                }

                var productPoolEnitys = db.ProductPools.Where(p => p.MergedProductCode == extProductPool.MergedProductCode).ToList();
                if (productPoolEnitys.Count == 0)
                {
                    Log.ErrorFormat("在ProductPool中获取商品失败,productId:{0},mergeProductCode:{1}", productId, extProductPool.MergedProductCode);
                    return null;
                }

                return productPoolEnitys;
            }
        }


        public IEnumerable<string> GetAddedMergedProductCode()
        {
            /**
            *   获取已经成功上传产品的IDS
             *  默认一次获取10条数据
            */
            using (var db = new YintaiHangzhouContext())
            {
                var list = db.ProductPools.Where(
                    p =>
                        p.Status == 200
                        && p.ChannelId == ChannelId)
                    .OrderBy(p => p.ProductId)
                   .Skip(0)
                   .Take(10).ToList();

                return list.ConvertAll(p => p.MergedProductCode.ToString(CultureInfo.InvariantCulture));
            }
        }

        public IEnumerable<int> GetProductIdsByMergedProductCode(string code)
        {
            using (var db = new YintaiHangzhouContext())
            {
                var list = db.ProductPools.Where(
                    p =>
                        p.Status == 200
                        && p.MergedProductCode == code
                        && p.IsDefault == true
                        && p.ChannelId == ChannelId)
                    .OrderBy(p => p.ProductId)
                   .Skip(0)
                   .Take(10).ToList();

                return list.ConvertAll(p => p.ProductId);
            }
        }

        /// <summary>
        /// 根据库存id,获取单品列表信息
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        public IEnumerable<ESStock> GetItemsByIdsByProductId(int productId)
        {
            /**
             * 根据ProductId进行聚合的单品
             */
            var productPoolEnitys = GetGroupedProductIdsByProductId(productId);

            if (productPoolEnitys == null)
            {
                Log.ErrorFormat("获取ProductPool列表失败,{0}", productId);
                return null;
            }

            var productIds = productPoolEnitys.Select(p => p.ProductId.ToString(CultureInfo.InvariantCulture)).ToList();

            Log.InfoFormat("获取ESStock,ProductIds:{0}", string.Join(",", productIds.ToArray()));

            return GetDefaultElasticClient().Search<ESStock>(
                body =>
                    body.Filter(q =>
                        q.Terms(p => p.ProductId, productIds))
                        .Skip(0).
                        Size(5000))
                    .Documents;
        }

        #endregion

        #region 属性提取

        public string GetSalePropertyDesc(int id)
        {
            using (var db = new YintaiHangzhouContext())
            {
                return db.ProductPropertyValues.Where(c => c.Id == id).Select(p => p.ValueDesc).FirstOrDefault();
            }
        }

        #endregion

        #region 更新同步商品状态及失败原因

        public void UpdateProductStatus(int productId, ProductPoolStatus status, string errorMessage)
        {
            var productPoolEnitys = GetGroupedProductIdsByProductId(productId);

            foreach (var productPoolEntity in productPoolEnitys)
            {
                UpdateChildProductStatus(productPoolEntity.ProductId, status, errorMessage);
            }
        }

        public void UpdateChildProductStatus(int productId, ProductPoolStatus status, string errorMessage)
        {
            using (var db = new YintaiHangzhouContext())
            {
                var extProductPool = db.ProductPools.FirstOrDefault(p => p.ProductId == productId);
                if (extProductPool != null)
                {
                    extProductPool.Status = (int)status;
                    extProductPool.UpdateDate = DateTime.Now;
                    extProductPool.ErrorMessage = errorMessage;
                }
                db.SaveChanges();
            }
        }

        #endregion

        #region 帮助方法

        /// <summary>
        /// 获取默认的Es搜索Client
        /// </summary>
        /// <returns>ES搜索客户端</returns>
        private IElasticClient GetDefaultElasticClient()
        {
            return SearchLogic.GetClient();
        }

        #endregion
    }
}
