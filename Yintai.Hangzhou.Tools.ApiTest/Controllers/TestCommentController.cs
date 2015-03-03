using System;
using System.Web;
using System.Web.Mvc;
using Yintai.Hangzhou.Contract.Comment;
using Yintai.Hangzhou.Contract.DTO.Request.Comment;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.WebSupport.Binder;
using Yintai.Hangzhou.WebSupport.Mvc;

namespace Yintai.Hangzhou.Tools.ApiTest.Controllers
{
    public class TestCommentController : Controller
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
        private ICommentDataService _commentDataService;//= new PromotionDataService(new PromotionRepository(), new FavoriteRepository(), new ShareRepository(), new CouponDataService(new CouponRepository(), new TimeSeedRepository(), new PromotionRepository(), new StoreRepository()), new CustomerRepository(), new ResourceService(new ResourceRepository()));

        public TestCommentController(ICommentDataService commentDataService)
        {
            this._commentDataService = commentDataService;
        }

        [HttpGet()]
        public ActionResult Create()
        {
            return View();
        }

        //[RestfulAuthorize]
        [HttpPost()]
        public ActionResult Create(FormCollection formCollection, CommentCreateRequest request, [FetchUser(KeyName = "userid")]UserModel userModel)
        {
            request.AuthUid = userModel.Id;
            request.Files = Request.Files;
            request.AuthUser = userModel;
            request.Content = UrlDecode(request.Content);

            this._commentDataService.Create(request);

            return new ContentResult(){Content = "OK"};
        }


        public ActionResult Del(FormCollection formCollection, CommentDestroyRequest request, [FetchUser(KeyName = "userid")]UserModel userModel)
        {
            request.AuthUid = userModel.Id;
            request.AuthUser = userModel;

            this._commentDataService.Destroy(request);

            return new ContentResult() { Content = "OK" };
        }

    }
}