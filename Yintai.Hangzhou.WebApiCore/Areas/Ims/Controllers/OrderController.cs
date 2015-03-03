using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.WebSupport.Mvc;
using com.intime.fashion.common;
using com.intime.fashion.common.Extension;
using com.intime.fashion.webapi.domain;

using DomainRequest = com.intime.fashion.webapi.domain.Request;
using com.intime.fashion.service;
using System.Transactions;
using Yintai.Hangzhou.Repository.Contract;
namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class OrderController:RestfulController
    {
        private IOrderRepository _orderRepo;
        private IOrderItemRepository _orderItemRepo;
        private IOrderLogRepository _orderLogRepo;
        private OrderService _orderService;
        public OrderController(IOrderRepository orderRepo,
            IOrderItemRepository orderItemRepo,
            IOrderLogRepository orderLogRepo,
            OrderService orderService)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _orderLogRepo = orderLogRepo;
            _orderService = orderService;
        }
        [RestfulAuthorize]
        public ActionResult My(PagerInfoRequest request,int authuid)
        {
            var itemLinq = Context.Set<OrderItemEntity>()
                          .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product && r.Status == (int)DataStatus.Normal && r.Type == (int)ResourceType.Image),
                                        o => new { Product = o.ProductId, Color = o.ColorValueId },
                                        i => new { Product = i.SourceId, Color = i.ColorId },
                                        (o, i) => new { OI = o, R = i.OrderByDescending(ir => ir.SortOrder).FirstOrDefault() });
            var linq = Context.Set<OrderEntity>().Where(o => o.CustomerId == authuid)
                       .GroupJoin(itemLinq, o => o.OrderNo, i => i.OI.OrderNo, (o, i) => new { Order = o,OI=i});

            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.Order.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new 
            {
                order_no = l.Order.OrderNo,
                amount = l.Order.TotalAmount,
                create_date = l.Order.CreateDate,
                status = ((OrderStatus)l.Order.Status).ToFriendlyString(),
                status_i = l.Order.Status,
                shipping_name = l.Order.ShippingContactPerson,
                products = l.OI.ToList().Select(p=>new {
                    id = p.OI.ProductId,
                    name = p.OI.ProductName,
                    productdesc = p.OI.ProductDesc,
                    price = p.OI.ItemPrice,
                    sku_code = p.OI.StoreItemNo,
                    image = p.R==null?string.Empty:p.R.Name.Image320Url()
                })
            });
            var response = new PagerInfoResponse<dynamic>(request.PagerRequest, totalCount)
            {
                Items = result.ToList<dynamic>()
            };

            return this.RenderSuccess<PagerInfoResponse<dynamic>>(c => c.Data = response);
            
        }

        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Fill_Pro(DomainRequest.IMSOrderFillPromotionRequest request, int authuid)
        {
            var orderEntity = Context.Set<OrderEntity>().Where(o => o.OrderNo == request.OrderNo).FirstOrDefault();
            if (orderEntity == null)
                return this.RenderError(r => r.Message = "订单不存在！");
            if (!_orderService.IsAssociateOrder(authuid,orderEntity))
                return this.RenderError(r => r.Message = "无权操作该订单！");
            if (!_orderService.CanChangePro(orderEntity))
                return this.RenderError(r => r.Message = "订单状态不允许修改促销信息！");
            using (var ts = new TransactionScope())
            {
                orderEntity.PromotionDesc = request.PromotionDesc;
                orderEntity.PromotionRules = request.PromotionRules;
                orderEntity.UpdateDate = DateTime.Now;
                orderEntity.UpdateUser = authuid;
                orderEntity.Status = (int)OrderStatus.AgentConfirmed;
                _orderRepo.Update(orderEntity);

                _orderLogRepo.Insert(new OrderLogEntity()
                {
                    CreateDate = DateTime.Now,
                    CreateUser = authuid,
                    CustomerId = orderEntity.CustomerId,
                    Operation = "导购填写销售码",
                    OrderNo = orderEntity.OrderNo,
                    Type = (int)OrderOpera.FromOperator
                });

                foreach (var item in Context.Set<OrderItemEntity>().Where(oi => oi.OrderNo == request.OrderNo
                    && oi.Status == (int)DataStatus.Normal))
                {
                    var matchItem = request.Items.Where(i => i.ProductId == item.ProductId).FirstOrDefault();
                    if (matchItem == null)
                        return this.RenderError(r => r.Message = string.Format("商品还没有填写销售码：{0}",item.ProductName));
                    item.StoreSalesCode = matchItem.Sales_Code;
                    item.UpdateDate = DateTime.Now;
                    item.UpdateUser = authuid;
                    _orderItemRepo.Update(item);
                }
                ts.Complete();
            }
            return this.RenderSuccess<dynamic>(null); 
        }

    }
}
