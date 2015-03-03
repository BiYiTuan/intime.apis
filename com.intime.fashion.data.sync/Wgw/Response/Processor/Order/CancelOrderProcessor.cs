using System;
using System.Linq;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Response.Processor.Order
{
    public class CancelOrderProcessor:IProcessor
    {
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="response"></param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        public bool Process(dynamic response, dynamic otherInfo)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                string dealCode = response.dealCode;
                var order =
                    db.Orders.Join(
                        db.Map4Order.Where(m => m.ChannelOrderCode == dealCode && m.Channel == ConstValue.WGW_CHANNEL_NAME),
                        m => m.OrderNo, o => o.OrderNo, (o, m) => o).FirstOrDefault();

                var mappedOrder = 
                    db.Map4Order.FirstOrDefault(
                        m => m.ChannelOrderCode == dealCode && m.Channel == ConstValue.WGW_CHANNEL_NAME);
                if (order == null || mappedOrder == null)
                {
                    ErrorMessage = string.Format("Order not exists in local db:{0}",dealCode);
                    return false;
                }

                switch ((OrderStatus) order.Status)
                {
                    case OrderStatus.Void:
                        ErrorMessage = "Order status already void.";
                        return false;
                    case OrderStatus.Shipped:
                        ErrorMessage = "Order is on shipping, can't void!";
                        return false;
                    default:
                        try
                        {
                            order.Status = (int) OrderStatus.Void;
                            order.UpdateDate = DateTime.Now;
                            order.UpdateUser = ConstValue.WGW_OPERATOR_USER;
                            mappedOrder.SyncStatus = (int)OrderOpera.CustomerVoid;
                            db.SaveChanges();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = ex.Message;
                            return false;
                        }
                }
            }
        }

        public string ErrorMessage { get; private set; }
    }
}
