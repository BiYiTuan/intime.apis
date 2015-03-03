using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.DTO.Request.Point;
using Yintai.Hangzhou.Contract.Point;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    [RestfulAuthorize]
    public class PointController :RestfulController
    {
        private readonly IPointDataService _pointDataService;

        public PointController(IPointDataService pointDataService)
        {
            _pointDataService = pointDataService;
        }

        public ActionResult List(GetListPointCollectionRequest request,int? authuid ,UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;

            return new RestfulResult { Data = this._pointDataService.GetList(request) };
        }

        public ActionResult Detail(GetPointInfoRequest request, int? authuid, UserModel authUser)
        {
            request.AuthUid = authuid.Value;
            request.AuthUser = authUser;

            return new RestfulResult { Data = this._pointDataService.Get(request) };
        }
    }
}
