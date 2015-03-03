using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request;
using Yintai.Hangzhou.Contract.DTO.Request.Favorite;
using Yintai.Hangzhou.Contract.Favorite;
using Yintai.Hangzhou.Contract.Request.Favorite;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.WebSupport.Binder;
using Yintai.Hangzhou.WebSupport.Mvc;
using System.Linq;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.DTO.Response.Product;
using System.Web.Mvc;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class FavoriteController : RestfulController
    {
        private readonly IFavoriteDataService _favoriteDataService;

        public FavoriteController(IFavoriteDataService favoriteDataService)
        {
            this._favoriteDataService = favoriteDataService;
        }

        [RestfulAuthorize]
        public RestfulResult Create(FavoriteCreateRequest request, int? authuid)
        {
            request.AuthUid = authuid.Value;
            return new RestfulResult { Data = this._favoriteDataService.Create(request) };
        }

        [RestfulAuthorize]
        public RestfulResult List(GetFavoriteListRequest request, int? authuid, UserModel authUser)
        {
            //判断当前被读取的收藏列表的USER，是否是达人or 店长
            if (authUser == null)
            {
                return new RestfulResult { Data = new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "user未找到" } };
            }
            request.UserModel = authUser;

            return new RestfulResult { Data = this._favoriteDataService.GetFavoriteList(request) };
        }

       
        [RestfulAuthorize]
        public RestfulResult Destroy(FavoriteDestroyRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;

            return new RestfulResult { Data = this._favoriteDataService.Destroy(request) };
        }

        [RestfulAuthorize]
        public ActionResult My(GetFavorListRequest request, int? authuid, UserModel authUser)
        {
            if (!(new int[] { (int)SourceType.Product, (int)SourceType.Promotion }).Contains(request.SourceType))
            {
                request.SourceType = (int)SourceType.Product;
            }
            var linq = Context.Set<FavoriteEntity>().Where(p => p.User_Id == authUser.Id && p.FavoriteSourceType == request.SourceType && p.Status != (int)DataStatus.Deleted);
            if (request.SourceType == (int)SourceType.Product)
            {
                var linq2 = linq.Join(Context.Set<ProductEntity>(), o => o.FavoriteSourceId, i => i.Id, (o, i) => new { P = i, F = o })
                          .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product && r.Type == (int)ResourceType.Image), o => o.P.Id, i => i.SourceId, (o, i) => new { P = o.P, F = o.F, R = i });
                int totalCount = linq2.Count();
                int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
                linq2 = linq2.OrderByDescending(l => l.P.CreatedDate).Skip(skipCount).Take(request.Pagesize);

                return this.RenderSuccess<PagerInfoResponse<ProductInfoResponse>>(r =>
                {
                    r.Data = new PagerInfoResponse<ProductInfoResponse>(request.PagerRequest, totalCount)
                    {
                        Items = linq2.ToList().Select(l => new ProductInfoResponse().FromEntity<ProductInfoResponse>(l.P, p =>
                        {
                            p.ResourceInfoResponses = l.R.OrderByDescending(pr => pr.SortOrder).Select(pr => new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(pr)).ToList();
                        })).ToList()
                    };
                });
            }
            else
            {
                var linq2 = linq.Join(Context.Set<PromotionEntity>(), o => o.FavoriteSourceId, i => i.Id, (o, i) => new { P = i, F = o })
                        .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Promotion && r.Type == (int)ResourceType.Image), o => o.P.Id, i => i.SourceId, (o, i) => new { P = o.P, F = o.F, R = i });
                int totalCount = linq2.Count();
                int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
                linq2 = linq2.OrderByDescending(l => l.P.CreatedDate).Skip(skipCount).Take(request.Pagesize);

                return this.RenderSuccess<PagerInfoResponse<PromotionInfoResponse>>(r =>
                {
                    r.Data = new PagerInfoResponse<PromotionInfoResponse>(request.PagerRequest, totalCount)
                    {
                        Items = linq2.ToList().Select(l => new PromotionInfoResponse().FromEntity<PromotionInfoResponse>(l.P, p =>
                        {
                            p.ResourceInfoResponses = l.R.OrderByDescending(pr => pr.SortOrder).Select(pr => new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(pr)).ToList();
                        })).ToList()
                    };
                });
            }


        }

        public ActionResult Daren(GetFavorListRequest request,int userId)
        {
            if (!(new int[] { (int)SourceType.Product, (int)SourceType.Promotion }).Contains(request.SourceType))
            {
                request.SourceType = (int)SourceType.Product;
            }
            var linq = Context.Set<FavoriteEntity>().Where(p => p.User_Id == userId && p.FavoriteSourceType == request.SourceType && p.Status != (int)DataStatus.Deleted);
            if (request.SourceType == (int)SourceType.Product)
            {
                var linq2 = linq.Join(Context.Set<ProductEntity>(), o => o.FavoriteSourceId, i => i.Id, (o, i) => new { P = i, F = o })
                          .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product && r.Type == (int)ResourceType.Image), o => o.P.Id, i => i.SourceId, (o, i) => new { P = o.P, F = o.F, R = i });
                int totalCount = linq2.Count();
                int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
                linq2 = linq2.OrderByDescending(l => l.P.CreatedDate).Skip(skipCount).Take(request.Pagesize);

                return this.RenderSuccess<PagerInfoResponse<ProductInfoResponse>>(r =>
                {
                    r.Data = new PagerInfoResponse<ProductInfoResponse>(request.PagerRequest, totalCount)
                    {
                        Items = linq2.ToList().Select(l => new ProductInfoResponse().FromEntity<ProductInfoResponse>(l.P, p =>
                        {
                            p.ResourceInfoResponses = l.R.OrderByDescending(pr => pr.SortOrder).Select(pr => new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(pr)).ToList();
                        })).ToList()
                    };
                });
            }
            else
            {
                var linq2 = linq.Join(Context.Set<PromotionEntity>(), o => o.FavoriteSourceId, i => i.Id, (o, i) => new { P = i, F = o })
                        .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Promotion && r.Type == (int)ResourceType.Image), o => o.P.Id, i => i.SourceId, (o, i) => new { P = o.P, F = o.F, R = i });
                int totalCount = linq2.Count();
                int skipCount = request.Page > 0 ? (request.Page - 1) * request.Pagesize : 0;
                linq2 = linq2.OrderByDescending(l => l.P.CreatedDate).Skip(skipCount).Take(request.Pagesize);

                return this.RenderSuccess<PagerInfoResponse<PromotionInfoResponse>>(r =>
                {
                    r.Data = new PagerInfoResponse<PromotionInfoResponse>(request.PagerRequest, totalCount)
                    {
                        Items = linq2.ToList().Select(l => new PromotionInfoResponse().FromEntity<PromotionInfoResponse>(l.P, p =>
                        {
                            p.ResourceInfoResponses = l.R.OrderByDescending(pr => pr.SortOrder).Select(pr => new ResourceInfoResponse().FromEntity<ResourceInfoResponse>(pr)).ToList();
                        })).ToList()
                    };
                });
            }


        }
    }
}