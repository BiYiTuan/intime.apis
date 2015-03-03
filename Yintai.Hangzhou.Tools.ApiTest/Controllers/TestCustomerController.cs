using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yintai.Hangzhou.Contract.DTO.Request.Product;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.Tools.ApiTest.Controllers
{
    public class TestCustomerController : Controller
    {        /// <summary>
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
        // GET: /TestCustomer/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            return View(); 
        }

        //[RestfulAuthorize]
        //public ActionResult Create(FormCollection formCollection, CreateProductRequest request, int? authuid)
        //{
        //    request.AuthUid = authuid.Value;
        //    request.Description = UrlDecode(request.Description);
        //    request.Favorable = UrlDecode(request.Favorable);
        //    request.Name = UrlDecode(request.Name);
        //    request.RecommendedReason = UrlDecode(request.RecommendedReason);
        //    request.RecommendUser = request.AuthUid;
        //    request.Files = Request.Files;

        //    //this._productDataService.CreateProduct(request);

        //    return View();
        //}
    }
}
