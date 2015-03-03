using com.intime.fashion.data.sync.Wgw.Request.Builder.Stock;
using com.intime.jobscheduler.Job.Wgw;
using System.Linq;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public class UpdateItemMultiStockRequestBuilder : RequestParamsBuilder
    {
        public UpdateItemMultiStockRequestBuilder(ISyncRequest request) : base(request)
        {
        }

        public override ISyncRequest BuildParameters(object entity)
        {
            var productId = (int)entity;
            return BuilderParamsters(productId);
        }

        private ISyncRequest BuilderParamsters(int productId)
        {
            Request.Put(ParamName.Param_ItemId,GetItemId(productId));
            using (var db = GetDbContext())
            {
                var item = db.Products.FirstOrDefault(p => p.Id == productId);
                if (item == null)
                {
                    throw new WgwSyncException(string.Format("Not exists product! id = {0}",productId));
                }
                var builder = new StockBuilder(Request, item);
                builder.BuildStockInfo("newData", false);
            }
            return Request;
        }

        private string GetItemId(int productId)
        {
            using (var db = GetDbContext())
            {
                var map = db.Map4Product.FirstOrDefault(m => m.ProductId == productId && m.Channel == ConstValue.WGW_CHANNEL_NAME);
                return map == null?string.Empty:map.ChannelProductId;
            }
        }
    }
}
