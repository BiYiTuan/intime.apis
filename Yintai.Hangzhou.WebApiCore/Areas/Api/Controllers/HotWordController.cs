using System.Web.Mvc;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Common.Web.Mvc.Controllers;
using Yintai.Hangzhou.Contract.HotWord;

namespace Yintai.Hangzhou.WebApiCore.Areas.Api.Controllers
{
    public class HotWordController : RestfulController
    {
        private readonly IHotwordDataService _service;

        public HotWordController(IHotwordDataService hotwordDataService)
        {
            _service = hotwordDataService;
        }

        public ActionResult List()
        {
            return new RestfulResult { Data = _service.GetCollection() };
        }
    }
}
