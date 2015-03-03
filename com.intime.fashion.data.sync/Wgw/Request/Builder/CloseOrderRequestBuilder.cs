using com.intime.fashion.data.sync.Wgw.Request.Order;
using com.intime.jobscheduler.Job.Wgw;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public class CloseOrderRequestBuilder : RequestParamsBuilder
    {
        public CloseOrderRequestBuilder(ISyncRequest request) : base(request)
        {
        }

        public override ISyncRequest BuildParameters(object entity)
        {
            var order = entity as OrderEntity;
            if (order == null)
            {
                throw new WgwSyncException(string.Format("entity type must be OrderEntity"));
            }

            var dealCode = GetDealCodeByOrderNo(order.OrderNo);
            Request.Put("dealCode",dealCode);
            if (order.Status == (int) OrderStatus.Create)
            {
                //当订单状态dealState=60时:[6：商品缺货无法完成交易 7：买家地址无法送达 8：买家下单信息为假信息 9：买家误拍或是重拍了 10 无法联系上买家 11：买家未按时付款 12：其他原因] 
                Request.Put("dealState",OrderStatusConst.STATE_WG_WAIT_PAY);
                Request.Put("closeReason","11");
            }
            else
            {
                throw new WgwSyncException(string.Format("Unsupported order ({1}) status {0}",order.Status, order.OrderNo));
            }
            return Request;
        }

        private string GetDealCodeByOrderNo(string orderNo)
        {
            using (var db = GetDbContext())
            {
                return
                    db.Map4Order.First(m => m.OrderNo == orderNo && m.Channel == ConstValue.WGW_CHANNEL_NAME).ChannelOrderCode;
            }
        }
    }
}
