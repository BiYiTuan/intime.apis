using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request.Tag;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Contract.Tag;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class TagController : RestfulController
    {
        private readonly ITagDataService _tagDataService;

        public TagController(ITagDataService tagDataService)
        {
            this._tagDataService = tagDataService;
        }

        public RestfulResult Detail(TagGetRequest request)
        {
            Logger.Debug(string.Format("request channel:{0}", request.Channel));
            return new RestfulResult { Data = this._tagDataService.Get(request) };
        }

        public RestfulResult All(TagGetAllRequest request)
        {
            if (String.IsNullOrEmpty(request.Type))
            {
                return new RestfulResult { Data = this._tagDataService.GetAll(request) };
            }

            if (request.Type.ToLower() == "refresh")
            {
                return Refresh(new TagGetRefreshRequest
                {
                    RTs = request.Refreshts
                });
            }

            return new RestfulResult { Data = this._tagDataService.GetAll(request) };
        }

        public RestfulResult Refresh(TagGetRefreshRequest request)
        {
            return new RestfulResult { Data = this._tagDataService.GetRefresh(request) };
        }

        [RestfulRoleAuthorize(UserRole.Admin)]
        [HttpPost]
        public RestfulResult Create(TagCreateRequest request, int? authuid)
        {
            request.Name = UrlDecode(request.Name);
            request.Description = UrlDecode(request.Description);
            request.AuthUid = authuid.Value;

            return new RestfulResult { Data = this._tagDataService.Create(request) };
        }

        [RestfulRoleAuthorize(UserRole.Admin)]
        [HttpPost]
        public RestfulResult Update(TagUpdateRequest request, int? authuid)
        {
            request.Name = UrlDecode(request.Name);
            request.Description = UrlDecode(request.Description);
            request.AuthUid = authuid.Value;

            return new RestfulResult { Data = this._tagDataService.Update(request) };
        }

        /// <summary>
        /// return request tag's available properties
        /// </summary>
        /// <param name="request"></param>
        /// <param name="authUser"></param>
        /// <returns></returns>
        public ActionResult Property(TagGetRequest request, UserModel authUser)
        {
            var linq = Context.Set<CategoryPropertyEntity>().Where(c => c.CategoryId == request.TagId && c.Status == (int)DataStatus.Normal)
                        .GroupJoin(Context.Set<CategoryPropertyValueEntity>().Where(pv => pv.Status == (int)DataStatus.Normal),
                                o => o.Id,
                                i => i.PropertyId,
                                (o, i) => new { P = o, PV = i })
                        .OrderByDescending(p => p.P.SortOrder);
            var result = linq.ToList()
                        .Select(l => new TagPropertyDetailResponse().FromEntity<TagPropertyDetailResponse>(l.P, p => {
                            p.PropertyId = l.P.Id;
                            p.PropertyName = l.P.PropertyDesc;
                            p.Values = l.PV.Select(pv => new TagPropertyValueDetailResponse() { 
                                 ValueId = pv.Id,
                                  ValueName =pv.ValueDesc
                            });
                        }));
            var response = new PagerInfoResponse<TagPropertyDetailResponse>(new PagerRequest(), result.Count())
            {
                Items = result.ToList()
            };
            return new RestfulResult { Data = new ExecuteResult<PagerInfoResponse<TagPropertyDetailResponse>>(response) };
        }
    }
}
