using com.intime.o2o.data.exchange.Tmall.Product.Models;
using com.intime.o2o.data.exchange.Tmall.Product.Services;
using com.intime.o2o.data.exchange.Tmall.Product.Services.Support;
using Common.Logging;
using Quartz;

namespace com.intime.jobscheduler.Job.Tmall.Product
{
    /// <summary>
    /// 产品同步Job
    /// </summary>
    [DisallowConcurrentExecution]
    public class ProductPushJob : IJob
    {
        private readonly IProductPoolService _productPoolService = new ProductPoolService();
        private readonly IProductPushService _productPushService = new ProductPushService();
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public void Execute(IJobExecutionContext context)
        {
            /**
             * 1. 获取待处理的商品列表
             * 2.遍历商品列表调用ProductPushService进行Push商品到Tmall
             * 3.更新处理后的状态
             */
            Log.Info("开始同步产品到天猫");

            var productIds = _productPoolService.GetPendingProductIds();

            foreach (var productId in productIds)
            {
                Log.Info(string.Format("开始同步产品id:{0}到天猫", productId));

                // 获取搜索数据
                var productSchema = _productPoolService.GetProductByProductId(productId);

                // 如果搜索没有找到商品进行记录，后续修改状态进行同步添加到天猫
                if (productSchema == null)
                {
                    var errorMsg = string.Format("搜索中没有找到产品信息:productId:{0}", productId);
                    Log.Error(errorMsg);
                    _productPoolService.UpdateProductStatus(productId, ProductPoolStatus.ElasticSearchNotFound, string.Format("搜索中没有找到产品信息:productId:{0}", productId));
                    continue;
                }

                // push 本地商品到tmall
                var result = _productPushService.AddProduct(productSchema, "intime");

                // 更新商品更新状态
                _productPoolService.UpdateProductStatus(productSchema.Id, result.IsError ? ProductPoolStatus.AddProudctError : ProductPoolStatus.AddProudctFinished, result.ErrMsg);

                Log.Info(string.Format("完成同步产品id:{0}到天猫", productSchema.Id));
            }

            Log.Info("结束同步产品到天猫");
        }
    }
}
