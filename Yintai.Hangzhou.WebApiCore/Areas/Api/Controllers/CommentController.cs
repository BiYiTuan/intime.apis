using System;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.Comment;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Request.Comment;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;
using Yintai.Hangzhou.Data.Models;
using Yintai.Architecture.Framework;
using Yintai.Hangzhou.Contract.DTO.Response.Comment;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class CommentController : RestfulController
    {
        private readonly ICommentDataService _commentDataService;
        private ICommentRepository _commentRepo;
        private IResourceRepository _resourceRepo;
        private ICustomerRepository _customerRepo;
        private IProductRepository _productRepo;
        private IPromotionRepository _promotionRepo;

        public CommentController(ICommentDataService commentDataService,
            ICommentRepository commentRepo,
            IResourceRepository resourceRepo,
            ICustomerRepository customerRepo,
            IPromotionRepository promotionRepo,
            IProductRepository productRepo)
        {
            _commentDataService = commentDataService;
            _commentRepo = commentRepo;
            _customerRepo = customerRepo;
            _resourceRepo = resourceRepo;
            _productRepo = productRepo;
            _promotionRepo = promotionRepo;
        }

        public ActionResult List(CommentListRequest request)
        {
            var linq = _commentRepo.Get(c => c.Status != (int)DataStatus.Deleted && c.SourceId == request.SourceId && c.SourceType == request.SourceType)
                           .GroupJoin(_resourceRepo.Get(r => r.Status != (int)DataStatus.Deleted && r.SourceType == (int)SourceType.CommentAudio),
                                       o => o.Id,
                                       i => i.SourceId,
                                       (o, i) => new { C = o, Aud = i })
                           .GroupJoin(_customerRepo.Get(cu => cu.Status != (int)DataStatus.Deleted),
                                       o => o.C.User_Id,
                                       i => i.Id,
                                       (o, i) => new { C = o.C, Aud = o.Aud, U = i.FirstOrDefault() })
                            .GroupJoin(_customerRepo.Get(cu => cu.Status != (int)DataStatus.Deleted),
                                       o => o.C.ReplyUser,
                                       i => i.Id,
                                       (o, i) => new { C = o.C, Aud = o.Aud, U = o.U, RU = i.FirstOrDefault() })
                            .GroupJoin(_commentRepo.Get(cr => cr.Status != (int)DataStatus.Deleted), o => o.C.ReplyId, i => i.Id,
                                      (o, i) => new { C = o.C, Aud = o.Aud, U = o.U, RU =o.RU,CR=i.FirstOrDefault()});
            
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(c => c.C.CreatedDate).Skip(skipCount).Take(request.Pagesize);
            var responseData = from l in linq.ToList()
                               select new CommentInfoResponse().FromEntity<CommentInfoResponse>(l.C,
                                           c =>
                                           {
                                               c.Customer = new ShowCustomerInfoResponse().FromEntity<ShowCustomerInfoResponse>(l.U);
                                               if (l.RU !=null)
                                               {
                                                   c.ReplyUserNickname = l.RU.Nickname;
                                                   c.ReplyUser = l.RU.Id;
                                               }
                                               c.ResourceInfoResponses=l.Aud.Select(ca=>new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(ca)).ToList();

                                           });
            var response = new CommentCollectionResponse(request.PagerRequest, totalCount)
            {
               Comments = responseData.ToList()
            };
            return this.RenderSuccess<CommentCollectionResponse>(r => r.Data = response);
                
        }

        [HttpPost]
        [RestfulAuthorize]
        public ActionResult Create(CommentCreateRequest request, int? authuid , UserModel authUser)
        {
            request.Content = UrlDecode(request.Content);
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            request.Files = Request.Files;

            return new RestfulResult { Data = this._commentDataService.Create(request) };
        }

        public ActionResult Detail(CommentDetailRequest request)
        {
            return new RestfulResult { Data = this._commentDataService.Detail(request) };
        }

        [HttpPost]
        [RestfulAuthorize]
        public ActionResult Destroy(CommentDestroyRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            return new RestfulResult { Data = this._commentDataService.Destroy(request) };
        }

        [HttpPost]
        [RestfulAuthorize]
        public ActionResult Update(CommentUpdateRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;
            request.Content = UrlDecode(request.Content);
            request.Files = Request.Files;

            return new RestfulResult { Data = this._commentDataService.Update(request) };
        }

        [RestfulAuthorize]
        public ActionResult My(MyCommentListRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUser = authUser;

            var comments = _commentRepo.Get(c => c.Status != (int)DataStatus.Deleted).OrderByDescending(c => c.CreatedDate)
                            .GroupJoin(_resourceRepo.Get(r => r.Status != (int)DataStatus.Deleted && r.SourceType == (int)SourceType.CommentAudio),
                                        o => o.Id,
                                        i => i.SourceId,
                                        (o, i) => new { C = o, Aud = i.FirstOrDefault() })
                            .GroupJoin(_customerRepo.Get(cu => cu.Status != (int)DataStatus.Deleted),
                                        o => o.C.User_Id,
                                        i => i.Id,
                                        (o, i) => new { C = o.C, Aud = o.Aud, U = i.FirstOrDefault() })
                             .GroupJoin(_customerRepo.Get(cu => cu.Status != (int)DataStatus.Deleted),
                                        o => o.C.ReplyUser,
                                        i => i.Id,
                                        (o, i) => new { C = o.C, Aud = o.Aud, U = o.U, RU = i.FirstOrDefault() });
            var dbContext = _commentRepo.Context;
            
            var linq = (from c2 in dbContext.Set<CommentEntity>()
                        let products = dbContext.Set<ProductEntity>().Where(p=>p.Status!=(int)DataStatus.Deleted && p.RecommendUser ==authUser.Id)
                        let promotions = dbContext.Set<PromotionEntity>().Where(p=>p.Status!=(int)DataStatus.Deleted && p.RecommendUser == authUser.Id)
                        where (c2.User_Id == authUser.Id && c2.Status != (int)DataStatus.Deleted) ||
                               products.Any(p=>c2.SourceId == p.Id && c2.SourceType==(int)SourceType.Product) ||
                               promotions.Any(p=>c2.SourceId == p.Id && c2.SourceType == (int)SourceType.Promotion)
                         select new { SourceId = c2.SourceId, SourceType = c2.SourceType }).Distinct()
                           .GroupJoin(comments,
                                o => new { SourceType = o.SourceType, SourceId = o.SourceId },
                                i => new { SourceType = i.C.SourceType, SourceId = i.C.SourceId },
                                (o, i) => new { SourceType = o.SourceType, SourceId = o.SourceId, Comment = i.OrderByDescending(c=>c.C.CreatedDate).FirstOrDefault() });
            int totalCount = linq.Count();
            int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
            linq = linq.OrderByDescending(c => c.Comment.C.CreatedDate).Skip(skipCount).Take(request.Pagesize);
             var responseData = from l in linq.ToList()
                                select new MyCommentInfoResponse().FromEntity<MyCommentInfoResponse>(l.Comment.C,
                                            c =>
                                            {
                                                //c.SourceId = l.SourceId;
                                               // c.SourceType = l.SourceType;
                                                c.CommentUser = new UserInfoResponse().FromEntity<UserInfoResponse>(l.Comment.U);
                                                c.ReplyUserName = l.Comment.RU == null ? string.Empty : l.Comment.RU.Nickname;
                                                c.Resource = new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(l.Comment.Aud);

                                            });
            var response = new PagerInfoResponse<MyCommentInfoResponse>(request.PagerRequest, totalCount)
            {
                Items = responseData.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<MyCommentInfoResponse>>(response) };
                            

        }
    }
}
