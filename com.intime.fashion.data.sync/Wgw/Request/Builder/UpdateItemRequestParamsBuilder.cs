using com.intime.fashion.data.sync.Wgw.Request.Builder.Image;
using com.intime.fashion.data.sync.Wgw.Request.Builder.Stock;
using com.intime.jobscheduler.Job.Wgw;
using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public class UpdateItemRequestParamsBuilder:ItemRequestParamsBuilder
    {
        public UpdateItemRequestParamsBuilder(ISyncRequest request) : base(request)
        {
        }

        protected override void Build(ProductEntity item)
        {
            if (item == null) throw new ArgumentNullException("item");
            var map4Product = GetMap4Product(item.Id);
            if (map4Product == null)
            {
                throw new WgwSyncException(string.Format("Unmapped product:{0}", item.Id));
            }
            
            Request.Put(ParamName.Param_ItemId, map4Product.ChannelProductId);
            if (!map4Product.IsImageUpload.HasValue || map4Product.IsImageUpload.Value != 1)
            {
                new ImageBuilder().BuildImage(Request, item);
            }
            new StockBuilder(Request, item).BuildStockInfo("stockstr");
        }

        private Map4ProductEntity GetMap4Product(int productId)
        {
            using (var db = GetDbContext())
            {
                return db.Map4Product.FirstOrDefault(m => m.ProductId == productId && m.Channel == ConstValue.WGW_CHANNEL_NAME);
            }
        }
    }
}
