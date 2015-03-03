using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Yintai.Architecture.Common.Models
{
    public class ContentType
    {
        /// <summary>
        /// key = contenttype value=extension
        /// </summary>
        static ContentType()
        {
            Image = new Dictionary<string, string>
                {
                    { "application/x-jpe", "jpe" }
                    ,{ "application/x-png","png" }
                    ,{"application/x-jpg","jpg"}

                    ,{"image/png", "png" }
                    ,{"image/jpeg","jpeg"}
                    ,{"image/gif","gif"}
                    ,{"image/jpg","jpeg"}
                };

            Sound = new Dictionary<string, string>
                {
                    {"audio/mp3","mp3"}
                    ,{"audio/wav","wav"}
                    ,{"audio/x-ms-wma","wma"}
                    ,{"audio/basic","snd" }
                    ,{"audio/x-m4a","m4a"}
                };

            Video = new Dictionary<string, string>
                {
                    {"video/mp4","mp4"}
                };

            All = Image;

            foreach (var key in Sound.Keys)
            {
                All.Add(key, Sound[key]);
            }

            foreach (var key in Video.Keys)
            {
                All.Add(key, Video[key]);
            }
        }

        public static Dictionary<string, string> All { get; set; }
        public static Dictionary<string, string> Image { get; set; }
        public static Dictionary<string, string> Sound { get; set; }
        public static Dictionary<string, string> Video { get; set; }

        public static KeyValuePair<ResourceType, string> GetExtension(string contentType)
        {
            return GetExtension(contentType, ResourceType.Default);
        }

        public static KeyValuePair<ResourceType, string> GetExtension(string contentType, ResourceType resourceType)
        {
            if (String.IsNullOrEmpty(contentType))
            {
                return new KeyValuePair<ResourceType, string>(ResourceType.Default, null);
            }

            string val;
            switch (resourceType)
            {
                case ResourceType.Image:
                    val = GetExt(contentType, Image);
                    break;
                case ResourceType.Sound:
                    val = GetExt(contentType, Sound);
                    break;
                case ResourceType.Video:
                    val = GetExt(contentType, Video);
                    break;
                default:
                    val = GetExt(contentType, All);
                    break;
            }

            if (String.IsNullOrEmpty(val))
            {
                return new KeyValuePair<ResourceType, string>(ResourceType.Default, null);
            }

            var newResourceType = resourceType;
            if (newResourceType == ResourceType.Default)
            {
                //查找分类
                if (Image.ContainsKey(contentType.ToLower()))
                {
                    newResourceType = ResourceType.Image;
                }
                else
                    if (Sound.ContainsKey(contentType.ToLower()))
                    {
                        newResourceType = ResourceType.Sound;
                    }
                    else
                        if (Video.ContainsKey(contentType.ToLower()))
                        {
                            newResourceType = ResourceType.Video;
                        }
            }

            return new KeyValuePair<ResourceType, string>(newResourceType, val);
        }

        public static string GetExt(string contentType)
        {
            return GetExt(contentType, All);
        }

        private static string GetExt(string contentType, IDictionary<string, string> contents)
        {
            var c = contentType.ToLower();
            string val;

            return contents.TryGetValue(c, out val) ? val : null;
        }

        /// <summary>
        /// 根据扩展名获取已维护的 contenttype
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetContentType(string extension)
        {
            var t = extension.ToLower();

            string contentType;
            if (HttpContentType.All.TryGetValue(t, out contentType))
            {
                return contentType;
            }

            return null;
        }

        /// <summary>
        /// 根据扩展名获取已维护的 ResourceType
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static ResourceType GetResourceType(string extension)
        {
            var t = extension.ToLower();

            if (HttpContentType.Image.Keys.Contains(t))
            {
                return ResourceType.Image;
            }

            if (HttpContentType.Sound.Keys.Contains(t))
            {
                return ResourceType.Sound;
            }

            if (HttpContentType.Video.Keys.Contains(t))
            {
                return ResourceType.Video;
            }

            return ResourceType.Default;
        }

        private class HttpContentType
        {
            /// <summary>
            /// key=Extension value=contenttype
            /// </summary>
            static HttpContentType()
            {
                Image = new Dictionary<string, string>
                    {
                        {"jpe", "image/jpeg"},//, { "jpe", "application/x-jpe" }
                        {"jpeg", "image/jpeg"},
                        {"jpg", "image/jpeg"},//, { "jpg", "application/x-jpg" }
                        {"png", "image/png"},//, { "png", "application/x-png" }
                        {"gif", "image/gif"}
                    };

                Sound = new Dictionary<string, string>
                    {
                        {"mp3", "audio/mp3"},
                        {"wav", "audio/wav"},
                        {"wma", "audio/x-ms-wma"},
                        {"snd", "audio/basic"},
                        {"m4a","audio/x-m4a"}
                    };


                Video = new Dictionary<string, string> { { "mp4", "video/mp4" } };

                All = new Dictionary<string, string>();

                foreach (var k in Image.Keys)
                {
                    All.Add(k, Image[k]);
                }

                foreach (var k in Sound.Keys)
                {
                    All.Add(k, Sound[k]);
                }

                foreach (var k in Video.Keys)
                {
                    All.Add(k, Video[k]);
                }
            }

            public static Dictionary<string, string> Image { get; private set; }
            public static Dictionary<string, string> Sound { get; private set; }
            public static Dictionary<string, string> Video { get; private set; }
            public static Dictionary<string, string> All { get; private set; }
        }
    }
}