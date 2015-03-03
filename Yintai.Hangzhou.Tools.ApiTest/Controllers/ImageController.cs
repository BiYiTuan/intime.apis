using System.Web.Mvc;

namespace Yintai.Hangzhou.Tools.ApiTest.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            //foreach (string f in Request.Files)
            //{
            //    if (!Request.Files[f].HasFile()) continue;

            //    FileInfor file;
            //    ThumbnailInfo thumbnailInfo;
            //    var result = ExampleCore.UploadFileAndReturnInfo(Request.Files[f], "promotion", out file, out thumbnailInfo);
            //}

            return View();
        }
    }
}
