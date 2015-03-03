using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api2.Controllers
{
   public class RMAController:Rest2Controller
    {
       private IRMARepository _rmaRepo;
       private IRMALogRepository _rmalogRepo;
       public RMAController(IRMARepository rmaRepo,IRMALogRepository rmalogRepo)
       {
           _rmaRepo = rmaRepo;
           _rmalogRepo = rmalogRepo;
       }
       [HttpPost]
       public ActionResult Confirm(RMAConfirmRequest request)
       {
           if (!ModelState.IsValid)
           {
               var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
               return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
           }
           var rmaEntity = Context.Set<RMA2ExEntity>().Where(r => r.ExRMA == request.RMANo)
                            .Join(Context.Set<RMAEntity>(), o => o.RMANo, i => i.RMANo, (o, i) => i).FirstOrDefault();
           if (rmaEntity == null)
               return this.RenderError(r => r.Message = "RMA 不存在");
           var status = (int)RMAStatus.PassConfirmed;
           var operation = "审核通过！";
           if (!(request.IsPass))
           {
               status = (int)RMAStatus.Reject;
               operation = "审核不通过！";
           }
           using (var ts = new TransactionScope())
           {

               rmaEntity.Status = status;
               rmaEntity.UpdateDate = DateTime.Now;
               rmaEntity.MailAddress = request.MailAddress;
               if (!(request.IsPass))
                   rmaEntity.RejectReason = request.Memo;
               _rmaRepo.Update(rmaEntity);

               _rmalogRepo.Insert(new RMALogEntity() { 
                 CreateDate = request.UpdateTime,
                  CreateUser = 0,
                   Operation = operation,
                    RMANo = rmaEntity.RMANo
               });
               ts.Complete();
           }
           return this.RenderSuccess<dynamic>(r => r.Data = new { mailaddress = request.MailAddress });
       }

       [HttpPost]
       public ActionResult Received(RMAReceivedRequest request)
       {
           if (!ModelState.IsValid)
           {
               var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
               return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
           }
           var rmaEntity = Context.Set<RMA2ExEntity>().Where(r => r.ExRMA == request.RMANo)
                            .Join(Context.Set<RMAEntity>(), o => o.RMANo, i => i.RMANo, (o, i) => i).FirstOrDefault();
           if (rmaEntity == null)
               return this.RenderError(r => r.Message = "RMA 不存在");
           using (var ts = new TransactionScope())
           {
               rmaEntity.Status = (int)RMAStatus.PackageReceived;
               rmaEntity.UpdateDate = DateTime.Now;
               _rmaRepo.Update(rmaEntity);

               _rmalogRepo.Insert(new RMALogEntity()
               {
                   CreateDate = request.UpdateTime,
                   CreateUser = 0,
                   Operation = "收到包裹！",
                   RMANo = rmaEntity.RMANo
               });
               ts.Complete();
           }
           return this.RenderSuccess<BaseResponse>(null);
       }

       [HttpPost]
       public ActionResult Complete(RMACompleteRequest request)
       {
           if (!ModelState.IsValid)
           {
               var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
               return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
           }
           var rmaEntity = Context.Set<RMA2ExEntity>().Where(r => r.ExRMA == request.RMANo)
                            .Join(Context.Set<RMAEntity>(), o => o.RMANo, i => i.RMANo, (o, i) => i).FirstOrDefault();
           if (rmaEntity == null)
               return this.RenderError(r => r.Message = "RMA 不存在");
           using (var ts = new TransactionScope())
           {
               rmaEntity.Status = (int)RMAStatus.Complete;
               rmaEntity.UpdateDate = DateTime.Now;
               _rmaRepo.Update(rmaEntity);

               _rmalogRepo.Insert(new RMALogEntity()
               {
                   CreateDate = request.UpdateTime,
                   CreateUser = 0,
                   Operation = "退款完成！",
                   RMANo = rmaEntity.RMANo
               });
               ts.Complete();
           }
           return this.RenderSuccess<BaseResponse>(null);
       }
    }
}
