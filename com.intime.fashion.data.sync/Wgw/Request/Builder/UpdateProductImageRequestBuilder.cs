using com.intime.fashion.data.sync.Wgw.Request.Builder.Image;
using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public class UpdateProductImageRequestBuilder:RequestParamsBuilder
    {
        public UpdateProductImageRequestBuilder(ISyncRequest request) : base(request)
        {
        }

        public override ISyncRequest BuildParameters(object entity)
        {
            var product = entity as ProductEntity;
            if (product == null)
            {
                throw new ArgumentNullException("entity");
            }
            Request.Put(ParamName.Param_ItemId, GetMappedItemId(product.Id));
            var builder = new ImageBuilder();
            builder.BuildImage(Request,product);
            return Request;
        }

        private string GetMappedItemId(int id)
        {
            using (var db = GetDbContext())
            {
                var map = db.Map4Product.FirstOrDefault(m => m.ProductId == id && m.Channel == ConstValue.WGW_CHANNEL_NAME);
                return map == null ? string.Empty : map.ChannelProductId;
            }
        }
    }
}
