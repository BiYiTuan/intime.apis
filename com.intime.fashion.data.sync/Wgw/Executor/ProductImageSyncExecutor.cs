using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.fashion.data.sync.Wgw.Request.Builder;
using com.intime.fashion.data.sync.Wgw.Request.Item;
using Common.Logging;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Executor
{
    public class ProductImageSyncExecutor : ExecutorBase
    {
        private int _totalCount;
        private int _succeedCount;
        private int _failedCount;
        public ProductImageSyncExecutor(DateTime benchTime, ILog logger)
            : base(benchTime, logger)
        {

        }

        protected override int SucceedCount
        {
            get { return _succeedCount; }
        }

        protected override int FailedCount
        {
            get { return _failedCount; }
        }

        private void QueryUpdatedResourceIds(Action<IEnumerable<int>> callback)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                var linq =
                    db.Resources.Where(
                        r =>
                            r.UpdatedDate >= BenchTime && r.Type == 1 && r.SourceType == (int) SourceType.Product &&
                            db.Map4Product.Any(
                                m => m.Channel == ConstValue.WGW_CHANNEL_NAME && m.ProductId == r.SourceId))
                        .GroupBy(r => r.SourceId, (key, rs) => key);
                if (callback != null)
                    callback(linq);
            }
        }

        protected override void ExecuteCore(dynamic extraParameter = null)
        {
            QueryUpdatedResourceIds(ids => _totalCount = ids.Count());
            const int pageSize = 20;
            if (_totalCount > 0)
            {
                int lastCursor = 0;
                int cursor = 0;
                while (cursor < _totalCount)
                {
                    List<int> oneTimeList = null;
                    QueryUpdatedResourceIds(images =>
                    {
                        oneTimeList = images.Where(a => a > lastCursor).OrderBy(a => a).Take(pageSize).ToList();
                    });

                    foreach (var id in oneTimeList)
                    {
                        if (UploadImage(id))
                        {
                            _succeedCount ++;
                        }
                        else
                        {
                            _failedCount ++;
                        }
                    }

                    cursor += pageSize;
                    lastCursor = oneTimeList.Max(t => t);
                }
            }
        }

        private bool UploadImage(int productId)
        {
            try
            {
                var product = GetProduct(productId);
                var builder = RequestParamsBuilderFactory.CreateBuilder(new UpdateProductImageRequest());
                var rsp = Client.Execute<dynamic>(builder.BuildParameters(product));
                if (rsp.errorCode == 0)
                {
                    return true;
                }
                Logger.Error(rsp.errorMessage);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return false;
        }

        private ProductEntity GetProduct(int productId)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                return db.Products.FirstOrDefault(p => p.Id == productId);
            }
        }
    }
}
