using System.Web.Script.Serialization;
using com.intime.fashion.data.tmall.Models;
using com.intime.o2o.data.exchange.Ims.Request;
using com.intime.o2o.data.exchange.IT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;

namespace com.intime.fashion.data.sync.Tmall.Executor
{
    public class OrderSyncExecutor : OrderExecutorBase
    {
        private static string TMALL_PAYMENT_METHOD_CODE = ConfigurationManager.AppSettings["TMALL_PAYMENT_METHOD_CODE"] ?? "274";
        private static string TMALL_PAYMENT_METHOD_NAME = ConfigurationManager.AppSettings["TMALL_PAYMENT_METHOD_NAME"] ?? "TMMini";
        private IApiClient _imsClient;
        public OrderSyncExecutor(DateTime benchTime, int pageSize, IApiClient client)
            : base(benchTime, pageSize)
        {
            this._imsClient = client;
        }

        public OrderSyncExecutor(IApiClient client)
            : this(DateTime.Now.AddMinutes(-10), 50, client)
        {
        }

        public override void Execute()
        {
            int totalCount = 0;
            int cursor = 0;
            Decimal lastCursor = 0;

            DoQuery(null, orders => totalCount = orders.Count());            
            while (cursor < totalCount)
            {
                List<JDP_TB_TRADE> oneTimeList = null;
                DoQuery(null,
                    orders =>
                        oneTimeList = orders.Where(o => o.tid > lastCursor).OrderBy(o => o.tid).Take(_pageSize).ToList());

                foreach (var trade in oneTimeList)
                {
                    SyncResult result = null;
                    try
                    {
                        result = SyncOne(trade);
                    }
                    catch (Exception ex)
                    {
                        result = SyncResult.ExceptionResult(ex, trade);
                    }
                    ProcessSyncResult(result);
                }

                cursor += _pageSize;
                lastCursor = oneTimeList.Max(o => o.tid);
            }
        }

        /// <summary>
        /// 处理成功后要记录在订单映射表
        /// </summary>
        /// <param name="result"></param>
        private void ProcessSyncResult(SyncResult result)
        {
            using (var db = DbContextHelper.GetJushitaContext())
            {
                if (result.Succeed)
                {
                    using (var trans = new TransactionScope())
                    {
                        if (!db.Set<OrderSync>().Any(x => x.TmallOrderId == result.TmalOrderId))
                        {
                            //天猫订单和ims订单映射关系
                            db.Set<OrderSync>().Add(new OrderSync()
                            {
                                CreateDate = DateTime.Now,
                                TmallOrderId = result.TmalOrderId,
                                ImsOrderNo = result.TargetOrderNo,
                                Type = (int)OrderType.Order,
                                UpdateDate = DateTime.Now,
                                LogisticsSynced = false
                            });

                            // 子订单映射到ims库存，以便物流信息回传使用
                            foreach (var subOrder in result.TmallOrder.orders.order)
                            {
                                db.Set<SubOrder>().Add(new SubOrder()
                                {
                                    CreateDate = DateTime.Now,
                                    IsForceSynced = false,
                                    LogisticsSynced = false,
                                    Store = null,
                                    TmallOrderId = result.TmalOrderId,
                                    TmallSubOrderId = subOrder.oid,
                                    UpdateDate = DateTime.Now,
                                    ImsInventoryId = subOrder.outer_sku_id
                                });
                            }
                            db.SaveChanges();
                            trans.Complete();
                        }
                    }
                }
                else
                {
                    var failedLog = db.Set<OrderSynchErrorLog>()
                        .FirstOrDefault(x => x.TmallOrderId == result.TmalOrderId);
                    if (failedLog != null)
                    {
                        failedLog.FailedCount += 1;
                        db.SaveChanges();
                    }
                    else
                    {
                        failedLog = new OrderSynchErrorLog()
                        {
                            CreateDate = DateTime.Now,
                            FailedCount = 1,
                            Reason = result.Exception != null ? result.Exception.Message : result.FailedReason,
                            Status = 1,
                            TmallOrderId = result.TmalOrderId,
                            Type = (int)OrderType.Order
                        };
                        db.Set<OrderSynchErrorLog>().Add(failedLog);
                        db.SaveChanges();
                    }
                    if (result.Exception != null)
                    {
                        Logger.Error(result.Exception);
                    }
                }
            }
        }

        /// <summary>
        /// 推送迷你银订单
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        private SyncResult SyncOne(JDP_TB_TRADE trade)
        {
            if (string.IsNullOrEmpty(trade.jdp_response))
            {
                throw new ArgumentException("Trade response is null");
            }
            dynamic tradeRsp = JsonConvert.DeserializeObject(trade.jdp_response);
            Logger.Error(trade.jdp_response);
            var tmallOrder = tradeRsp["trade_fullinfo_get_response"]["trade"];
            dynamic imsOrder = new
            {
                sonumber = tmallOrder.tid,
                transno = tmallOrder.alipay_no,
                totalAmount = tmallOrder.total_fee,
                receivedAmount = tmallOrder.payment,
                paymentcode = TMALL_PAYMENT_METHOD_CODE,//todo 支付编码未确定
                paymentname = TMALL_PAYMENT_METHOD_NAME,
                shipingFee = tmallOrder.post_fee,
                memo = tmallOrder.buyer_message,
                paid = true,
                invoice = new
                {
                    sbuject = tmallOrder.invoice_name,
                    detail = tmallOrder.invoice_type,
                    amount = tmallOrder.payment
                },
                contact = new
                {
                    addr = string.Format("{0}{1}{2}", tmallOrder.receiver_state, tmallOrder.receiver_city, tmallOrder.receiver_address),
                    person = tmallOrder.receiver_name,
                    phone = tmallOrder.receiver_mobile ?? tmallOrder.receiver_phone,
                    zip = tmallOrder.receiver_zip,
                },
                payment = new List<dynamic>(),
                products = new List<dynamic>(),
            };

            //todo 支付编码未确定
            imsOrder.payment.Add(new
            {
                code = TMALL_PAYMENT_METHOD_CODE,
                name = TMALL_PAYMENT_METHOD_NAME,
                amount = tmallOrder.payment,
            });


            foreach (var order in tmallOrder.orders.order)
            {
                imsOrder.products.Add(new
                {
                    stockId = order.outer_sku_id,
                    quantity = order.num,
                    itemPrice = order.price,
                    extendPrice = order.price
                });
            }


            var request = new CreateOrderRequest { Data = imsOrder };

            var rsp = _imsClient.Post(request);            
            if (rsp.Data != null)
            {
                return SyncResult.SucceedResult(rsp.Data.orderno.ToString(), Convert.ToInt64(tmallOrder.tid.ToString()), tmallOrder);
            }
            return SyncResult.FailedResult(string.Format("Notify order failed, order tid is ({0}) Error reason is: ({1})", tmallOrder.tid, rsp.Message));
        }

        public override OrderType OrderType
        {
            get { return OrderType.Order; }
        }
    }
}
