using com.intime.fashion.common;
using com.intime.fashion.common.Wxpay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using com.intime.fashion.service;
using Yintai.Hangzhou.WebSupport.Filters;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class OrderController:RestfulController
    {
        private IOrderLogRepository _orderlogRepo;
        private IOrderRepository _orderRepo;
        private IRMARepository _rmaRepo;
        private IRMAItemRepository _rmaitemRepo;
        private IRMALogRepository _rmalogRepo;
        private  IRMA2ExRepository _rmaexRepo;
        private IInventoryRepository _inventoryRepo;

        public OrderController(IOrderRepository orderRepo,IOrderLogRepository orderlogRepo,
            IRMARepository rmaRepo,
            IRMAItemRepository rmaitemRepo,
            IRMALogRepository rmalogRepo,
            IRMA2ExRepository rmaexRepo,
            IInventoryRepository inventoryRepo)
        {
            _orderlogRepo = orderlogRepo;
            _orderRepo = orderRepo;
            _rmaRepo = rmaRepo;
            _rmaitemRepo = rmaitemRepo;
            _rmalogRepo = rmalogRepo;
            _rmaexRepo = rmaexRepo;
            _inventoryRepo = inventoryRepo;
        }
        [RestfulAuthorize]
        public RestfulResult My(MyOrderRequest request, UserModel authUser)
        {
            var dbContext = Context;
            var onGoStatus = new int[]{
                                        (int)OrderStatus.Paid,
                                        (int)OrderStatus.PreparePack,
                                        (int)OrderStatus.Shipped,
                                        (int)OrderStatus.PassConfirmed
                };
            var linq = Context.Set<OrderEntity>().Where(o=>o.CustomerId == authUser.Id);
            switch(request.Type)
            {
                case OrderRequestType.WaitForPay:
                    linq = linq.Where(o => o.Status == (int)OrderStatus.Create);
                    break;
                case OrderRequestType.OnGoing:
                    linq = linq.Where(o => onGoStatus.Any(status => status == o.Status));
                    break;
                case OrderRequestType.Complete:
                    linq = linq.Where(o=>o.Status==(int)OrderStatus.CustomerReceived ||
                                        o.Status==(int)OrderStatus.CustomerRejected ||
                                        o.Status == (int)OrderStatus.Complete);
                    break;
                case OrderRequestType.Void:
                    linq = linq.Where(o=>o.Status == (int)OrderStatus.Void ||
                                          o.Status == (int)OrderStatus.RMAd);
                    break;
            }
            var linq2 = linq.GroupJoin(dbContext.Set<OrderItemEntity>().Join(dbContext.Set<BrandEntity>(), o => o.BrandId, i => i.Id, (o, i) => new { OI=o,B=i})
                                        .GroupJoin(dbContext.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product && r.Type == (int)ResourceType.Image),
                                                     o => new { P = o.OI.ProductId, PC = o.OI.ColorValueId },
                                                     i => new { P = i.SourceId, PC = i.ColorId },
                                                     (o, i) => new { OI = o.OI,B=o.B, R = i.OrderByDescending(r => r.SortOrder).FirstOrDefault() }),
                                        o => o.OrderNo,
                                        i => i.OI.OrderNo,
                                        (o, i) => new { O = o, R =i });

            int totalCount = linq2.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq2 = linq2.OrderByDescending(l => l.O.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq2.ToList().Select(l=>new MyOrderDetailResponse().FromEntity<MyOrderDetailResponse>(l.O,o=>{
                if (l.R == null)
                    return;
                o.Products = l.R.Select(oi => new MyOrderItemDetailResponse().FromEntity<MyOrderItemDetailResponse>(oi.OI, product => {
                    product.ProductResource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(oi.R);
                    product.BrandName = oi.B.Name;
                    product.Brand2Name = oi.B.EnglishName;
                }));
               
            }));
            var response = new PagerInfoResponse<MyOrderDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<MyOrderDetailResponse>>(response) };
        }
        [RestfulAuthorize]
        public ActionResult Detail(MyOrderDetailRequest request, UserModel authUser)
        {
            var dbContext = Context;
            var linq = dbContext.Set<OrderEntity>().Where(o => o.OrderNo == request.OrderNo)
                        .GroupJoin(dbContext.Set<OrderItemEntity>(),
                            o => o.OrderNo,
                            i => i.OrderNo,
                            (o, i) => new { O = o, OI = i })
                        .FirstOrDefault();
           
            if (linq == null)
            {
                return this.RenderError(m => m.Message = "订单号不存在");
            }
            var isDaogou = false;
            if (linq.O.CustomerId != authUser.Id) 
            {
                var associateEntity = dbContext.Set<IMS_AssociateIncomeHistoryEntity>().Where(im => im.SourceType == (int)AssociateOrderType.Product &&
                                    im.SourceNo == request.OrderNo &&
                                    im.AssociateUserId == authUser.Id).FirstOrDefault();
                if (associateEntity==null)
                    return this.RenderError(m => m.Message = "订单号不存在");
                isDaogou = true;
            }
            var result = new MyOrderDetailResponse().FromEntity<MyOrderDetailResponse>(linq.O, o =>
            {

                o.Products =linq.OI.Select(oi=> new MyOrderItemDetailResponse().FromEntity<MyOrderItemDetailResponse>(oi, op =>
                {
                    op.ProductResource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(
                            dbContext.Set<ResourceEntity>()
                                     .Where(resource => resource.SourceType == (int)SourceType.Product 
                                                        && resource.SourceId == oi.ProductId
                                                        && resource.ColorId == oi.ColorValueId
                                                        && resource.Type == (int)ResourceType.Image)
                                     .OrderByDescending(resource => resource.SortOrder).FirstOrDefault());
                 


                }));

                o.RMAs = dbContext.Set<RMAEntity>().Where(r => r.OrderNo == o.OrderNo).OrderByDescending(r => r.CreateDate)
                        .ToList()
                        .Select(r => new MyRMAResponse().FromEntity<MyRMAResponse>(r, ro => {
                            ro.CanVoid = RMARule.CanVoid(ro.Status);
                        }));

                o.Outbounds = dbContext.Set<OutboundEntity>().Where(ob => ob.SourceType == (int)OutboundType.Order && ob.SourceNo == o.OrderNo)
                              .Join(dbContext.Set<ShipViaEntity>(),ob=>ob.ShippingVia,i=>i.Id,(ob,i)=>new {OB=ob,OS=i})
                             .ToList().Select(ob => new MyShipResponse() { 
                                 ShipViaName = ob.OS.Name,
                                  ShipNo = ob.OB.ShippingNo
                             });
                o.IsDaoGou = isDaogou;
                o.IsOwner = linq.O.CustomerId == authUser.Id;
                var storeId = linq.OI.First().StoreId;
                var groupEntity = Context.Set<StoreEntity>().Where(s => s.Id == storeId)
                                  .Join(Context.Set<GroupEntity>(), og => og.Group_Id, i => i.Id, (og, i) => i)
                                  .Join(Context.Set<Group_WeixinKeysEntity>(), ow => ow.Id, i => i.GroupId, (ow, i) => i).FirstOrDefault();
                if (groupEntity !=null)
                    o.LikeRedirectUrl = groupEntity.PaidLikeUrl;
            });
            return this.RenderSuccess<MyOrderDetailResponse>(r => r.Data=result);
        }

        [RestfulAuthorize]
        public ActionResult Void(MyOrderDetailRequest request, UserModel authUser)
        {
            var dbContext = Context;
            var linq = dbContext.Set<OrderEntity>().Where(o => o.OrderNo == request.OrderNo && o.CustomerId == authUser.Id).FirstOrDefault();   
            if (linq == null)
            {
                return this.RenderError(m => m.Message = "订单号不存在");
            }
            var itemsEntity = dbContext.Set<OrderItemEntity>().Where(o => o.OrderNo == request.OrderNo).ToList();
            var currentStatus = linq.Status;
           var voidStatus = new int[]{(int)OrderStatus.Create};
           if (!voidStatus.Any(s=>s==currentStatus))
           {
               return this.RenderError(m => m.Message = "订单状态现在不能取消");
           }
           using (var ts = new TransactionScope())
           {
               linq.Status = (int)OrderStatus.Void;
               linq.UpdateDate = DateTime.Now;
               linq.UpdateUser = authUser.Id;
               _orderRepo.Update(linq);

               foreach (var item in itemsEntity)
               {
                   var inventoryEntity = dbContext.Set<InventoryEntity>().Where(i => i.ProductId == item.ProductId && i.PColorId == item.ColorValueId && i.PSizeId == item.SizeValueId).FirstOrDefault();
                   if (inventoryEntity == null)
                       continue;
                   inventoryEntity.Amount += item.Quantity;
                   _inventoryRepo.Update(inventoryEntity);
               }

               _orderlogRepo.Insert(new OrderLogEntity() {
                 CreateDate = DateTime.Now,
                  CreateUser = authUser.Id,
                   CustomerId = authUser.Id,
                     OrderNo = linq.OrderNo,
                      Type = (int)OrderOpera.CustomerVoid,
                       Operation="用户取消订单。"
               });
               
               bool isSuccess = false;
               if (currentStatus != (int)OrderStatus.Create)
               {
                   //create ex rma request here to refund
                   var products = dbContext.Set<OrderItemEntity>().Where(o => o.OrderNo == linq.OrderNo)
                     .Select(oi => new RMAProductDetailRequest()
                     {
                         ProductId = oi.ProductId,
                         Properties = new ProductPropertyValueRequest()
                         {
                             ColorValueId = oi.ColorValueId,
                             SizeValueId = oi.SizeValueId
                         },
                         Quantity = oi.Quantity
                     });
                   var result = DoRMA(new RMARequest()
                   {
                       OrderNo = linq.OrderNo,
                       RMAReason = ConfigManager.VoidOrderRMAReason,
                       Reason = "取消订单",
                       Products = JsonConvert.SerializeObject(products)
                   }, authUser, false) as RestfulResult;
                   if (result.Data is ExecuteResult<MyRMAResponse>)
                   {
                       isSuccess = true;
                   }
               }
               else
               {
                   isSuccess = true;
               }
               if (isSuccess)
               {
                   ts.Complete();

               }
               else
               {
                   return this.RenderError(r => r.Message = "取消失败");
               }
               
           }
           return Detail(request, authUser);
          
        }

        [RestfulAuthorize]
        public ActionResult RMA(RMARequest request, UserModel authUser)
        {

            return DoRMA(request, authUser, true);
        }
        [RestfulAuthorize]
        public ActionResult RMA_All(RMARequest request, UserModel authUser)
        {

            return DoRMA(request, authUser, false);
        }
        [RestfulAuthorize]
        public ActionResult WxAppPay(WxGetPay4AppTokenRequest request, UserModel authUser) 
        {
            if (string.IsNullOrEmpty(request.OrderNo))
                return this.RenderError(r => r.Message = "订单号必须");
            var orderEntity = Context.Set<OrderEntity>().Where(o => o.OrderNo == request.OrderNo && o.Status == (int)OrderStatus.Create).FirstOrDefault();
            if (null == orderEntity)
                return this.RenderError(r => r.Message = "订单状态不能支付");
            if (string.Compare(orderEntity.PaymentMethodCode, WxPayConfig.PAYMENT_CODE4APP, true) != 0)
                return this.RenderError(r => r.Message = "订单支付方式不正确");

            var token = WxServiceHelper.GetAppPayToken(orderEntity.OrderNo, orderEntity.TotalAmount,Util.ClientIp(Request));

            return this.RenderSuccess<WxGetPay4AppTokenResponse>(r => r.Data = new WxGetPay4AppTokenResponse { 
                 NonceStr = token.noncestr,
                  Package = token.package,
                   PartnerId = token.partnerid,
                    PrepayId = token.prepayid,
                     TimeStamp = token.timestamp,
                      Sign = token.sign
            });
        }

        [RestfulAuthorize]
        public ActionResult WxHtmlPay(WxGetPay4HtmlUrlRequest request, UserModel authUser)
        {
            if (string.IsNullOrEmpty(request.OrderNo))
                return this.RenderError(r => r.Message = "订单号必须");
            if (string.IsNullOrEmpty(request.ClientIp))
                return this.RenderError(r => r.Message = "客户ip必须");
            if (string.IsNullOrEmpty(request.ReturnUrl))
                return this.RenderError(r => r.Message = "回调地址必须");
            var orderEntity = Context.Set<OrderEntity>().Where(o => o.OrderNo == request.OrderNo && o.Status == (int)OrderStatus.Create).FirstOrDefault();
            if (null == orderEntity)
                return this.RenderError(r => r.Message = "订单状态不能支付");
            if (string.Compare(orderEntity.PaymentMethodCode, WxPayConfig.PAYMENT_CODE4HTML, true) != 0)
                return this.RenderError(r => r.Message = "订单支付方式不正确");

            string url = WxServiceHelper.GetHtmlPayUrl(orderEntity.OrderNo, orderEntity.TotalAmount, request.ClientIp,request.ReturnUrl);

            return this.RenderSuccess<WxGetPay4HtmlUrlResponse>(r => r.Data = new WxGetPay4HtmlUrlResponse() { 
                 PayUrl = url
               });
        }

        private ActionResult DoRMA(RMARequest request, UserModel authUser,bool ifNeedValidate)
        {
            var dbContext = Context;
            var orderEntity = dbContext.Set<OrderEntity>().Where(o => o.OrderNo == request.OrderNo && o.CustomerId == authUser.Id).FirstOrDefault();
            if (orderEntity == null)
            {
                return this.RenderError(m => m.Message = "订单号不存在");
            }
          
            if (ifNeedValidate)
            {
                if (orderEntity.Status != (int)OrderStatus.Shipped)
                {
                    return this.RenderError(m => m.Message = "订单状态现在不能申请退货");
                }
                var rmaEntity = dbContext.Set<RMAEntity>().Where(r => r.OrderNo == request.OrderNo
                                 && r.Status != (int)RMAStatus.Reject2Customer 
                                 && r.Status != (int)RMAStatus.Void
                                 && r.Status !=(int)RMAStatus.Reject
                                 && r.Status !=(int)RMAStatus.Complete).FirstOrDefault();
                if (rmaEntity != null)
                {
                    return this.RenderError(m => m.Message = "已经申请了退货单，不能再次申请！");
                }
            }
            request.Reason = string.Format("{0}-{1}", dbContext.Set<RMAReasonEntity>().Find(request.RMAReason).Reason, request.Reason ?? string.Empty);
          
            using (var ts = new TransactionScope())
            {
                decimal rmaAmount = 0;
                var newRma = _rmaRepo.Insert(new RMAEntity()
                {
                    RMANo = OrderRule.CreateRMACode(),
                    Reason = request.Reason,
                    RMAReason = request.RMAReason,
                    Status = (int)RMAStatus.Created,
                    OrderNo = request.OrderNo,
                    CreateDate = DateTime.Now,
                    CreateUser = authUser.Id,
                    RMAAmount = rmaAmount,
                    RMAType = (int)RMAType.FromOnline,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authUser.Id,
                    ContactPhone = request.ContactPhone,
                    UserId = orderEntity.CustomerId
                }); 
                var rmaItems = new List<OrderItemEntity>();
                if (request.Products2 != null && request.Products2.Count()>0)
                {
                    foreach (var rma in request.Products2)
                    {
                        var orderItemEntity = dbContext.Set<OrderItemEntity>().Where(o => o.OrderNo == request.OrderNo && o.ProductId == rma.ProductId && o.ColorValueId == rma.Properties.ColorValueId && o.SizeValueId == rma.Properties.SizeValueId).FirstOrDefault();
                        if (orderItemEntity == null)
                            return this.RenderError(r => r.Message = string.Format("{0} not in order", rma.ProductId));
                        if (orderItemEntity.Quantity < rma.Quantity)
                            return this.RenderError(r => r.Message = string.Format("{0} 超出购买数量", rma.ProductId));
                        orderItemEntity.Quantity = rma.Quantity;
                        rmaItems.Add(orderItemEntity);
                    }
                } else 
                {
                    rmaItems.AddRange(dbContext.Set<OrderItemEntity>().Where(o=>o.OrderNo == request.OrderNo));
                }
                foreach (var orderItemEntity in rmaItems)
                {
                    rmaAmount += orderItemEntity.ItemPrice * orderItemEntity.Quantity;
                    _rmaitemRepo.Insert(new RMAItemEntity()
                    {
                        CreateDate = DateTime.Now,
                        ItemPrice = orderItemEntity.ItemPrice,
                        ExtendPrice = orderItemEntity.ItemPrice * orderItemEntity.Quantity,
                        ProductDesc = orderItemEntity.ProductDesc,
                        ProductId = orderItemEntity.ProductId,
                        ColorValueId = orderItemEntity.ColorValueId,
                        SizeValueId = orderItemEntity.SizeValueId,
                        Quantity = orderItemEntity.Quantity,
                        RMANo = newRma.RMANo,
                        Status = (int)DataStatus.Normal,
                        UnitPrice = orderItemEntity.UnitPrice,
                        SizeValueName = orderItemEntity.SizeValueName,
                        ColorValueName = orderItemEntity.ColorValueName,
                        UpdateDate = DateTime.Now,
                        StoreItem = orderItemEntity.StoreItemNo,
                        StoreDesc = orderItemEntity.StoreItemDesc,
                        BrandId = orderItemEntity.BrandId
                        

                    });
                    
                }
                newRma.RMAAmount = rmaAmount;
                _rmaRepo.Update(newRma);

                _rmalogRepo.Insert(new RMALogEntity
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authUser.Id,
                    RMANo = newRma.RMANo,
                    Operation = "申请线上退货"
                });
                string exRMANo = string.Empty;
                bool isSuccess = true;

                if (isSuccess)
                {
                    
                    ts.Complete();
                    return this.RenderSuccess<MyRMAResponse>(r => r.Data = new MyRMAResponse().FromEntity<MyRMAResponse>(newRma));
                }
                else
                {
                    return this.RenderError(r => r.Message = "创建失败");
                }
            }
        }

        
       
    }
}
