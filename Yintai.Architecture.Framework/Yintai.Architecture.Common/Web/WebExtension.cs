using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Yintai.Architecture.Common.Web
{
    public static class WebExtension
    {
        /// <summary>
        /// 是否包含文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
}
