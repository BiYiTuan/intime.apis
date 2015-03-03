using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yintai.Hangzhou.Contract.DTO.Request.Product;
using Yintai.Hangzhou.Contract.Product;
using Yintai.Hangzhou.Contract.Promotion;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.Tools.ApiTest.Controllers
{
    public class TestProductController : Controller
    {
        /// <summary>
        /// url 解码
        /// </summary>
        /// <param name="encodeString"></param>
        /// <returns></returns>
        public static string UrlDecode(string encodeString)
        {
            if (String.IsNullOrWhiteSpace(encodeString))
            {
                return String.Empty;
            }

            return HttpUtility.UrlDecode(encodeString);
        }

        //
        // GET: /Product/
        private IProductDataService _productDataService;//= new PromotionDataService(new PromotionRepository(), new FavoriteRepository(), new ShareRepository(), new CouponDataService(new CouponRepository(), new TimeSeedRepository(), new PromotionRepository(), new StoreRepository()), new CustomerRepository(), new ResourceService(new ResourceRepository()));

        public TestProductController(IProductDataService productDataService)
        {
            this._productDataService = productDataService;
        }

        [HttpGet()]
        public ActionResult Create()
        {
            return View();
        }

        [RestfulAuthorize]
        [HttpPost()]
        public ActionResult Create(FormCollection formCollection, CreateProductRequest request, int? authuid)
        {
            request.AuthUid = authuid.Value;
            request.Description = UrlDecode(request.Description);
            request.Favorable = UrlDecode(request.Favorable);
            request.Name = UrlDecode(request.Name);
            request.RecommendedReason = UrlDecode(request.RecommendedReason);
            request.RecommendUser = request.AuthUid;
            request.Files = Request.Files;

            this._productDataService.CreateProduct(request);

            return View();
        }

    }
}
