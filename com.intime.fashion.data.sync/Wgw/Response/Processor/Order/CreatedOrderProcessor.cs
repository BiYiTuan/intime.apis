using System;
using com.intime.jobscheduler.Job.Wgw;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Response.Processor.Order
{
    public class CreatedOrderProcessor:OrderProcessor
    {
        protected override void ProcessOrder(dynamic orderInfo, dynamic dealDetail)
        {
            string dealCode = dealDetail.dealCode;
            if (IsOrderExists(dealCode)) //已经加载过的订单更新状态
            {
                return;
            }
            CreateOrder(orderInfo, dealDetail);
        }

        protected override OrderTransactionEntity CreateOrderTransaction(OrderEntity order, dynamic dealDetail)
        {
            return new OrderTransactionEntity()
            {
                OrderNo = order.OrderNo,
                PaymentCode = order.PaymentMethodCode,
                Amount = order.TotalAmount,
                CreateDate = DateTime.Now,
                TransNo = dealDetail.cftPayId, //财付通订单号
                IsSynced = false,
                CanSync = -1,
                OutsiteUId = dealDetail.buyerOpenid,
                OrderType = (int) PaidOrderType.Self
            };
        }

        protected override bool CheckStocks(InventoryEntity inventory, int buyNum)
        {
            return inventory.Amount >= buyNum;
            //未支付订单不扣减库存
        }
    }
}