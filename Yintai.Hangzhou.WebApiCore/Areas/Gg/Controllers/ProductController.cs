using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using com.intime.fashion.service.search;
using Nest;
using Yintai.Hangzhou.WebApiCore.Areas.Gg.ViewModels;

namespace Yintai.Hangzhou.WebApiCore.Areas.Gg.Controllers
{
    public class ProductController : GgController
    {
        private readonly IElasticClient _elasticClient;

        public ProductController()
        {
            _elasticClient = GetDefaultElasticClient();
        }

        /// <summary>
        /// 根据最后更新时间获取商品
        /// </summary>
        /// <param name="channel">接口调用方标识</param>
        ///  <param name="request">获取商品请求参数</param>
        /// <returns>商品列表信息</returns>
        /// <example>
        /// {
        ///     page_index:1,
        ///     page_size:10,
        ///     last_update:yyyy-MM-dd'T'HH:mm:ss
        /// }
        /// </example>
        [ValidateParameters]
        public ActionResult Search(dynamic request, string channel)
        {
            int pageIndex = request.page_index;
            int pageSize = request.page_size;

            pageSize = Math.Min(pageSize, 100);

            string lastUpdate = request.last_update;

            // ===========================================================================
            //  根据最后更新时间获取商品信息
            // ===========================================================================

            var result = _elasticClient.Search<ESProducts>(body =>
                body.From(Skip(pageIndex, pageSize))
                    .Size(pageSize)
                    .SortAscending(p => p.UpdatedDate)
                    .Query(q =>
                        q.Term(p => p.IsSystem, true) &
                        q.Term(p => p.Status, 1) &
                        q.Range(p => p.GreaterOrEquals(lastUpdate).OnField(f => f.UpdatedDate)
                    )
                ));

            // ===========================================================================
            //  获取商品总数
            // ===========================================================================

            var total = result.Hits == null ? 0 : result.Hits.Total;

            if (total == 0)
            {
                return this.RenderSuccess<dynamic>(r => r.Data = new PagedListViewModel<ESProducts>()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Total = total,
                    Data = new List<ESProducts>()
                });
            }

            // ===========================================================================
            // 获取商品id列表
            // ===========================================================================
            var productIds = result.Documents.Select(s => s.Id.ToString(CultureInfo.InvariantCulture));

            // ===========================================================================
            // 根据Id列表查询单品信息
            // ===========================================================================
            var productItems = _elasticClient.Search<ESStocks>(
               body =>
                   body.Filter(
                   q => q.Terms(p => p.ProductId, productIds)
           ).Skip(0).Size(5000));

            // ===========================================================================
            // 针对ProductId进行分组Map
            // ===========================================================================
            var maped = Map2ProductId(productItems.Documents);

            // ===========================================================================
            // 遍历设置商品的Items
            // ===========================================================================
            foreach (var product in result.Documents)
            {
                product.Items = maped.ContainsKey(product.Id) ? maped[product.Id] : new List<ESStocks>();
            }

            return this.RenderSuccess<dynamic>(r => r.Data = new PagedListViewModel<ESProducts>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Data = result.Documents
            });
        }

        /// <summary>
        /// 根据最后更新时间获取库存价格信息
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="channel">渠道名称</param>
        /// <returns>库存列表</returns>
        /// <example>
        /// {
        ///     page_index:1,
        ///     page_size:10,
        ///     last_update:yyyy-MM-dd'T'HH:mm:ss
        /// }
        /// </example>
        [ValidateParameters]
        public ActionResult Stock(dynamic request, string channel)
        {
            int pageIndex = request.page_index;
            int pageSize = request.page_size;

            pageSize = Math.Min(pageSize, 100);

            string lastUpdate = request.last_update;

            var stocks = _elasticClient.Search<ESStocks>(body =>
                 body.From(Skip(pageIndex, pageSize))
                    .Size(pageSize)
                    .SortAscending(p => p.UpdateDate)
                    .Query(q => q.Range(p => p.GreaterOrEquals(lastUpdate)
                    .OnField(f => f.UpdateDate))
                ));

            var total = stocks.Hits == null ? 0 : stocks.Hits.Total;

            return this.RenderSuccess<dynamic>(r => r.Data = new PagedListViewModel<ESStocks>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total,
                Data = stocks.Documents
            });

        }

        /// <summary>
        /// 获取默认的Es搜索Client
        /// </summary>
        /// <returns>ES搜索客户端</returns>
        private IElasticClient GetDefaultElasticClient()
        {
            return SearchLogic.GetClient();
        }

        private int Skip(int pageIndex, int pageSize)
        {
            pageIndex = Math.Max(pageIndex - 1, 0);
            return pageIndex * pageSize;
        }

        private IDictionary<int, IList<ESStocks>> Map2ProductId(IEnumerable<ESStocks> docs)
        {
            var result = new Dictionary<int, IList<ESStocks>>();

            if (docs == null)
            {
                return result;
            }

            foreach (var doc in docs)
            {
                if (!result.ContainsKey(doc.ProductId))
                {
                    result.Add(doc.ProductId, new List<ESStocks>() { doc });
                }
                else
                {
                    result[doc.ProductId].Add(doc);
                }
            }

            return result;
        }
    }
}
