using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;
using com.intime.fashion.common.Extension;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class ResourceController:RestfulController
    {
        private IResourceService _resourceService;
        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        [RestfulRoleAuthorize(Model.Enums.UserLevel.DaoGou)]
        public ActionResult Upload(int authuid,int image_type)
        {
            var files = HttpContext.Request.Files;
            if (files.Count!=1)
             return this.RenderError(r=>r.Message="必须选择唯一一个图片上传！");
            
            int[] permitTypes = new int[]{(int)SourceType.Product,(int)SourceType.CustomerPortrait,(int)SourceType.Combo};
            if (!permitTypes.Contains(image_type))
                return this.RenderError(r=>r.Message="图片类型不允许！");
            var resources = _resourceService.Save(files
                                     , authuid
                                     , -1
                                     , 0
                                     , (SourceType)image_type)
                             .Select(r => new { 
                                id = r.Id,
                                url = image_type==(int)SourceType.CustomerPortrait?r.Name.Image100Url():r.Name.Image320Url()
                             });
            if (resources != null && resources.Count() > 0)
                return this.RenderSuccess<dynamic>(c => c.Data = resources.First());
            else
                return this.RenderError(r => r.Message = "图片不可识别");
        }
    }
}
