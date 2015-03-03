using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
   [RestfulAuthorize]
    public class PMessageController : RestfulController
    {
       private IPMessageRepository _pmRepo;
       public PMessageController(IPMessageRepository pmRepo)
       {
           _pmRepo = pmRepo;
       }
       /// <summary>
       /// create private message from current user to other user
       /// </summary>
       /// <param name="request"></param>
       /// <param name="authUser"></param>
       /// <returns></returns>
       public ActionResult Say(NewPMessageRequest request, UserModel authUser)
       {
           if (!ModelState.IsValid)
           {
               var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
               return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
           }
           var messageEntity = _pmRepo.Insert(new PMessageEntity() { 
             CreateDate = DateTime.Now,
              FromUser = authUser.Id,
               IsAuto = false,
                IsVoice = false,
                 TextMsg = request.TextMsg,
                  ToUser = request.ToUser
           });
           return this.RenderSuccess<NewPMessageResponse>(r=>r.Data=new NewPMessageResponse().FromEntity<NewPMessageResponse>(messageEntity, m => {
                   m.FromUserModel = new UserInfoResponse().FromEntity<UserInfoResponse>(Context.Set<UserEntity>().Where(u => u.Id == messageEntity.FromUser).FirstOrDefault());
                   m.ToUserModel = new UserInfoResponse().FromEntity<UserInfoResponse>(Context.Set<UserEntity>().Where(u => u.Id == messageEntity.ToUser).FirstOrDefault());
               }));
          
       }

       /// <summary>
       /// return current user's all conversion window
       /// </summary>
       /// <param name="authUser"></param>
       /// <returns></returns>
       public ActionResult List(PagerInfoRequest request, UserModel authUser)
       {
           var linq =  Context.Set<PMessageEntity>().Where(p => p.FromUser == authUser.Id)
                       .GroupBy(p=>p.ToUser)
                       .Select(p=>new {ToUser = p.Key,Id = p.Max(pid=>pid.Id)})
                       .Union(Context.Set<PMessageEntity>().Where(p => p.ToUser == authUser.Id)
                       .GroupBy(p=>p.FromUser)
                       .Select(p=>new {ToUser = p.Key,Id = p.Max(pid=>pid.Id)}))
                       .GroupBy(p=>p.ToUser)
                       .Select(p=>new {Id = p.Max(pid=>pid.Id)});
           var linq2 = linq.Join(Context.Set<PMessageEntity>(),o=>o.Id,i=>i.Id,(o,i)=>i)
                      .Join(Context.Set<UserEntity>(), o => o.FromUser, i => i.Id, (o, i) => new { M = o, F = i })
                      .Join(Context.Set<UserEntity>(), o => o.M.ToUser, i => i.Id, (o, i) => new { M = o.M, F = o.F, T = i });
           int totalCount = linq2.Count();
           int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
           linq2 = linq2.OrderByDescending(c => c.M.Id).Skip(skipCount).Take(request.Pagesize);
           var responseData = linq2.ToList().Select(l=>new NewPMessageResponse().FromEntity<NewPMessageResponse>(l.M,
                                          c =>
                                          {
                                             c.FromUserModel=new UserInfoResponse().FromEntity<UserInfoResponse>(l.F);
                                             c.ToUserModel = new UserInfoResponse().FromEntity<UserInfoResponse>(l.T);

                                          }));
           var response = new PagerInfoResponse<NewPMessageResponse>(request.PagerRequest, totalCount)
           {
               Items = responseData.ToList()
           };
           return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<NewPMessageResponse>>(response) };

       }
       /// <summary>
       /// return the conversation list current user with the query user
       /// </summary>
       /// <param name="request"></param>
       /// <param name="authUser"></param>
       /// <returns></returns>
       public ActionResult Conversation(GetConversationRequest request, UserModel authUser)
       {
           var linq = Context.Set<PMessageEntity>().Where(p=>(p.FromUser==authUser.Id && p.ToUser==request.UserId) ||
                                        (p.FromUser==request.UserId && p.ToUser==authUser.Id))
                                        .Where(p=>request.LastConversationId==0 || (request.LastConversationId>0 && p.Id>request.LastConversationId) )
                      .Join(Context.Set<UserEntity>(), o => o.FromUser, i => i.Id, (o, i) => new { M = o, F = i })
                      .Join(Context.Set<UserEntity>(), o => o.M.ToUser, i => i.Id, (o, i) => new { M = o.M, F = o.F, T = i });
           int totalCount = linq.Count();
           int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
           int pageSize = request.Pagesize;
           if (request.LastConversationId > 0)
           {
               skipCount = 0;
               pageSize = totalCount;
           }
           linq = linq.OrderByDescending(c => c.M.Id).Skip(skipCount).Take(pageSize);
           var responseData = linq.ToList().Select(l => new NewPMessageResponse().FromEntity<NewPMessageResponse>(l.M,
                                          c =>
                                          {
                                              c.FromUserModel = new UserInfoResponse().FromEntity<UserInfoResponse>(l.F);
                                              c.ToUserModel = new UserInfoResponse().FromEntity<UserInfoResponse>(l.T);

                                          }));
           var response = new PagerInfoResponse<NewPMessageResponse>(request.PagerRequest, totalCount)
           {
               Items = responseData.ToList()
           };
           return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<NewPMessageResponse>>(response) };
       }
    }
}
