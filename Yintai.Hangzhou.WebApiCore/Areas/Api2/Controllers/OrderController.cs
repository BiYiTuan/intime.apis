using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using com.intime.fashion.service;
using Yintai.Hangzhou.WebApiCore;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api2.Controllers
{
    public class OrderController:Rest2Controller
    {
        private IOrderRepository _orderRepo;
        private IOrderLogRepository _orderLogRepo;
        private IOutboundRepository _obRepo;
        private IOutboundItemRepository _obiRepo;
        private IShippViaRepository _shipviaRepo;
        private IEFRepository<ExOrderEntity> _exorderRepo;
        public OrderController(IOrderRepository orderRepo,IOrderLogRepository orderLogRepo
            ,IOutboundRepository obRepo,IOutboundItemRepository obiRepo
            ,IShippViaRepository shipviaRepo
            , IEFRepository<ExOrderEntity> exorderRepo
            )
        {
            _orderRepo = orderRepo;
            _orderLogRepo = orderLogRepo;
            _obRepo = obRepo;
            _obiRepo = obiRepo;
            _shipviaRepo = shipviaRepo;
            _exorderRepo = exorderRepo;
        }
        [HttpPost]
        public ActionResult PrepareShip(OrderPrepareShipRequest request)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var orderEntity = Context.Set<Order2ExEntity>().Where(o => o.ExOrderNo == request.OrderNo)
                            .Join(Context.Set<OrderEntity>(), o => o.OrderNo, i => i.OrderNo, (o, i) => i).FirstOrDefault();
            if (orderEntity==null)
                return this.RenderError(r => r.Message = "订单不存在！");
            if (orderEntity.Status >= (int)OrderStatus.PreparePack)
                return this.RenderSuccess<BaseResponse>(null);
            using (var ts = new TransactionScope())
            {
                orderEntity.Status = (int)OrderStatus.PreparePack;
                orderEntity.UpdateDate = DateTime.Now;
                _orderRepo.Update(orderEntity);

                _orderLogRepo.Insert(new OrderLogEntity() { 
                     CreateDate = DateTime.Now,
                      CreateUser =0,
                       CustomerId = orderEntity.CustomerId,
                        Operation = request.Memo??"准备发货",
                         OrderNo = orderEntity.OrderNo,
                          Type = (int)OrderOpera.PrepareShip
                });

                ts.Complete();
                return this.RenderSuccess<BaseResponse>(null);
            }
        }

        [HttpPost]
        public ActionResult Ship(OrderShipRequest request)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            request.ShipNo = request.ShipNo ?? string.Empty;
           
            if (!request.IsOrderShip)
            {
                //check if it's a pure ex order
                var exOrderEntity = Context.Set<ExOrderEntity>().Where(o => o.ExOrderNo == request.Sales_Sid).FirstOrDefault();
                if (exOrderEntity == null)
                {
                    return this.RenderError(r => r.Message = "订单不存在！");
                }
                if (exOrderEntity.IsShipped.HasValue && exOrderEntity.IsShipped.Value)
                    return this.RenderSuccess<BaseResponse>(null);
                exOrderEntity.IsShipped = true;
                exOrderEntity.ShipDate = DateTime.Now;
                _exorderRepo.Update(exOrderEntity);
                return this.RenderSuccess<BaseResponse>(null);
            }
            var orderEntity = Context.Set<Order2ExEntity>().Where(o => o.ExOrderNo == request.OrderNo)
                           .Join(Context.Set<OrderEntity>(), o => o.OrderNo, i => i.OrderNo, (o, i) => i)
                           .Join(Context.Set<OrderItemEntity>(), o => o.OrderNo, i => i.OrderNo, (o, i) => new { O = o, OI = i });
            if (!request.ForceShip && orderEntity.First().O.Status !=(int)OrderStatus.PreparePack)
                return this.RenderError(r=>r.Message="订单状态不是准备发货");

            var rawOrderEntity = orderEntity.First().O;

             //ensure shipvia available
            var shipviaEntity = Context.Set<ShipViaEntity>().Where(s=>s.Name==request.ShipViaName).FirstOrDefault();
            if (shipviaEntity == null)
            {
                shipviaEntity = _shipviaRepo.Insert(new ShipViaEntity() { 
                     CreateDate = DateTime.Now,
                      IsOnline = false,
                       Name = request.ShipViaName,
                        Status = 1,
                         UpdateDate = DateTime.Now,
                         Url = string.Empty
                });
            }

            bool isAllShipped = true;
            if (!request.ForceShip)
            {
                var haveShipped = Context.Set<OutboundEntity>().Where(ob => ob.SourceType == (int)OutboundType.Order && ob.SourceNo == rawOrderEntity.OrderNo)
                                        .Join(Context.Set<OutboundItemEntity>(), o => o.OutboundNo, i => i.OutboundNo, (o, i) => new { OB = o, OBI = i })
                                        .ToList();
                var haveShippedQuantity = 0;
                if (haveShipped != null && haveShipped.Count() > 0)
                    haveShippedQuantity = haveShipped.Sum(l => l.OBI.Quantity);
                var totalQuantity = orderEntity.Sum(l => l.OI.Quantity);

                var shippingQuantity = request.Products.Sum(l => l.Quantity);

                if ((haveShippedQuantity  + shippingQuantity) < totalQuantity)
                {
                    isAllShipped = false;
                }
            }
            using (var ts = new TransactionScope())
            { 

                //step1: insert outbound log
                //step2: if all shipped, update order status
                var obEntity = _obRepo.Insert(new OutboundEntity() {
                     OutboundNo = OrderRule.CreateShippingCode(request.StoreId.ToString()),
                      SourceNo = rawOrderEntity.OrderNo,
                       SourceType=(int)OutboundType.Order,
                        CreateDate=DateTime.Now,
                         CreateUser = 0,
                          Status =(int)DataStatus.Normal,
                           ShippingVia = shipviaEntity.Id,
                            ShippingNo = request.ShipNo,
                            ShippingAddress = rawOrderEntity.ShippingAddress,
                            UpdateDate = DateTime.Now,
                            UpdateUser = 0
                });
                if (request.ForceShip)
                {
                    foreach (var item in orderEntity)
                    {
                        _obiRepo.Insert(new OutboundItemEntity()
                        {
                            CreateDate = DateTime.Now,
                            ColorId = item.OI.ColorValueId,
                            OutboundNo = obEntity.OutboundNo,
                            ProductId = item.OI.ProductId,
                            Quantity = item.OI.Quantity,
                            SizeId = item.OI.SizeId,
                            UnitPrice = item.OI.UnitPrice??0m,
                            ExtendPrice = item.OI.ExtendPrice,
                            ItemPrice = item.OI.ItemPrice,
                            UpdateDate = DateTime.Now
                        });
                    }
                }
                else
                {
                    foreach (var item in request.Products)
                    {
                        var ppLinq = Context.Set<ProductPropertyEntity>().Join(Context.Set<ProductPropertyValueEntity>(), o => o.Id, i => i.PropertyId, (o, i) => new { PP = o, PPV = i });
                        var linq = Context.Set<ProductMapEntity>().Where(l => l.ChannelPId == item.ProductId)
                                    .Join(ppLinq, o => o.ProductId, i => i.PP.ProductId, (o, i) => new { P = o, PP = i.PP, PPV = i.PPV });
                        _obiRepo.Insert(new OutboundItemEntity()
                        {
                            CreateDate = DateTime.Now,
                            ColorId = linq.Where(l => l.PPV.ChannelValueId == item.Properties.ColorValueId).FirstOrDefault().PPV.Id,
                            OutboundNo = obEntity.OutboundNo,
                            ProductId = linq.FirstOrDefault().P.ProductId,
                            Quantity = item.Quantity,
                            SizeId = linq.Where(l => l.PPV.ChannelValueId == item.Properties.SizeValueId).FirstOrDefault().PPV.Id,
                            UpdateDate = DateTime.Now
                        });
                    }
                }
                if (isAllShipped)
                {
                    rawOrderEntity.Status = (int)OrderStatus.Shipped;
                    rawOrderEntity.UpdateDate = DateTime.Now;
                    _orderRepo.Update(rawOrderEntity);

                    _orderLogRepo.Insert(new OrderLogEntity() { 
                         CreateDate =DateTime.Now,
                          CustomerId = 0,
                           Operation = "已发货",
                            OrderNo = rawOrderEntity.OrderNo,
                             Type = (int)OrderOpera.Shipping
                             
                    });

                }
                ts.Complete();
            }
            return this.RenderSuccess<BaseResponse>(null);

        }

        [HttpPost]
        public ActionResult Void(OrderVoidRequest request)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var orderEntity = Context.Set<Order2ExEntity>().Where(o => o.ExOrderNo == request.OrderNo)
                             .Join(Context.Set<OrderEntity>(), o => o.OrderNo, i => i.OrderNo, (o, i) => i).FirstOrDefault();
            if (orderEntity == null)
                return this.RenderError(r => r.Message = "订单不存在！");
            if (orderEntity.Status == (int)OrderStatus.Void)
                return this.RenderSuccess<BaseResponse>(null);
            using (var ts = new TransactionScope())
            {
                orderEntity.Status = (int)OrderStatus.Void;
                orderEntity.UpdateDate = request.UpdateTime ?? DateTime.Now;
                _orderRepo.Update(orderEntity);

                _orderLogRepo.Insert(new OrderLogEntity() { 
                     CreateDate = request.UpdateTime??DateTime.Now,
                      CreateUser =0,
                      CustomerId = 0,
                       Operation="取消订单",
                        OrderNo = orderEntity.OrderNo,
                         Type = (int)OrderOpera.SystemVoid
                });
                ts.Complete();
            }
            return this.RenderSuccess<BaseResponse>(null);

        }

        [HttpPost]
        public ActionResult CustomerReject(OrderRejectRequest request)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var orderEntity = Context.Set<Order2ExEntity>().Where(o => o.ExOrderNo == request.OrderNo)
                             .Join(Context.Set<OrderEntity>(), o => o.OrderNo, i => i.OrderNo, (o, i) => i).FirstOrDefault();
            if (orderEntity == null)
                return this.RenderError(r => r.Message = "订单不存在！");
            if (orderEntity.Status == (int)OrderStatus.CustomerRejected)
                return this.RenderSuccess<BaseResponse>(null);
            using (var ts = new TransactionScope())
            {
                orderEntity.Status = (int)OrderStatus.CustomerRejected;
                orderEntity.UpdateDate = request.UpdateTime ?? DateTime.Now;
                _orderRepo.Update(orderEntity);

                _orderLogRepo.Insert(new OrderLogEntity()
                {
                    CreateDate = request.UpdateTime ?? DateTime.Now,
                    CreateUser = 0,
                    CustomerId = 0,
                    Operation = "订单拒收",
                    OrderNo = orderEntity.OrderNo,
                    Type = (int)OrderOpera.CustomerReject
                });
                ts.Complete();
            }
            return this.RenderSuccess<BaseResponse>(null);

        }
    }
}
