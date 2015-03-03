using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Com.Alipay;
using System.Data.Entity;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using System.Transactions;
using Yintai.Architecture.Common.Data.EF;
using Newtonsoft.Json;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using com.intime.fashion.common.Wxpay;
using Yintai.Hangzhou.Contract.Customer;
using Yintai.Hangzhou.Contract.DTO.Request.Customer;
using com.intime.fashion.service;
using Yintai.Hangzhou.Model;
using com.intime.fashion.common;
using System.Xml;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using com.intime.fashion.common.Weigou;
using System.Web;
using com.intime.fashion.common.Erp2;


namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    [ValidateInput(false)]
    public class PaymentController : Controller
    {
        private IEFRepository<OrderTransactionEntity> _orderTranRepo;
        private IEFRepository<PaymentNotifyLogEntity> _paymentNotifyRepo;
        private IOrderRepository _orderRepo;
        private IOrderLogRepository _orderlogRepo;
        private ICustomerDataService _customerService;
        private IEFRepository<ExOrderEntity> _exorderRepo;

        public PaymentController(IEFRepository<OrderTransactionEntity> ordTranRepo
            , IEFRepository<PaymentNotifyLogEntity> paynotiRepo
            , IOrderRepository orderRepo
            , IOrderLogRepository orderLogRepo,
            ICustomerDataService customerService
            ,IEFRepository<ExOrderEntity> exorderRepo)
        {
            _orderTranRepo = ordTranRepo;
            _paymentNotifyRepo = paynotiRepo;
            _orderRepo = orderRepo;
            _orderlogRepo = orderLogRepo;
            _customerService = customerService;
            _exorderRepo = exorderRepo;
        }

        public ActionResult Notify2()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)
                {
                    string out_trade_no = Request.Form["out_trade_no"];
                    string trade_no = Request.Form["trade_no"];

                    string trade_status = Request.Form["trade_status"];

                    var amount = decimal.Parse(Request.Form["total_fee"]);
                    if (trade_status == "TRADE_FINISHED" ||
                        trade_status == "TRADE_SUCCESS")
                    {
                        var paymentEntity = Context.Set<OrderTransactionEntity>().Where(p => p.OrderNo == out_trade_no
                            && p.PaymentCode == Config.PaymentCode).FirstOrDefault();
                        var orderEntity = Context.Set<OrderEntity>().Where(o => o.OrderNo == out_trade_no).FirstOrDefault();
                        if (paymentEntity == null && orderEntity != null)
                        {
                            OrderTransactionEntity orderTransaction = null;
                            using (var ts = new TransactionScope())
                            {
                                _paymentNotifyRepo.Insert(new PaymentNotifyLogEntity()
                                {
                                    CreateDate = DateTime.Now,
                                    OrderNo = out_trade_no,
                                    PaymentCode = Config.PaymentCode,
                                    PaymentContent = JsonConvert.SerializeObject(sPara)
                                });

                                orderTransaction = _orderTranRepo.Insert(new OrderTransactionEntity()
                                {
                                    Amount = amount,
                                    OrderNo = out_trade_no,
                                    CreateDate = DateTime.Now,
                                    IsSynced = false,
                                    PaymentCode = Config.PaymentCode,
                                    TransNo = trade_no,
                                    OrderType = (int)PaidOrderType.Self
                                });
                                if (orderEntity.Status != (int)OrderStatus.Paid && orderEntity.TotalAmount<=amount)
                                {
                                    orderEntity.Status = (int)OrderStatus.Paid;
                                    orderEntity.UpdateDate = DateTime.Now;
                                    orderEntity.RecAmount = amount;
                                    _orderRepo.Update(orderEntity);

                                    _orderlogRepo.Insert(new OrderLogEntity()
                                    {
                                        CreateDate = DateTime.Now,
                                        CreateUser = 0,
                                        CustomerId = orderEntity.CustomerId,
                                        Operation = "支付订单",
                                        OrderNo = out_trade_no,
                                        Type = (int)OrderOpera.CustomerPay
                                    });
                                }
                                ts.Complete();
                               
                            }
                           
                        }

                    }
                    return Content("success");

                }
                else
                {
                    return Content("fail");
                }
            }
            else
            {
                return Content("无通知参数");
            }
        }

        public ActionResult Notify()
        {
            Dictionary<string, string> sPara = GetRequestPost2();

            if (sPara.Count > 0)
            {
                var aliNotify = new Com.Alipay2.Notify();
                bool verifyResult = aliNotify.VerifyNotify(sPara, Request.Form["sign"]);

                if (verifyResult)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(sPara["notify_data"]);

                    string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                    string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;

                    string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;

                    var amount = decimal.Parse(xmlDoc.SelectSingleNode("/notify/total_fee").InnerText);
                    if (trade_status == "TRADE_FINISHED")
                    {
                        var paymentEntity = Context.Set<OrderTransactionEntity>().Where(p => p.OrderNo == out_trade_no
                            && p.PaymentCode == Config.PaymentCode).FirstOrDefault();
                        var orderEntity = Context.Set<OrderEntity>().Where(o => o.OrderNo == out_trade_no).FirstOrDefault();
                        if (paymentEntity == null && orderEntity != null)
                        {
                            OrderTransactionEntity orderTransaction = null;
                            using (var ts = new TransactionScope())
                            {
                                _paymentNotifyRepo.Insert(new PaymentNotifyLogEntity()
                                {
                                    CreateDate = DateTime.Now,
                                    OrderNo = out_trade_no,
                                    PaymentCode = Config.PaymentCode,
                                    PaymentContent = JsonConvert.SerializeObject(sPara)
                                });

                                 orderTransaction = _orderTranRepo.Insert(new OrderTransactionEntity()
                                {
                                    Amount = amount,
                                    OrderNo = out_trade_no,
                                    CreateDate = DateTime.Now,
                                    IsSynced = false,
                                    PaymentCode = Config.PaymentCode,
                                    TransNo = trade_no,
                                    OrderType = (int)PaidOrderType.Self
                                });
                                if (orderEntity.Status != (int)OrderStatus.Paid && orderEntity.TotalAmount <= amount)
                                {
                                    orderEntity.Status = (int)OrderStatus.Paid;
                                    orderEntity.UpdateDate = DateTime.Now;
                                    orderEntity.RecAmount = amount;
                                    _orderRepo.Update(orderEntity);

                                    _orderlogRepo.Insert(new OrderLogEntity()
                                    {
                                        CreateDate = DateTime.Now,
                                        CreateUser = 0,
                                        CustomerId = orderEntity.CustomerId,
                                        Operation = "支付订单",
                                        OrderNo = out_trade_no,
                                        Type = (int)OrderOpera.CustomerPay
                                    });
                                }
                                ts.Complete();
                               
                            }
                           
                        }

                    }
                    return Content("success");

                }
                else
                {
                    return Content("fail");
                }
            }
            else
            {
                return Content("无通知参数");
            }
        }
        public ActionResult NotifyWx(WxNotifyPostRequest request)
        {
            Dictionary<string, string> sPara = GetQueryStringParams();

            if (sPara.Count > 0)
            {
                var requestSign = sPara["sign"];
                sPara.Remove("sign");
                var notifySigned = Util.NotifySign(sPara);

                if (string.Compare(requestSign, notifySigned, true) != 0)
                    return Content("fail");
                //external order no
                string out_trade_no = sPara["out_trade_no"];
                string trade_no = sPara["transaction_id"];
                int trade_status = int.Parse(sPara["trade_state"]);
                string bank_bill_no = sPara["bank_billno"];
                var amount = decimal.Parse(sPara["total_fee"]) / 100;
                if (trade_status == 0)
                {
                    var orderEntity = Context.Set<OrderEntity>().Join(Context.Set<Order2ExEntity>().Where(oe => oe.ExOrderNo == out_trade_no), o => o.OrderNo, i => i.OrderNo, (o, i) => o).FirstOrDefault();
                    var paymentEntity = Context.Set<OrderTransactionEntity>().Where(p => p.OrderNo == orderEntity.OrderNo 
                                                && p.PaymentCode == WxPayConfig.PaymentCode 
                                                && (!p.OrderType.HasValue || p.OrderType.Value==(int)PaidOrderType.Self)).FirstOrDefault();

                    if (paymentEntity == null && orderEntity != null)
                    {
                        OrderTransactionEntity orderTransaction = null; 
                        using (var ts = new TransactionScope())
                        {
                            _paymentNotifyRepo.Insert(new PaymentNotifyLogEntity()
                            {
                                CreateDate = DateTime.Now,
                                OrderNo = orderEntity.OrderNo,
                                PaymentCode = WxPayConfig.PaymentCode,
                                PaymentContent = JsonConvert.SerializeObject(sPara)
                            });

                            orderTransaction= _orderTranRepo.Insert(new OrderTransactionEntity()
                            {
                                Amount = amount,
                                OrderNo = orderEntity.OrderNo,
                                CreateDate = DateTime.Now,
                                IsSynced = false,
                                PaymentCode = WxPayConfig.PaymentCode,
                                TransNo = trade_no,
                                OutsiteType = (int)OutsiteType.WX,
                                OutsiteUId = request.OpenId,
                                 OrderType =(int)PaidOrderType.Self
                            });
                            if (orderEntity.Status != (int)OrderStatus.Paid && orderEntity.TotalAmount <= amount)
                            {

                                orderEntity.Status = (int)OrderStatus.Paid;
                                orderEntity.UpdateDate = DateTime.Now;
                                orderEntity.RecAmount = amount;
                                _orderRepo.Update(orderEntity);

                                _orderlogRepo.Insert(new OrderLogEntity()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = 0,
                                    CustomerId = orderEntity.CustomerId,
                                    Operation = "支付订单",
                                    OrderNo = orderEntity.OrderNo,
                                    Type = (int)OrderOpera.CustomerPay
                                });
                                

                            }
                            bool isSuccess = false;
                            var orderItemEntity = Context.Set<OrderItemEntity>().Where(o => o.OrderNo == orderEntity.OrderNo).FirstOrDefault();
                            var expDate = string.Format("{0}之前", DateTime.Today.AddDays(1).ToShortDateString());
                            var detailRemark = string.Format("提货码:{0} 专柜:{1}", out_trade_no, orderEntity.ShippingAddress);
                           var targetUrl = new Dictionary<string, string>() {
                                            {"productname",orderItemEntity.ProductName}
                                            , {"quantity",orderItemEntity.Quantity.ToString()}
                                            ,{"expdate",expDate}
                                            ,{"remark",detailRemark}
                                        }.Aggregate(new StringBuilder(), (s, e) => s.AppendFormat("{0}={1}&", e.Key, HttpUtility.UrlEncode(e.Value))
                                                                        , s => s.ToString().TrimEnd('&'));
                            isSuccess = WxServiceHelper.SendMessage(new
                            {
                                touser = request.OpenId,
                                template_id = WxPayConfig.MESSAGE_TEMPLATE_ID,
                                url = string.Format("{0}?{1}",WeigouConfig.MESSAGE_TARGET_URL,targetUrl),
                                data = new
                                {
                                    productType = new { value = "商品名",  color = "#000000" },
                                    name = new { value = orderItemEntity.ProductName, color = "#173177" },
                                    number = new { value = orderItemEntity.Quantity.ToString(), color = "#173177" },
                                    expDate =new { value =  expDate, color = "#173177" },
                                    remark = new { value = detailRemark, color = "#173177" } 
                                }
                            }, null, null);
                            if (isSuccess)
                                ts.Complete();
                            else
                                return Content("fail");
                            ts.Complete();

                        }
                        
                    }

                }
                return Content("success");

            }
            else
            {
                return Content("fail");
            }
        }

        public ActionResult NotifyWxErp(WxNotifyPostRequest request)
        {
            Dictionary<string, string> sPara = GetQueryStringParams();

            if (sPara.Count > 0)
            {
                var requestSign = sPara["sign"];
                sPara.Remove("sign");
                var notifySigned = Util.NotifySign(sPara);

                if (string.Compare(requestSign, notifySigned, true) != 0)
                {
                    Log.Info(string.Format("{0} vs {1}",requestSign,notifySigned));
                    return Content("fail");
                }
                //external order no
                string out_trade_no = sPara["out_trade_no"];
                string trade_no = sPara["transaction_id"];
                int trade_status = int.Parse(sPara["trade_state"]);
                string bank_bill_no = sPara["bank_billno"];
                var amount = decimal.Parse(sPara["total_fee"]) / 100;
                if (trade_status == 0)
                {
              
                    var paymentEntity = Context.Set<OrderTransactionEntity>().Where(p => p.OrderNo == out_trade_no
                        && p.PaymentCode == WxPayConfig.PaymentCode
                        && p.OrderType == (int)PaidOrderType.Erp).FirstOrDefault();

                    if (paymentEntity == null)
                    {
                        OrderTransactionEntity orderTransaction = null;
                        using (var ts = new TransactionScope())
                        {
                            _paymentNotifyRepo.Insert(new PaymentNotifyLogEntity()
                            {
                                CreateDate = DateTime.Now,
                                OrderNo = out_trade_no,
                                PaymentCode = WxPayConfig.PaymentCode,
                                PaymentContent = JsonConvert.SerializeObject(sPara)
                            });

                            orderTransaction= _orderTranRepo.Insert(new OrderTransactionEntity()
                            {
                                Amount = amount,
                                OrderNo = out_trade_no,
                                CreateDate = DateTime.Now,
                                IsSynced = false,
                                PaymentCode = WxPayConfig.PaymentCode,
                                TransNo = trade_no,
                                OutsiteType = (int)OutsiteType.WX,
                                OutsiteUId = request.OpenId,
                                 OrderType = (int)PaidOrderType.Erp
                            });
                            _exorderRepo.Insert(new ExOrderEntity() { 
                                 ExOrderNo = out_trade_no,
                                  Amount = amount,
                                   PaidDate = DateTime.Now,
                                    PaymentCode = WxPayConfig.PaymentCode,
                                     OrderType = (int)PaidOrderType.Erp,
                                      IsShipped = false
                            });
                            /*
                            bool isSuccess = false;
                            var targetUrl = new Dictionary<string, string>() {
                                            {"productname","详见小票"}
                                            , {"quantity","详见小票"}
                                            ,{"expdate","详见小票"}
                                        }.Aggregate(new StringBuilder(), (s, e) => s.AppendFormat("{0}={1}&", e.Key, HttpUtility.UrlEncode(e.Value))
                                                                         , s => s.ToString().TrimEnd('&'));
                            isSuccess = WxServiceHelper.SendMessage(new
                            {
                                touser = request.OpenId,
                                template_id = WxPayConfig.MESSAGE_TEMPLATE_ID,
                                url =string.Format("{0}?{1}",WeigouConfig.MESSAGE_TARGET_URL,targetUrl),
                                data = new
                                {
                                    productType = new { value = "商品名", color = "#000000" },
                                    name = new { value = "详见小票", color = "#173177" },
                                    number = new { value = "详见小票", color = "#173177" },
                                    expDate = new { value = "详见小票", color = "#173177" },
                                }
                            }, null, null);
                             * */
                            ts.Complete();
                           
                           
                        }
                       
                    }

                }
                return Content("success");

            }
            else
            {
                return Content("fail");
            }
        }

        public ActionResult NotifyWxErp2(WxNotifyPostRequest request)
        {
            Dictionary<string, string> sPara = GetQueryStringParams();

            if (sPara.Count > 0)
            {
                var requestSign = sPara["sign"];
                sPara.Remove("sign");
                var notifySigned = Util.NotifySign(sPara);

                if (string.Compare(requestSign, notifySigned, true) != 0)
                    return Content("fail");
                //external order no
                string out_trade_no = sPara["out_trade_no"];
                string trade_no = sPara["transaction_id"];
                int trade_status = int.Parse(sPara["trade_state"]);
                string bank_bill_no = sPara["bank_billno"];
                var amount = decimal.Parse(sPara["total_fee"]) / 100;
                if (trade_status == 0)
                {

                    var paymentEntity = Context.Set<OrderTransactionEntity>().Where(p => p.OrderNo == out_trade_no
                        && p.PaymentCode == WxPayConfig.PaymentCode && p.OrderType == (int)PaidOrderType.Erp2).FirstOrDefault();

                    if (paymentEntity == null)
                    {
                        OrderTransactionEntity orderTransaction = null;
                        using (var ts = new TransactionScope())
                        {
                            _paymentNotifyRepo.Insert(new PaymentNotifyLogEntity()
                            {
                                CreateDate = DateTime.Now,
                                OrderNo = out_trade_no,
                                PaymentCode = WxPayConfig.PaymentCode,
                                PaymentContent = JsonConvert.SerializeObject(sPara)
                            });

                            orderTransaction = _orderTranRepo.Insert(new OrderTransactionEntity()
                            {
                                Amount = amount,
                                OrderNo = out_trade_no,
                                CreateDate = DateTime.Now,
                                IsSynced = false,
                                PaymentCode = WxPayConfig.PaymentCode,
                                TransNo = trade_no,
                                OutsiteType = (int)OutsiteType.WX,
                                OutsiteUId = request.OpenId,
                                OrderType = (int)PaidOrderType.Erp2
                            });
                            _exorderRepo.Insert(new ExOrderEntity()
                            {
                                ExOrderNo = out_trade_no,
                                Amount = amount,
                                PaidDate = DateTime.Now,
                                PaymentCode = WxPayConfig.PaymentCode,
                                OrderType = (int)PaidOrderType.Erp2,
                                IsShipped = false
                            });
                            
                            ts.Complete();


                        }
                        
                    }

                }
                return Content("success");

            }
            else
            {
                return Content("fail");
            }
        }

        public ActionResult NotifyWxApp(WxNotifyPostRequest request)
        {
            Dictionary<string, string> sPara = GetQueryStringParams();

            if (sPara.Count > 0)
            {
                var requestSign = sPara["sign"];
                sPara.Remove("sign");
                var notifySigned = Util.NotifySignApp(sPara);

                if (string.Compare(requestSign, notifySigned, true) != 0)
                    return Content("fail");
                //external order no
                string out_trade_no = sPara["out_trade_no"];
                string trade_no = sPara["transaction_id"];
                int trade_status = int.Parse(sPara["trade_state"]);
                string bank_bill_no = sPara["bank_billno"];
                var amount = decimal.Parse(sPara["total_fee"]) / 100;
                if (trade_status == 0)
                {
                    var orderEntity = Context.Set<OrderEntity>().Where(o=>o.OrderNo==out_trade_no).FirstOrDefault();
                    var paymentEntity = Context.Set<OrderTransactionEntity>().Where(p => p.OrderNo == orderEntity.OrderNo
                                                && p.PaymentCode == WxPayConfig.PAYMENT_CODE4APP
                                                && (!p.OrderType.HasValue || p.OrderType.Value == (int)PaidOrderType.Self)).FirstOrDefault();

                    if (paymentEntity == null && orderEntity != null)
                    {
                        OrderTransactionEntity orderTransaction = null;
                        using (var ts = new TransactionScope())
                        {
                            _paymentNotifyRepo.Insert(new PaymentNotifyLogEntity()
                            {
                                CreateDate = DateTime.Now,
                                OrderNo = orderEntity.OrderNo,
                                PaymentCode = WxPayConfig.PAYMENT_CODE4APP,
                                PaymentContent = JsonConvert.SerializeObject(sPara)
                            });

                            orderTransaction = _orderTranRepo.Insert(new OrderTransactionEntity()
                            {
                                Amount = amount,
                                OrderNo = orderEntity.OrderNo,
                                CreateDate = DateTime.Now,
                                IsSynced = false,
                                PaymentCode = WxPayConfig.PAYMENT_CODE4APP,
                                TransNo = trade_no,
                                OrderType = (int)PaidOrderType.Self
                            });
                            if (orderEntity.Status != (int)OrderStatus.Paid && orderEntity.TotalAmount <= amount)
                            {

                                orderEntity.Status = (int)OrderStatus.Paid;
                                orderEntity.UpdateDate = DateTime.Now;
                                orderEntity.RecAmount = amount;
                                _orderRepo.Update(orderEntity);

                                _orderlogRepo.Insert(new OrderLogEntity()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = 0,
                                    CustomerId = orderEntity.CustomerId,
                                    Operation = "支付订单",
                                    OrderNo = orderEntity.OrderNo,
                                    Type = (int)OrderOpera.CustomerPay
                                });


                            }
                           
                          ts.Complete();
                           

                        }
                       
                    }

                }
                return Content("success");

            }
            else
            {
                return Content("fail");
            }
        }

        public ActionResult NotifyWxHtml(WxNotifyPostRequest request)
        {
            Dictionary<string, string> sPara = GetQueryStringParams();

            if (sPara.Count > 0)
            {
                var requestSign = sPara["sign"];
                sPara.Remove("sign");
                var notifySigned = Util.NotifySignHtml(sPara);

                if (string.Compare(requestSign, notifySigned, true) != 0)
                    return Content("fail");
                //external order no
                string out_trade_no = sPara["out_trade_no"];
                string trade_no = sPara["transaction_id"];
                int trade_status = int.Parse(sPara["trade_state"]);
                string bank_bill_no = sPara["bank_billno"];
                var amount = decimal.Parse(sPara["total_fee"]) / 100;
                if (trade_status == 0)
                {
                    var orderEntity = Context.Set<OrderEntity>().Where(o => o.OrderNo == out_trade_no).FirstOrDefault();
                    var paymentEntity = Context.Set<OrderTransactionEntity>().Where(p => p.OrderNo == orderEntity.OrderNo
                                                && p.PaymentCode == WxPayConfig.PAYMENT_CODE4HTML
                                                && (!p.OrderType.HasValue || p.OrderType.Value == (int)PaidOrderType.Self)).FirstOrDefault();

                    if (paymentEntity == null && orderEntity != null)
                    {
                        OrderTransactionEntity orderTransaction = null;
                        using (var ts = new TransactionScope())
                        {
                            _paymentNotifyRepo.Insert(new PaymentNotifyLogEntity()
                            {
                                CreateDate = DateTime.Now,
                                OrderNo = orderEntity.OrderNo,
                                PaymentCode = WxPayConfig.PAYMENT_CODE4HTML,
                                PaymentContent = JsonConvert.SerializeObject(sPara)
                            });

                            orderTransaction = _orderTranRepo.Insert(new OrderTransactionEntity()
                            {
                                Amount = amount,
                                OrderNo = orderEntity.OrderNo,
                                CreateDate = DateTime.Now,
                                IsSynced = false,
                                PaymentCode = WxPayConfig.PAYMENT_CODE4HTML,
                                TransNo = trade_no,
                                OrderType = (int)PaidOrderType.Self
                            });
                            if (orderEntity.Status != (int)OrderStatus.Paid && orderEntity.TotalAmount <= amount)
                            {

                                orderEntity.Status = (int)OrderStatus.Paid;
                                orderEntity.UpdateDate = DateTime.Now;
                                orderEntity.RecAmount = amount;
                                _orderRepo.Update(orderEntity);

                                _orderlogRepo.Insert(new OrderLogEntity()
                                {
                                    CreateDate = DateTime.Now,
                                    CreateUser = 0,
                                    CustomerId = orderEntity.CustomerId,
                                    Operation = "支付订单",
                                    OrderNo = orderEntity.OrderNo,
                                    Type = (int)OrderOpera.CustomerPay
                                });


                            }

                            ts.Complete();


                        }
                      
                    }

                }
                return Content("success");

            }
            else
            {
                return Content("fail");
            }
        }

        [HttpPost]
        public ActionResult WxPackage(WxPackageGetRequest request)
        {
            if (request == null )
            {
                return new XmlResult(composePackageError(r => r.RetErrMsg = "请求数据异常！"));
            }
            if (string.Compare(request.ValidSign, request.AppSignature, false) != 0)
            {
                Log.Debug(JsonConvert.SerializeObject(request));
                Log.Debug(request.ValidSign);
                return new XmlResult(composePackageError(r => r.RetErrMsg = "请求没有授权！"));
            }
            var productId = request.ProductId;
            var productIds = productId.Split('-');
            if (productIds.Length < 2)
                return new XmlResult(composePackageError(r => r.RetErrMsg = "商品信息有误！"));
            var packageType = int.Parse(productIds[0]);
            switch (packageType)
            { 
                case (int)WxPackageType.Order:
                    return doOrderPackage(productIds[1], request);
  
                case (int)WxPackageType.ErpOrder:
                     return doErpOrderPackage(productIds[1],request);
                case (int)WxPackageType.Erp2Order:
                     return doErp2OrderPackage(productIds[1], request);
                default:
                    return new XmlResult(composePackageError(r => r.RetErrMsg = "订单类型有误！"));
            }
          
                
        }

        private ActionResult doOrderPackage(string orderNo, WxPackageGetRequest request)
        {
            var orderEntity = Context.Set<OrderEntity>().Where(o => o.OrderNo == orderNo)
                            .Join(Context.Set<Order2ExEntity>(), o => o.OrderNo, i => i.OrderNo, (o, i) => new { O = o, OE = i })
                            .GroupJoin(Context.Set<OrderItemEntity>(), o => o.O.OrderNo, i => i.OrderNo, (o, i) => new { O = o.O, OE = o.OE, OI = i }).FirstOrDefault();

            if (orderEntity == null)
                return new XmlResult(composePackageError(r => r.RetErrMsg = "订单不存在！"));
            if (orderEntity.O.Status != (int)OrderStatus.Create)
                return new XmlResult(composePackageError(r => r.RetErrMsg = "订单状态无法支付！"));
            var orderItemEntity = orderEntity.OI.First();
            return new XmlResult(composePackageSuccess(r => r.Package = new WxPackage()
            {
                Body = orderItemEntity.ProductName,
                Attach = orderItemEntity.ProductDesc,
                OutTradeNo = orderEntity.OE.ExOrderNo,
                TotalFee = Util.Feng4Decimal(orderEntity.O.TotalAmount),
                TransportFee = Util.Feng4Decimal(orderEntity.O.ShippingFee ?? 0m),
                SPBill_Create_IP = clientIP(),
                Time_Start = orderEntity.O.CreateDate.ToString("yyyyMMddHHmmss"),
                Time_End = orderEntity.O.CreateDate.AddHours(0.5).ToString("yyyyMMddHHmmss")

            }));
        }
        private ActionResult doErpOrderPackage(string orderNo, WxPackageGetRequest request)
        {
           if (string.IsNullOrEmpty(orderNo))
               return new XmlResult(composePackageError(r =>
               {
                   r.RetErrMsg = "订单不存在！";
                   r.TimeStamp = request.TimeStamp + 1;
               }));
           dynamic erpOrder = null;
           bool isSuccess = ErpServiceHelper.SendHttpMessage(ConfigManager.ErpBaseUrl
                            , new { func = "GetSalesInfo", sales_sid = orderNo }
                            ,r=>erpOrder = r.Result
                            , null);
           if (!isSuccess || erpOrder.productDetail == null)
               return new XmlResult(composePackageError(r =>
               {
                   r.RetErrMsg = "订单无法获取！";
                   r.TimeStamp = request.TimeStamp + 1;
               }));
            decimal totalAmount = erpOrder.totalamount;
            if (totalAmount <=0)
                return new XmlResult(composePackageError(r =>
                {
                    r.RetErrMsg = "订单金额不正确！";
                    r.TimeStamp = request.TimeStamp + 1;
                }));
            return new XmlResult(composePackageSuccess(r =>
            {
                r.Package = new WxPackage()
                    {
                        Body = "银泰商业微信支付",
                        OutTradeNo = orderNo,
                        TotalFee = Util.Feng4Decimal(totalAmount),
                        TransportFee = 0,
                        SPBill_Create_IP = clientIP(),
                        NotifyUrl = WxPayConfig.NOTIFY_ERP_URL
                    };
                r.TimeStamp = request.TimeStamp + 1;
            }));
        }

       private ActionResult doErp2OrderPackage(string orderNo, WxPackageGetRequest request)
        {
            if (string.IsNullOrEmpty(orderNo))
                return new XmlResult(composePackageError(r =>
                {
                    r.RetErrMsg = "订单不存在！";
                    r.TimeStamp = request.TimeStamp + 1;
                }));
            dynamic erpOrder = null;
            bool isSuccess = Erp2ServiceHelper.SendHttpMessage(Erp2Config.PACKAGE_URL
                             , new { saleno = orderNo }
                             , r => erpOrder = r
                             , null);
            if (!isSuccess)
                return new XmlResult(composePackageError(r =>
                {
                    r.RetErrMsg = "订单无法获取！";
                    r.TimeStamp = request.TimeStamp + 1;
                }));
            decimal totalAmount = erpOrder.Data;
            if (totalAmount <= 0)
                return new XmlResult(composePackageError(r =>
                {
                    r.RetErrMsg = "订单金额不正确！";
                    r.TimeStamp = request.TimeStamp + 1;
                }));
            return new XmlResult(composePackageSuccess(r =>
            {
                r.Package = new WxPackage()
                {
                    Body = "银泰商业微信支付",
                    OutTradeNo = orderNo,
                    TotalFee = Util.Feng4Decimal(totalAmount),
                    TransportFee = 0,
                    SPBill_Create_IP = clientIP(),
                    NotifyUrl = WxPayConfig.NOTIFY_ERP2_URL
                };
                r.TimeStamp = request.TimeStamp + 1;
            }));
        }
        private WxPackageGetResponse composePackageError(Action<WxPackageGetResponse> more)
        {
            var response = new WxPackageGetResponse()
                {
                    AppId = WxPayConfig.APP_ID,
                    TimeStamp = DateTime.Now.TicksOfWx(),
                    NonceStr = Util.Nonce(),
                    RetCode = (int)WxPayRetCode.RequestError,
                    RetErrMsg = "信息有误！",
                    SignMethod = WxPaySignMethod.SHA1,
                    Package = null
                };
            if (more != null)
                more(response);
            return response;
        }
        private WxPackageGetResponse composePackageSuccess(Action<WxPackageGetResponse> more)
        {
            var response = new WxPackageGetResponse()
            {
                AppId = WxPayConfig.APP_ID,
                TimeStamp = DateTime.Now.TicksOfWx(),
                NonceStr = Util.Nonce(),
                RetCode = (int)WxPayRetCode.Success,
                RetErrMsg = "ok",
                SignMethod = WxPaySignMethod.SHA1,
                Package = null
            };
            if (more != null)
                more(response);
            return response;
        }
        private string clientIP()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (ipAddress == null || ipAddress.ToLower() == "unknown")
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];

            return ipAddress;
        }
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            var unValidRequest = Request.Unvalidated;
            coll = unValidRequest.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], unValidRequest.Form[requestItem[i]]);
            }

            return sArray;
        }
        private Dictionary<string, string> GetRequestPost2()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            var unValidRequest = Request.Unvalidated;
            coll = unValidRequest.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], unValidRequest.Form[requestItem[i]]);
            }

            return sArray;
        }
        private Dictionary<string, string> GetQueryStringParams()
        {
            var requestParams = new Dictionary<string, string>();
            foreach (var key in Request.QueryString.AllKeys)
            {
                if (string.IsNullOrEmpty(Request.QueryString[key]))
                    continue;
                requestParams.Add(key.ToLower(), Request.QueryString[key]);
            }
            return requestParams;
        }
        private DbContext Context
        {
            get
            {
                return ServiceLocator.Current.Resolve<DbContext>();
            }
        }
        private ILog Log { get {
            return ServiceLocator.Current.Resolve<ILog>();
        } }
    }
}
