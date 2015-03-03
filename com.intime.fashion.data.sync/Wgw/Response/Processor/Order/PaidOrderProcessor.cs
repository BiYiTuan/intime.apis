using System;
using System.Linq;
using System.Transactions;
using com.intime.fashion.data.sync.Wgw.Request.Order;
using com.intime.jobscheduler.Job.Wgw;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Response.Processor.Order
{
    public class PaidOrderProcessor:OrderProcessor
    {
        protected override void ProcessOrder(dynamic orderInfo, dynamic dealDetail)
        {
            string dealCode = dealDetail.dealCode;
            if (IsOrderExists(dealCode)) //已经加载过的订单更新状态
            {
                SyncOrderStatusToPaid(dealDetail);
            }
            else
            {
                this.CreateOrder(orderInfo,dealDetail);
            }
        }
        protected override bool CheckStocks(InventoryEntity inventory, int buyNum)
        {
            if (inventory.Amount < buyNum)
            {
                return false;
            }
            inventory.Amount -= buyNum;//支付状态订单扣减库存
            return true;
        }

        /// <summary>
        /// 更新本地订单状态为支付状态
        /// </summary>
        /// <param name="dealDetail"></param>
        private void SyncOrderStatusToPaid(dynamic dealDetail)
        {
            using (var ts = new TransactionScope())
            {
                using (var db = GetDbContext())
                {
                    string dealCode = dealDetail.dealCode.ToString();
                    var order =
                        db.Set<OrderEntity>()
                            .Join(
                                db.Set<Map4OrderEntity>()
                                    .Where(
                                        m => m.Channel == ConstValue.WGW_CHANNEL_NAME && m.ChannelOrderCode == dealCode),
                                o => o.OrderNo, m => m.OrderNo, (o, m) => o)
                            .FirstOrDefault();

                    if (order == null)
                    {
                        throw new WgwSyncException(string.Format("Order from wgw does'nt exisits dealCode :{0}",
                            dealCode));
                    }
                    var orderStatus = ExtractOrderStatus(dealDetail.dealState);
                    if (orderStatus == order.Status || order.Status!=(int)OrderStatus.Create)
                    {
                        return; //线上订单和线下订单状态一致或线下订单不是已创建状态则返回
                    }

                    foreach (var item in dealDetail.trades)
                    {
                        int stockId = this.GetInventoryId(item);

                        var inventory =
                            db.Inventories.FirstOrDefault(x => x.Id == stockId);
                        if (inventory == null)
                        {
                            throw new WgwSyncException(string.Format("Invalid inventory ID ({0})", stockId));
                        }

                        var product = db.Products.FirstOrDefault(t => t.Id == inventory.ProductId);
                        if (product == null)
                        {
                            throw new WgwSyncException(
                                string.Format("Order from wgw contains invalid product ({1}) ({0})", item.tilte,
                                    dealDetail.dealCode));
                        }

                        int buyNum = int.Parse(item.buyNum.ToString());

                        //检查并扣减库存
                        if (!CheckStocks(inventory, buyNum))
                        {
                            throw new StockInsufficientException(dealDetail.dealCode,OrderStatusConst.STATE_WG_PAY_OK);
                        }
                    }

                    var order2Ex = db.Set<Order2ExEntity>().FirstOrDefault(t => t.OrderNo == order.OrderNo);
                    //已同步给衡和则可以同步支付状态
                    int canSyncpaidStatus = order2Ex == null ? -1 : 0;

                    var orderTransaction = db.Set<OrderTransactionEntity>()
                        .FirstOrDefault(ot => ot.OrderNo == order.OrderNo);
                    if (orderTransaction != null)
                    {
                        orderTransaction.CanSync = canSyncpaidStatus;
                        orderTransaction.TransNo = dealDetail.cftPayId;//财付通订单号
                        orderTransaction.PaymentCode = dealDetail.bankType == 1
                            ? WgwConfigHelper.WGW_WX_PAYMENTCODE
                            : WgwConfigHelper.WGW_CFT_PAYMENTCODE;
                    }
                    else
                    {
                        db.OrderTransactions.Add(CreateOrderTransaction(order,dealDetail));
                    }
                    db.OrderLogs.Add(new OrderLogEntity()
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = ConstValue.WGW_OPERATOR_USER,
                        CustomerId = order.CustomerId,
                        Operation = "支付订单",
                        OrderNo = order.OrderNo,
                        Type = (int)OrderOpera.FromOperator
                    });
                    order.Status = orderStatus;//更新订单状态为已支付
                    db.SaveChanges();
                }
                ts.Complete();
            }
        }
    }
}