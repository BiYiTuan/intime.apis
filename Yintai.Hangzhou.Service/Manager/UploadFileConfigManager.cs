using com.intime.fashion.common;
using System;
using System.IO;

namespace Yintai.Hangzhou.Service.Manager
{
    public enum UploadFileType
    {
        Default = 0,

        Image = 1,

        Sound = 2,

        Video = 3
    }

    [System.Obsolete("过期请使用ConfigManager")]
    public class UploadFileConfigManager
    {
        public static string GetHttpApiImagePath()
        {
            return ConfigManager.GetHttpApiImagePath();
            //return _domain + "api/" + _imageUpload;
        }

        //public static string GetHttpApiSoundPath()
        //{
        //    return _domain + _soundUpload;
        //    //return _domain + "api/" + _soundUpload;
        //}

        //public static string GetPhysicalImagePath()
        //{
        //    return GetPhysicalApiPath(UploadFileType.Image);
        //}

        //public static string GetPhysicalSoundPath()
        //{
        //    return GetPhysicalApiPath(UploadFileType.Sound);
        //}

        //public static string GetPhysicalApiPath(UploadFileType sourceType)
        //{
        //    return GetPhysicalApiPath(sourceType, null);
        //}

        //public static string GetPhysicalApiPath(UploadFileType sourceType, string key)
        //{
        //    return GetPhysicalPath(sourceType, String.Empty, key);
        //    //return GetPhysicalPath(sourceType, "/api/", key);
        //}

        //public static string GetPhysicalPath(UploadFileType sourceType, string appType, string key)
        //{
        //    switch (sourceType)
        //    {
        //        case UploadFileType.Image:
        //            return Path.Combine(_domainPath, _imageUpload);
        //        case UploadFileType.Sound:
        //            return Path.Combine(_domainPath, _soundUpload);
        //        case UploadFileType.Video:
        //            return Path.Combine(_domainPath, _videoUpload);
        //        default:
        //            return Path.Combine(_domainPath, _defUplad);
        //    }
        //}
    }
}
