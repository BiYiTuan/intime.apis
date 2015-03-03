using com.intime.fashion.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using com.intime.fashion.service;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class RMAController:RestfulController
    {
        private IRMARepository _rmaRepo;
        private IRMALogRepository _rmalogRepo;
        public RMAController(IRMARepository rmaRepo,IRMALogRepository rmalogRepo)
        {
            _rmaRepo = rmaRepo;
            _rmalogRepo = rmalogRepo;
        }
        [RestfulAuthorize]
        public ActionResult OrderList(PagerInfoRequest request, UserModel authUser)
        {
            var dbContext = Context;
            var linq = Context.Set<OrderEntity>().Where(o => o.CustomerId == authUser.Id && o.Status == (int)OrderStatus.Shipped);

            var linq2 = linq.GroupJoin(dbContext.Set<OrderItemEntity>().Join(dbContext.Set<BrandEntity>(), o => o.BrandId, i => i.Id, (o, i) => new { OI = o, B = i })
                                        .GroupJoin(dbContext.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product && r.Type == (int)ResourceType.Image),
                                                     o => o.OI.ProductId,
                                                     i => i.SourceId,
                                                     (o, i) => new { OI = o.OI,B=o.B, R = i.OrderByDescending(r => r.SortOrder).FirstOrDefault() }),
                                        o => o.OrderNo,
                                        i => i.OI.OrderNo,
                                        (o, i) => new { O = o, R = i })
                              .GroupJoin(dbContext.Set<RMAEntity>(),o=>o.O.OrderNo,i=>i.OrderNo,(o,i)=>new {O=o.O,R=o.R,RMA=i});

            int totalCount = linq2.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq2 = linq2.OrderByDescending(l => l.O.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq2.ToList().Select(l => new MyOrderDetailResponse().FromEntity<MyOrderDetailResponse>(l.O, o =>
            {
                if (l.R == null)
                    return;
                o.Products = l.R.Select(oi => new MyOrderItemDetailResponse().FromEntity<MyOrderItemDetailResponse>(oi.OI, product =>
                {
                    product.ProductResource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(oi.R);
                    product.BrandName = oi.B.Name;
                    product.Brand2Name = oi.B.EnglishName;
                }));
                o.RMAs = l.RMA.Select(rma => new MyRMAResponse().FromEntity<MyRMAResponse>(rma));
                     

            }));
            var response = new PagerInfoResponse<MyOrderDetailResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };
            return this.RenderSuccess<PagerInfoResponse<MyOrderDetailResponse>>(r => r.Data = response);

        }

        [RestfulAuthorize]
        public ActionResult List(PagerInfoRequest request, UserModel authUser)
        {
            var linq = Context.Set<RMAEntity>().Where(r => r.UserId == authUser.Id)
                       .GroupJoin(Context.Set<RMAItemEntity>()
                                    .GroupJoin(Context.Set<ResourceEntity>().Where(res => res.SourceType == (int)SourceType.Product && res.Type == (int)ResourceType.Image)
                                    , o => new { Product = o.ProductId, Color = o.ColorValueId }
                                    , i => new { Product = i.SourceId, Color = i.ColorId }
                                    , (o, i) => new { R = o, Res = i.OrderByDescending(res2 => res2.SortOrder).FirstOrDefault() })
                                    .GroupJoin(Context.Set<BrandEntity>(), o => o.R.BrandId, i => i.Id, (o, i) => new { R=o.R,Res=o.Res,B=i.FirstOrDefault()})
                       , o => o.RMANo, i => i.R.RMANo, (o, i) => new { R = o, RI = i });
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(l => l.R.CreateDate).Skip(skipCount).Take(request.Pagesize);
            var result = linq.ToList().Select(l => new RMAInfoResponse().FromEntity<RMAInfoResponse>(l.R, o =>
            {
                if (l.RI == null)
                    return;
                o.CanVoid = RMARule.CanVoid(o.Status);
                o.Products = l.RI.ToList().Select(oi => new RMAItemInfoResponse().FromEntity<RMAItemInfoResponse>(oi.R, product =>
                {
                    product.ProductResource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(oi.Res);
                    if (oi.B != null)
                    {
                        product.BrandName = oi.B.Name;
                        product.Brand2Name = oi.B.EnglishName;
                    }

                }));
               
            }));
            var response = new PagerInfoResponse<RMAInfoResponse>(request.PagerRequest, totalCount)
            {
                Items = result.ToList()
            };
            return this.RenderSuccess<PagerInfoResponse<RMAInfoResponse>>(r => r.Data = response);
        }
         [RestfulAuthorize]
        public ActionResult Detail(RMAInfoRequest request, UserModel authUser)
        {
            var rmaEntity = Context.Set<RMAEntity>().Where(r => r.RMANo == request.RMANo).FirstOrDefault();
            if (rmaEntity == null)
                return this.RenderError(r => r.Message = "RMANo 不存在");
            var response = new RMAInfoResponse().FromEntity<RMAInfoResponse>(rmaEntity, r => { 
                var rmaItemsEntity = Context.Set<RMAItemEntity>().Where(ri=>ri.RMANo== request.RMANo)
                                    .GroupJoin(Context.Set<ResourceEntity>().Where(res => res.SourceType == (int)SourceType.Product && res.Type == (int)ResourceType.Image)
                                        , o => new { Product = o.ProductId, Color = o.ColorValueId }
                                        , i => new { Product = i.SourceId, Color = i.ColorId }
                                        , (o, i) => new { R = o, RR = i.OrderByDescending(res2 => res2.SortOrder).FirstOrDefault() });
                                
                r.CanVoid =  RMARule.CanVoid(r.Status);

                r.Products = rmaItemsEntity.ToList().Select(ri=>new RMAItemInfoResponse().FromEntity<RMAItemInfoResponse>(ri.R,ritem=>{
                   ritem.ProductResource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(ri.RR);
                    
                }));
                

            });
            return this.RenderSuccess<RMAInfoResponse>(r=>r.Data = response);
        }
         [RestfulAuthorize]
         private ActionResult Update(RMAUpdateRequest request, UserModel authUser)
         {
             if (!ModelState.IsValid)
             {
                 var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                 return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
             }
             var rmaEntity = Context.Set<RMAEntity>().Where(r => r.RMANo == request.RMANo).FirstOrDefault();
             if (rmaEntity == null)
             {
                 return this.RenderError(r => r.Message = "RMANo不存在");
             }
             if (rmaEntity.Status!=(int)RMAStatus.Created)
             {
                 return this.RenderError(r => r.Message = "RMA状态不正确");
             }
             using(var ts = new TransactionScope())
             {
                 rmaEntity.ContactPhone = request.ContactPhone;
                 rmaEntity.ShipNo = request.ShipViaNo;
                 rmaEntity.ShipviaId = request.ShipVia;
                 rmaEntity.ContactPerson = request.ContactPerson;
                 rmaEntity.UpdateDate = DateTime.Now;
                 rmaEntity.UpdateUser = authUser.Id;
                 rmaEntity.Status = (int)RMAStatus.CustomerConfirmed;
                 _rmaRepo.Update(rmaEntity);

                 _rmalogRepo.Insert(new RMALogEntity(){
                     CreateDate = DateTime.Now,
                      CreateUser = authUser.Id,
                       RMANo = request.RMANo,
                        Operation="用户填写送货信息"
                 });
                 //todo: call erp service to update rma
                 ts.Complete();
             }
  
             return this.RenderSuccess<RMAInfoResponse>(null);
         }

         public ActionResult Void(RMAInfoRequest request, UserModel authUser)
         {
             if (!ModelState.IsValid)
             {
                 var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                 return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
             }
             var rmaEntity = Context.Set<RMAEntity>().Where(r => r.RMANo == request.RMANo).FirstOrDefault();
             if (rmaEntity == null)
             {
                 return this.RenderError(r => r.Message = "RMANo不存在");
             }
           
             if (!RMARule.CanVoid(rmaEntity.Status))
             {
                 return this.RenderError(r => r.Message = "RMA状态不正确");
             }
             using (var ts = new TransactionScope())
             {
              
                 rmaEntity.UpdateDate = DateTime.Now;
                 rmaEntity.UpdateUser = authUser.Id;
                 rmaEntity.Status = (int)RMAStatus.Void;
                 _rmaRepo.Update(rmaEntity);

                 _rmalogRepo.Insert(new RMALogEntity()
                 {
                     CreateDate = DateTime.Now,
                     CreateUser = authUser.Id,
                     RMANo = request.RMANo,
                     Operation = "取消退货单"
                 });
                 // call erp service to void rma
                 var rmaMap = Context.Set<RMA2ExEntity>().Where(r=>r.RMANo == rmaEntity.RMANo).FirstOrDefault();
                 if (rmaMap == null || string.IsNullOrEmpty(rmaMap.ExRMA))
                     ts.Complete();
                 else
                 {
                     var erpRma = new List<dynamic>() { new {
                     Orders_refund_frt_sid = rmaMap.ExRMA,
                     REAL_NAME = authUser.Nickname,
                     USER_SID = "0"

                    }};
                     bool isSuccess = ErpServiceHelper.SendHttpMessage(ConfigManager.ErpBaseUrl, new { func = "Cancel_Refund", jsonRefunds = new { Data = erpRma } }, null
                     , null);
                     if (isSuccess)
                         ts.Complete();
                     else
                         return this.RenderError(r => r.Message = "退货单取消失败！");
                 }
             }

             return this.RenderSuccess<RMAInfoResponse>(null);
         }
    }
}
