using System;
using System.Collections.Generic;
using com.intime.o2o.data.exchange.Tmall.Product.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.o2o.data.exchange.Tmall.Product.Services
{
    /// <summary>
    /// Tmall产品的推送服务
    /// </summary>
    public interface IProductPushService
    {
        /// <summary>
        /// 天猫根据规则发布产品
        /// </summary>
        /// <param name="product">产品信息</param>
        /// <param name="consumerKey">消费者Key</param>
        /// <returns>产品处理结果（添加后的产品 Id）</returns>
        ResultInfo<long> AddProduct(ESProduct product, string consumerKey);

        /// <summary>
        /// 更具天猫规则更新产品
        /// </summary>
        /// <param name="product">产品信息</param>
        /// <param name="consumerKey">消费者Key</param>
        /// <returns>更新结构</returns>
        [Obsolete("目前先不考虑更新产品，逻辑已经ok")]
        ResultInfo<bool> UpdateProduct(ESProduct product, string consumerKey);

        /// <summary>
        /// 天猫TopSchema发布商品
        /// </summary>
        /// <param name="item">商品信息</param>
        ///  /// <param name="product">产品信息</param>
        /// <param name="consumerKey">消费者Key</param>
        /// <returns>添加商品结果</returns>
        ResultInfo<long> AddItem(IEnumerable<ESStock> item, ESProduct product, string consumerKey);

        /// <summary>
        /// 天猫TopSchema更新商品
        /// </summary>
        /// <param name="items">商品信息</param>
        /// <param name="product">产品信息</param>
        /// <param name="consumerKey">消费者Key</param>
        /// <returns>添加商品结果</returns>
        ResultInfo<bool> UpdateItem(IEnumerable<ESStock> items, ESProduct product, string consumerKey);
    }
}
