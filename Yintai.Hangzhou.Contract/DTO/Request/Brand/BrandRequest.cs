using System;
using System.Runtime.Serialization;
using System.Web;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Contract.DTO.Request.Brand
{
    public class BrandDetailRequest
    {
        public int BrandId { get; set; }
    }

    [DataContract]
    public abstract class BrandInfoRequest : AuthRequest
    {
        /// <summary>
        /// brandid
        /// </summary>
        public int BrandId { get; set; }

        /// <summary>
        /// brandid
        /// </summary>
        public int Id
        {
            get { return BrandId; }
            set { this.BrandId = value; }
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "englishname")]
        public string EnglishName { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "logo")]
        public string Logo { get; set; }

        [DataMember(Name = "website")]
        public string WebSite { get; set; }
    }

    public class BrandCreateRequest : BrandInfoRequest
    {
        public HttpFileCollectionBase Files { get; set; }
    }

    public class BrandUpdateRequest : BrandInfoRequest
    {
        
    }

    public class BrandLogoAddRequest : AuthRequest
    {
        /// <summary>
        /// brandid
        /// </summary>
        public int BrandId { get; set; }

        public HttpFileCollectionBase Files { get; set; }
    }

    public class BrandLogoDestroyRequest : AuthRequest
    {
        public int BrandId { get; set; }

        public int ResourceId { get; set; }
    }

    public class BrandAllRequest
    {
        public string Type { get; set; }

        public DateTime Refreshts { get; set; }

        public Timestamp Timestamp
        {
            get { return new Timestamp { Ts = Refreshts, TsType = TimestampType.New }; }
        }
    }

    public class BrandRefreshRequest
    {
        public DateTime Refreshts { get; set; }

        public Timestamp Timestamp
        {
            get { return new Timestamp { Ts = Refreshts, TsType = TimestampType.New }; }
        }
    }
}
