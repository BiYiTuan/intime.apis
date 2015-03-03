using System;
using System.Web.Mvc;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request.SpecialTopic;
using Yintai.Hangzhou.Contract.SpecialTopic;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.WebSupport.Binder;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class SpecialTopicController : RestfulController
    {
        private readonly ISpecialTopicDataService _specialTopicDataService;

        public SpecialTopicController(ISpecialTopicDataService specialTopicDataService)
        {
            _specialTopicDataService = specialTopicDataService;
        }

        public ActionResult List(GetSpecialTopicListRequest request)
        {
            request.Type = UrlDecode(request.Type);

            if (!String.IsNullOrEmpty(request.Type))
            {
                if (request.Type.ToLower() == "refresh")
                {
                    return new RestfulResult
                    {
                        Data = _specialTopicDataService.GetForRefresh(new GetSpecialTopicListForRefresh
                            {
                                Page = request.Page,
                                Pagesize = request.Pagesize,
                                RefreshTs = request.RefreshTs,
                                Sort = request.Sort
                            })
                    };
                }
            }
            else
            {
                return new RestfulResult { Data = _specialTopicDataService.GetList(request) };
            }

            return new RestfulResult();
        }

        public ActionResult Detail(GetSpecialTopicInfoRequest request, [FetchRestfulAuthUserAttribute(IsCanMissing = true, KeyName = Define.Token)]UserModel currentAuthUser)
        {
            request.CurrentAuthUser = currentAuthUser;

            return new RestfulResult { Data = _specialTopicDataService.GetInfo(request) };
        }
    }
}
