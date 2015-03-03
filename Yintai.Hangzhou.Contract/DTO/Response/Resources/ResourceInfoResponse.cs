
using com.intime.fashion.common;
using System;
using System.Runtime.Serialization;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.Extension;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Model.Enums;
using SType = Yintai.Hangzhou.Model.Enums.SourceType;
namespace Yintai.Hangzhou.Contract.DTO.Response.Resources
{
    [DataContract]
    public class ResourceInfoResponse:BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "sourceid")]
        public int SourceId { get; set; }
        [DataMember(Name = "sourcetype")]
        public int SourceType { get; set; }
        [IgnoreDataMember]
        public SourceType SType
        {
            get { return EnumExtension.Parser<SourceType>(SourceType); }
            set { }
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [IgnoreDataMember]
        public string Domain { get; set; }
        [DataMember(Name = "domain")]
        public string Domain_s { get {
            /*
            if (!string.IsNullOrEmpty(Domain))
                return Domain;
             * */
            switch (Type)
            { 
                case (int)ResourceType.Image:
                    return ConfigManager.GetHttpApiImagePath();
                case (int)ResourceType.Sound:
                    return ConfigManager.GetHttpApiSoundPath();
                case (int)ResourceType.Video:
                    return ConfigManager.GetHttpApivideoPath();
                default:
                    return ConfigManager.GetHttpApidefPath();
            }
        } set { } }
        [IgnoreDataMember]
        public int CreatedUser { get; set; }
        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }
        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }
        [IgnoreDataMember]
        public int UpdatedUser { get; set; }

        [DataMember(Name = "isdefault")]
        public bool IsDefault { get; set; }

        [DataMember(Name = "order")]
        public int SortOrder { get; set; }

        /// <summary>
        /// 0默认 1图片 2声音
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        [IgnoreDataMember]
        public ResourceType ResourceType
        {
            get { return (ResourceType)Type; }
            set { }
        }

        [IgnoreDataMember]
        public int Status { get; set; }

        /// <summary>
        /// 图片为 尺寸 800*600  声音为大小 单位kb
        /// </summary>
        //[DataMember(Name = "size")]
        [IgnoreDataMember]
        public string Size { get; set; }

        [DataMember(Name = "width")]
        public int Width { get; set; }

        [DataMember(Name = "height")]
        public int Height
        {
            get;
            set;
        }

        [DataMember(Name = "contentsize")]
        public int ContentSize { get; set; }

        /// <summary>
        /// 源文件扩展名
        /// </summary>
        [DataMember(Name = "extname")]
        public string ExtName { get; set; }
    }
}
