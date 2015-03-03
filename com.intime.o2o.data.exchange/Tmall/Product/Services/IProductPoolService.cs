using System;
using System.Collections.Generic;
using com.intime.o2o.data.exchange.Tmall.Product.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.o2o.data.exchange.Tmall.Product.Services
{
    /// <summary>
    /// 提取服务
    /// </summary>
    public interface IProductPoolService
    {
        /// <summary>
        /// 获取待处理的聚合产品Id列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> GetPendingProductIds();

        /// <summary>
        /// 根据聚合Code获取商品Id列表
        /// </summary>
        /// <param name="code">聚合的商品列表</param>
        /// <returns></returns>
        IEnumerable<int> GetProductIdsByMergedProductCode(string code);

        /// <summary>
        /// 获取已经成功添加的商品的列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAddedMergedProductCode();

        ESProduct GetProductByProductId(int productId);

        IEnumerable<ESStock> GetItemsByIdsByProductId(int productId);

        /// <summary>
        /// 更新产品更新状态
        /// </summary>
        /// <param name="productId">产品id</param>
        /// <param name="status">更新状态</param>
        /// <param name="errorMessage">错误信息</param>
        void UpdateProductStatus(int productId, ProductPoolStatus status, string errorMessage);
    }
}
