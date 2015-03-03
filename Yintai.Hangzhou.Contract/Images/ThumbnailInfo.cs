using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Yintai.Hangzhou.Contract.Images
{
    /// <summary>
    /// 缩率图信息
    /// </summary>
    [MessageContract]
    public class ThumbnailInfo
    {
        /// <summary>
        /// 缩略图文件大小
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public IDictionary<string, long> Info;

        /// <summary>
        /// Exif信息
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public IDictionary<int, string> ExifInfos;

        /// <summary>
        /// 缩略图尺寸
        /// </summary>
        [MessageBodyMember]
        public IDictionary<string, ImageSize> Sizes;


    }

    /// <summary>
    /// 尺寸
    /// </summary>
    [MessageContract]
    [DataContract]
    public class ImageSize
    {
        /// <summary>
        /// 宽y
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        [DataMember]
        public int Width;

        /// <summary>
        /// 高
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        [DataMember]
        public int Height;

        /// <summary>
        /// 长度（影片 长度 单位：秒）
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        [DataMember]
        public long Length;

        public ImageSize(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public ImageSize(int w, int h, long l)
        {
            Width = w;
            Height = h;
            Length = l;
        }
    }
}
