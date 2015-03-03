using System.Web.Mvc;
using System.Web.UI.WebControls;
using Yintai.Hangzhou.Contract.DTO.Request.Promotion;
using Yintai.Hangzhou.Contract.Promotion;
using Yintai.Hangzhou.Repository.Impl;
using Yintai.Hangzhou.Service;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.Tools.ApiTest.Controllers
{
    public class TestPromotionController : Controller
    {
        private IPromotionDataService _promotionDataService;//= new PromotionDataService(new PromotionRepository(), new FavoriteRepository(), new ShareRepository(), new CouponDataService(new CouponRepository(), new TimeSeedRepository(), new PromotionRepository(), new StoreRepository()), new CustomerRepository(), new ResourceService(new ResourceRepository()));

        public TestPromotionController(IPromotionDataService promotionDataService)
        {
            this._promotionDataService = promotionDataService;
        }

        [HttpGet()]
        public ActionResult Create()
        {
            return View();
        }

        [RestfulAuthorize]
        [HttpPost()]
        public ActionResult Create(FormCollection formCollection, CreatePromotionRequest request, int? authuid)
        {
            request.Files = Request.Files;
            request.AuthUid = authuid.Value;

            this._promotionDataService.CreatePromotion(request);

            return View();
        }
    }
}