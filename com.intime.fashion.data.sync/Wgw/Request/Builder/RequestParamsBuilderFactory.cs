using com.intime.fashion.data.sync.Wgw.Request.Item;
using com.intime.fashion.data.sync.Wgw.Request.Order;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public class RequestParamsBuilderFactory
    {
        public static RequestParamsBuilder CreateBuilder(ISyncRequest request)
        {
            if (request is AddItemRequest)
            {
                return new ItemRequestParamsBuilder(request);
            }
            if (request is UpdateItemRequest)
            {
                return new UpdateItemRequestParamsBuilder(request);
            }

            if (request is UpdateItemMultiStockRequest)
            {
                return new UpdateItemMultiStockRequestBuilder(request);
            }

            if (request is QueryOrderListRequest)
            {
                return new QueryOrderRequestBuilder(request);
            }

            if (request is MarkShippingRequest)
            {
                return new MarkShippingRequestBuilder(request);
            }

            if (request is UpdateProductImageRequest)
            {
                return new UpdateProductImageRequestBuilder(request);
            }

            return null;
        }
    }
}
