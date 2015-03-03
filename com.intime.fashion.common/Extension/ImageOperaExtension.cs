using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Extension
{
    public static class ImageOperaExtension
    {
        public static string Image160Url(this string name)
        {
            return ImageUrlWithSize(name, "160x0");
        }
        public static string Image320Url(this string name)
        {
            return ImageUrlWithSize(name,"320x0");
        }

        public static string Image640Url(this string name)
        {
            return ImageUrlWithSize(name, "640x0");
        }
        public static string Image100Url(this string name)
        {
            return ImageUrlWithSize(name, "100x100");
        }
        public static string ImageUrlWithSize(string name,string size)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            return string.Format("{0}{1}",Path.Combine(ConfigManager.GetHttpApiImagePath(),name),string.Format("_{0}.jpg",size));
        }
    }
}
