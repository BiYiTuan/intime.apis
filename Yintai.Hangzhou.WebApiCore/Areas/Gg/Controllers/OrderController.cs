using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Transactions;
using System.Web.Mvc;
using com.intime.fashion.service.contract;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using com.intime.fashion.service;
using Yintai.Hangzhou.Model.Order;
using Yintai.Hangzhou.Model.Product;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebApiCore.Areas.Gg.Controllers
{
    public class OrderController : GgController
    {
        private const int GG_CREATE_USERID = -1;
        private IInventoryRepository _inventoryRepo;
        private IOrderService _orderService;
        private IEFRepository<OrderTransactionEntity> _orderTranRepo;
        private IProductRepository _productRepo;
        private IAssociateIncomeService _associateIncomeService;
        private IOrderItemRepository _orderItemRepo;
        private IOrderRepository _orderRepo;
        public OrderController(
            IOrderService orderService,
            IAssociateIncomeService associateIncomeService,
            IEFRepository<OrderTransactionEntity> orderTranRepo,
            IInventoryRepository inventoryRepo,
            IOrderRepository orderRepo,
            IOrderItemRepository orderItemRepo,
            IProductRepository productRepo
            )
        {
            this._inventoryRepo = inventoryRepo;
            this._orderService = orderService;
            this._associateIncomeService = associateIncomeService;
            this._orderTranRepo = orderTranRepo;
            this._orderRepo = orderRepo;
            this._orderItemRepo = orderItemRepo;
            this._productRepo = productRepo;
        }

        [ValidateParameters]
        public ActionResult CreateOrder(dynamic request, string channel)
        {
            int authuid = GetUserId(channel);
            var db = Context;
            string channelOrderNo = request.sonumber;
            if (db.Set<Map4OrderEntity>().Any(x => x.Channel == channel && x.ChannelOrderCode == channelOrderNo))
            {
                return this.RenderError(r => r.Message = string.Format("订单号({0})对应订单已存在！", channelOrderNo));
            }
            bool isPaid = Convert.ToBoolean(request.paid);
            var orderNo = OrderRule.CreateCode(0);

            using (var ts = new TransactionScope())
            {
                var order = new OrderEntity()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authuid,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authuid,
                    BrandId = 0,
                    CustomerId = authuid,
                    InvoiceAmount = request.invoice != null ? request.invoice.amount : null,
                    InvoiceDetail = request.invoice.detail,
                    InvoiceSubject = request.invoice.subject,
                    Memo = request.memo,
                    ShippingAddress = request.contact.addr,
                    ShippingContactPerson = request.contact.person,
                    ShippingContactPhone = request.contact.phone,
                    ShippingZipCode = request.contact.zip,
                    TotalAmount = request.totalAmount,
                    RecAmount = request.receivedAmount,
                    ShippingFee = request.shippingFee,
                    OrderSource = channel,
                    OrderProductType = GetProductType(channel),
                    OrderNo = orderNo,
                    PaymentMethodCode = request.paymentcode,
                    PaymentMethodName = request.paymentname,
                    Status = isPaid ? (int)OrderStatus.Paid : (int)OrderStatus.Complete,
                };

                _orderRepo.Insert(order);

                if (isPaid)
                {
                    if (request.payment == null || request.payment.Count == 0)
                    {
                        return this.RenderError(r => r.Message = "没有支付信息的订单");
                    }

                    foreach (var pay in request.payment)
                    {
                        string payCode = pay.code;
                        var method = db.Set<PaymentMethodEntity>().FirstOrDefault(x => x.Code == payCode);
                        if (method == null)
                        {
                            return this.RenderError(r => r.Message = "不支持的支付方式");
                        }
                        order.PaymentMethodCode = method.Code;
                        order.PaymentMethodName = method.Name;
                        var payment = new OrderTransactionEntity()
                        {
                            Amount = pay.amount,
                            IsSynced =  false,
                            PaymentCode = method.Code,
                            CreateDate = DateTime.Now,
                            CanSync = -1,
                            OrderNo = order.OrderNo,
                            TransNo = request.transno,
                            OrderType = (int)PaidOrderType.Self
                            
                        };
                        _orderTranRepo.Insert(payment);
                    }
                }


                if (request.products == null || request.products.Count == 0)
                {
                    return this.RenderError(r => r.Message = "订单没有商品信息");
                }
                foreach (var item in request.products)
                {
                    var stockId = (int)item.stockId;
                    var orderItems =
                                from stock in db.Set<InventoryEntity>()
                                from p in db.Set<ProductEntity>()
                                from color in db.Set<ProductPropertyEntity>()
                                from size in db.Set<ProductPropertyEntity>()
                                from colorValue in db.Set<ProductPropertyValueEntity>()
                                from sizeValue in db.Set<ProductPropertyValueEntity>()

                                where
                                    stock.Id == stockId &&
                                    p.Id == stock.ProductId &&
                                    colorValue.Id == stock.PColorId &&
                                    color.ProductId == p.Id && color.IsColor.HasValue && color.IsColor.Value &&
                                    sizeValue.Id == stock.PSizeId &&                                    
                                    size.ProductId == p.Id && size.IsSize.HasValue && size.IsSize.Value 
                                    
                                select new
                                {
                                    Inventory = stock,
                                    Product = p,
                                    Color = color,
                                    Size = size,
                                    ColorValue = colorValue,
                                    SizeValue = sizeValue

                                };

                    var orderItem = orderItems.FirstOrDefault();

                    if (orderItem == null)
                    {
                        return this.RenderError(r => r.Message = string.Format("无效的商品商家编码 {0}", stockId));
                    }

                    int itemQuantity = item.quantity;
                    var inventory = orderItem.Inventory;

                    if (inventory.Amount < itemQuantity)
                    {
                        return this.RenderError(r => r.Message = string.Format("商品({0})库存不足", inventory.ProductId));
                    }
                    inventory.Amount -= itemQuantity;
                    inventory.UpdateDate = DateTime.Now;
                    _inventoryRepo.Update(inventory);

                    var product = orderItem.Product;
                    string salesCode = string.Empty;
                    if (product.ProductType == (int)ProductType.FromSelf)
                    {
                        var productSaleCodeEntity =
                            db.Set<ProductCode2StoreCodeEntity>().FirstOrDefault(p => p.ProductId == product.Id && p.Status == (int)DataStatus.Normal);

                        if (productSaleCodeEntity != null)
                        {
                            salesCode = productSaleCodeEntity.StoreProductCode;
                        }
                    }

                    _orderItemRepo.Insert(new OrderItemEntity()
                    {
                        OrderNo = order.OrderNo,
                        ProductId = product.Id,
                        BrandId = product.Brand_Id,
                        StoreId = product.Store_Id,
                        CreateUser = authuid,
                        CreateDate = DateTime.Now,
                        ItemPrice = item.itemPrice,
                        Quantity = itemQuantity,
                        ProductName = product.Name,
                        Status = (int)DataStatus.Normal,
                        UpdateDate = DateTime.Now,
                        UpdateUser = authuid,
                        ExtendPrice = item.extendPrice,
                        ProductDesc =
                            string.Format("{0}:{1},{2}:{3}", orderItem.Color.PropertyDesc,
                                orderItem.ColorValue.ValueDesc, orderItem.Size.PropertyDesc,
                                orderItem.SizeValue.ValueDesc),
                        ColorId = orderItem.Color.Id,
                        ColorValueId = orderItem.ColorValue.Id,
                        SizeId = orderItem.Size.Id,
                        SizeValueId = orderItem.SizeValue.Id,
                        ColorValueName = orderItem.ColorValue.ValueDesc,
                        SizeValueName = orderItem.SizeValue.ValueDesc,
                        StoreItemNo = product.SkuCode,
                        Points = 0,
                        UnitPrice = product.UnitPrice,
                        StoreSalesCode = salesCode,
                        ProductType = (int)ProductType.FromSelf

                    });
                }

                db.Set<Map4OrderEntity>().Add(new Map4OrderEntity
                {
                    ChannelOrderCode = request.sonumber,
                    OrderNo = order.OrderNo,
                    Channel = channel,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    SyncStatus = (int)OrderOpera.FromCustomer,
                });
                db.Set<OrderLogEntity>().Add(new OrderLogEntity()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authuid,
                    CustomerId = authuid,
                    Operation = isPaid ? "创建已支付订单" : "创建订单",
                    OrderNo = order.OrderNo,
                    Type = (int)OrderOpera.FromOperator
                });
                _associateIncomeService.Create(order);
                _associateIncomeService.Froze(orderNo);
                db.SaveChanges();
                ts.Complete();
            }

            return this.RenderSuccess<dynamic>(r => r.Data = new { orderno = orderNo });
        }
   
        private int GetUserId(string channel)
        {
            switch (channel.ToLower())
            {
                case "yintai":
                    return -1;
                case "tmall":
                    return -2;
            }
            return 0;
        }

        [ValidateParameters]
        public ActionResult Create(dynamic request, string channel)
        {
            const int authuid = GG_CREATE_USERID;
            //using (var db = Context)
            //{
            var db = Context;
            string channelOrderNo = request.sonumber;
            if (db.Set<Map4OrderEntity>().Any(x => x.Channel == channel && x.ChannelOrderCode == channelOrderNo))
            {
                return this.RenderError(r => r.Message = string.Format("订单号({0})对应订单已存在！", channelOrderNo));
            }
            bool isPaid = Convert.ToBoolean(request.paid);
            var orderNo = OrderRule.CreateCode(0);
            using (var ts = new TransactionScope())
            {
                var order = new OrderEntity()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authuid,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authuid,
                    BrandId = 0,
                    CustomerId = authuid,
                    InvoiceAmount = request.invoice != null ? request.invoice.amount : null,
                    InvoiceDetail = request.invoice.detail,
                    InvoiceSubject = request.invoice.subject,
                    Memo = request.memo,
                    ShippingAddress = request.contact.addr,
                    ShippingContactPerson = request.contact.person,
                    ShippingContactPhone = request.contact.phone,
                    ShippingZipCode = request.contact.zip,
                    TotalAmount = request.totalAmount,
                    RecAmount = request.receivedAmount,
                    ShippingFee = request.shippingFee,
                    OrderSource = channel,
                    OrderProductType = GetProductType(channel),
                    OrderNo = orderNo,
                    Status = isPaid ? (int)OrderStatus.Paid : (int)OrderStatus.Complete,
                };


                if (isPaid)
                {
                    if (request.payment == null || request.payment.Count == 0)
                    {
                        return this.RenderError(r => r.Message = "没有支付信息的订单");
                    }

                    foreach (var pay in request.payment)
                    {
                        string payCode = pay.code;
                        var method = db.Set<PaymentMethodEntity>().FirstOrDefault(x => x.Code == payCode);
                        if (method == null)
                        {
                            return this.RenderError(r => r.Message = "不支持的支付方式");
                        }
                        order.PaymentMethodCode = method.Code;
                        order.PaymentMethodName = method.Name;
                        var payment = new OrderTransactionEntity()
                        {
                            Amount = pay.amount,
                            PaymentCode = method.Code,
                            CreateDate = DateTime.Now,
                            CanSync = -1,
                            OrderNo = order.OrderNo,
                            TransNo = request.transno, //淘宝订单号??
                        };
                        db.Set<OrderTransactionEntity>().Add(payment);
                    }
                }


                if (request.products == null || request.products.Count == 0)
                {
                    return this.RenderError(r => r.Message = "订单没有商品信息");
                }
                foreach (var item in request.products)
                {

                    //var colorValueId = (int)item.colorValueId;
                    //var sizeValueId = (int)item.sizeValueId;
                    var stockId = (int)item.stockId;
                    var inventory =
                        db.Set<InventoryEntity>().FirstOrDefault(
                            x => x.Id == stockId);

                    int itemQuantity = item.quantity;

                    if (inventory == null)
                    {
                        return this.RenderError(r => r.Message = "订单商品没有库存信息，请排查!");
                    }

                    if (inventory.Amount < itemQuantity)
                    {
                        return this.RenderError(r => r.Message = string.Format("商品({0})库存不足，无法创建订单", inventory.ProductId));
                    }

                    //inventory.Amount -= itemQuantity; //扣减库存
                    //db.Entry(inventory).State = EntityState.Modified;
                    _inventoryRepo.Update(inventory);
                    var product = db.Set<ProductEntity>().FirstOrDefault(x => x.Id == inventory.ProductId);
                    if (product == null)
                    {
                        return
                            this.RenderError(
                                r => r.Message = string.Format("订单包含无效的商品！商品Id = ({0})", inventory.ProductId));
                    }

                    var colorEntity = db.Set<ProductPropertyValueEntity>().FirstOrDefault(ppv => ppv.Id == inventory.PColorId);
                    var sizeEntity = db.Set<ProductPropertyValueEntity>().FirstOrDefault(ppv => ppv.Id == inventory.PSizeId);

                    var colorValueName = colorEntity == null ? string.Empty : colorEntity.ValueDesc;
                    var sizeValueName = sizeEntity == null ? string.Empty : sizeEntity.ValueDesc;
                    db.Set<OrderItemEntity>().Add(new OrderItemEntity()
                    {
                        OrderNo = order.OrderNo,
                        ProductId = product.Id,
                        BrandId = product.Brand_Id,
                        StoreId = product.Store_Id,
                        CreateUser = authuid,
                        CreateDate = DateTime.Now,
                        ItemPrice = item.itemPrice,
                        Quantity = itemQuantity,
                        ProductName = product.Name,
                        Status = (int)DataStatus.Normal,
                        UpdateDate = DateTime.Now,
                        UpdateUser = authuid,
                        ExtendPrice = item.extendPrice,
                        ProductDesc = string.Format("{0}:{1},{2}:{3}", "颜色", colorValueName, "尺码", sizeValueName),
                        ColorId = colorEntity == null ? 0 : colorEntity.PropertyId,
                        ColorValueId = colorEntity == null ? 0 : colorEntity.Id,
                        SizeId = sizeEntity == null ? 0 : sizeEntity.PropertyId,
                        SizeValueId = sizeEntity == null ? 0 : sizeEntity.Id,
                        ColorValueName = colorValueName,
                        SizeValueName = sizeValueName,
                        StoreItemNo = product.SkuCode,
                        Points = 0,
                        UnitPrice = product.UnitPrice
                    });
                }

                db.Set<Map4OrderEntity>().Add(new Map4OrderEntity
                {
                    ChannelOrderCode = request.sonumber,
                    OrderNo = order.OrderNo,
                    Channel = channel,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    SyncStatus = (int)OrderOpera.FromCustomer,
                });
                db.Set<OrderLogEntity>().Add(new OrderLogEntity()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authuid,
                    CustomerId = authuid,
                    Operation = isPaid ? "创建已支付订单" : "创建订单",
                    OrderNo = order.OrderNo,
                    Type = (int)OrderOpera.FromOperator
                });

                db.Set<OrderEntity>().Add(order);

                db.SaveChanges();
                ts.Complete();
                //}

                return this.RenderSuccess<dynamic>(r => r.Data = new { orderno = orderNo });
            }
        }

        private int? GetProductType(string channel)
        {
            return channel.ToLower() == "yintai" ? (int)OrderProductType.SystemProduct : (int)OrderProductType.SelfProduct;
        }

        [ValidateParameters]
        public ActionResult Void(dynamic request, string channel)
        {
            string orderNo = request.orderno;
            //using (var db = Context)
            //{
            var db = Context;
            var order =
                db.Set<OrderEntity>()
                    .FirstOrDefault(
                        x =>
                            x.OrderNo == orderNo);
            if (order == null)
            {
                return this.RenderError(r => r.Message = string.Format("无效的订单号({0})", orderNo));
            }
            if (order.Status > (int)OrderStatus.OrderPrinted)
            {
                return this.RenderError(r => r.Message = "订单已发货，不能取消");
            }
            var map4Order = db.Set<Map4OrderEntity>().FirstOrDefault(m => m.Channel == channel && m.OrderNo == orderNo);
            if (map4Order == null)
            {
                return this.RenderError(r => r.Message = string.Format("该渠道({0})不存在此订单号({1})对应的订单", channel, orderNo));
            }

            var opcSales = db.Set<OPC_SaleEntity>().Where(x => x.OrderNo == orderNo).ToList();
            if (opcSales.Any())
            {
                if (
                    db.Set<OPC_SaleEntity>().Where(x => x.OrderNo == orderNo).Join(db.Set<OPC_SaleOrderNotificationLogEntity>().Where(l => l.Status == 1 || l.Status == 3), s => s.SaleOrderNo, l => l.SaleOrderNo,
                        (o, s) => s).Any())
                {
                    return this.RenderError(r => r.Message = "订单已送收银，不能取消");
                }

            }

            using (var ts = new TransactionScope())
            {
                foreach (var sale in opcSales)
                {
                    sale.Status = (int)OrderStatus.Void;
                    sale.UpdatedDate = DateTime.Now;
                    sale.UpdatedUser = GG_CREATE_USERID;
                    sale.Remark = string.Format("Void by ({0})", channel);
                    sale.RemarkDate = DateTime.Now;
                    db.Entry(sale).State = EntityState.Modified;
                }

                order.Status = (int)OrderStatus.Void;
                order.UpdateDate = DateTime.Now;
                order.UpdateUser = GG_CREATE_USERID;
                map4Order.SyncStatus = (int)OrderOpera.CustomerVoid;
                map4Order.UpdateDate = DateTime.Now;

                Context.Set<OrderLogEntity>().Add(
                    new OrderLogEntity()
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = GG_CREATE_USERID,
                        CustomerId = GG_CREATE_USERID,
                        Operation = "取消订单",
                        OrderNo = orderNo,
                        Type = (int)OrderOpera.SystemVoid
                    });

                db.SaveChanges();
                ts.Complete();
            }
            //}
            return this.RenderSuccess<dynamic>(t => t.IsSuccess = true);
        }

        [ValidateParameters]
        public ActionResult UpdateExpress(dynamic request, string channel)
        {
            string orderNo = request.orderno;
            if (string.IsNullOrEmpty(orderNo))
            {
                return this.RenderError(r => r.Message = "订单号为空");
            }

            var order = Context.Set<OrderEntity>().FirstOrDefault(x => x.OrderNo == orderNo);
            if (order == null)
            {
                return this.RenderError(r => r.Message = string.Format("无效的订单号({0})", orderNo));
            }
            if (!Context.Set<Map4OrderEntity>().Any(x => x.OrderNo == orderNo && x.Channel == channel))
            {
                return this.RenderError(r => r.Message = string.Format("只能修改自己渠道的订单({0})", orderNo));
            }

            if (order.Status > (int)OrderStatus.OrderPrinted)
            {
                return this.RenderError(r => r.Message = string.Format("订单({0})已发货，不能修改发货信息", orderNo));
            }
            order.ShippingAddress = request.contact.addr;
            order.ShippingContactPerson = request.contact.person;
            order.ShippingContactPhone = request.contact.phone;
            order.ShippingZipCode = request.contact.zip;
            order.UpdateDate = DateTime.Now;
            order.UpdateUser = GG_CREATE_USERID;
            Context.SaveChanges();
            return this.RenderSuccess<dynamic>(r => r.IsSuccess = true);
        }

        [ValidateParameters]
        public ActionResult UpdateInvoice(dynamic request, string channel)
        {
            string orderNo = request.orderno;
            if (string.IsNullOrEmpty(orderNo))
            {
                return this.RenderError(r => r.Message = "订单号为空");
            }

            var order = Context.Set<OrderEntity>().FirstOrDefault(x => x.OrderNo == orderNo);
            if (order == null)
            {
                return this.RenderError(r => r.Message = string.Format("无效的订单号({0})", orderNo));
            }
            if (!Context.Set<Map4OrderEntity>().Any(x => x.OrderNo == orderNo && x.Channel == channel))
            {
                return this.RenderError(r => r.Message = string.Format("只能修改自己渠道的订单({0})", orderNo));
            }

            //if (order.Status > (int)OrderStatus.OrderPrinted)
            //{
            //    return this.RenderError(r => r.Message = string.Format("订单({0})已发货，不能修改发货信息", orderNo));
            //}

            if (!order.RecAmount.HasValue)
            {
                return this.RenderError(r => r.Message = "订单未支付，无法确认发票金额!");
            }

            if (order.RecAmount.Value < (decimal)request.amount)
            {
                return this.RenderError(r => r.Message = "发票金额超过了订单金额，不能修改!");
            }
            order.NeedInvoice = true;
            order.InvoiceAmount = request.amount;
            order.InvoiceDetail = request.detail;
            order.InvoiceSubject = request.subject;
            order.UpdateDate = DateTime.Now;
            order.UpdateUser = GG_CREATE_USERID;
            Context.SaveChanges();
            return this.RenderSuccess<dynamic>(r => r.IsSuccess = true);
        }

        public ActionResult UpdateContactInfo(dynamic request, string channel)
        {
            string orderNo = request.orderno;
            if (string.IsNullOrEmpty(orderNo))
            {
                return this.RenderError(r => r.Message = "订单号为空");
            }

            var order = Context.Set<OrderEntity>().FirstOrDefault(x => x.OrderNo == orderNo);
            if (order == null)
            {
                return this.RenderError(r => r.Message = string.Format("无效的订单号({0})", orderNo));
            }
            if (!Context.Set<Map4OrderEntity>().Any(x => x.OrderNo == orderNo && x.Channel == channel))
            {
                return this.RenderError(r => r.Message = string.Format("只能修改自己渠道的订单({0})", orderNo));
            }

            var sales = Context.Set<OPC_SaleEntity>().Where(x => x.OrderNo == orderNo).ToList();

            if (sales.Any(x => x.Status == 30 || x.Status == 35 || x.Status == 40))
            {
                return this.RenderError(r => r.Message = string.Format("订单({0})已发货，无法修改收货人地址", orderNo));
            }

            order.ShippingAddress = request.contact.addr;
            order.ShippingContactPerson = request.contact.person;
            order.ShippingContactPhone = request.contact.phone;
            order.ShippingZipCode = request.contact.zip;
            order.UpdateDate = DateTime.Now;
            order.UpdateUser = GG_CREATE_USERID;
            Context.Entry(order).State = EntityState.Modified;

            Context.SaveChanges();
            return this.RenderSuccess<dynamic>(x => x.IsSuccess = true);
        }

        [ValidateParameters]
        public ActionResult QueryOrderStatus(dynamic request, string channel)
        {
            var orderNoList = new List<string>();
            foreach (var orderNo in request)
            {
                orderNoList.Add(orderNo.orderno.ToString());
            }

            if (orderNoList.Count > 40)
            {
                return this.RenderError(r => r.Message = "超过最大查询订单量，最大量是40");
            }

            var orderList = Context.Set<OrderEntity>()
                .Where(
                    x =>
                        orderNoList.Contains(x.OrderNo) &&
                        Context.Set<Map4OrderEntity>().Any(m => m.Channel == channel && m.OrderNo == x.OrderNo)).ToList();

            var result = new List<dynamic>();

            foreach (var order in orderList)
            {
                result.Add(new
                {
                    orderno = order.OrderNo,
                    status = order.Status,
                    itemStatus = QueryStatus(order),
                    isSplited = IsOrderSplited(order)
                });
            }

            return this.RenderSuccess<dynamic>(r => r.Data = result);

        }

        private bool IsOrderSplited(OrderEntity order)
        {
            return
                Context.Set<OPC_SaleEntity>()
                    .Where(o => o.OrderNo == order.OrderNo)
                    .Join(Context.Set<SectionEntity>(), o => o.SectionId, s => s.Id, (o, s) => s.StoreId)
                    .Distinct()
                    .Count() > 1;
        }

        private IEnumerable<dynamic> QueryStatus(OrderEntity order)
        {
            //if (order.Status != (int)OrderStatus.Shipped)
            //{
            //    return new List<dynamic>();
            //}

            var items = from sale in Context.Set<OPC_SaleEntity>()
                        from sd in Context.Set<OPC_SaleDetailEntity>()
                        from sse in Context.Set<OPC_ShippingSaleEntity>()
                        from oi in Context.Set<OrderItemEntity>()
                        from store in Context.Set<StoreEntity>()
                        from stock in Context.Set<InventoryEntity>()
                        where
                            sale.OrderNo == order.OrderNo && sd.SaleOrderNo == sale.SaleOrderNo && sale.ShippingSaleId == sse.Id &&
                            oi.Id == sd.OrderItemID && store.Id == sse.StoreId && oi.ProductId == stock.ProductId &&
                            oi.ColorValueId == stock.PColorId && oi.SizeValueId == stock.PSizeId
                        select new
                        {
                            productId = oi.ProductId,
                            stockId = stock.Id,
                            express = sse.ShipViaName,
                            expressId = sse.ShipViaId,
                            shippno = sse.ShippingCode,
                            store = store.Name,
                            storeId = store.Id
                        };
            return items;
        }
    }
}
