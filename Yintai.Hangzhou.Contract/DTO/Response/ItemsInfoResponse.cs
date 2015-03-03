using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.DTO.Response.Store;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    /// <summary>
    /// 产品综合（包括 产品+活动）
    /// </summary>
    [DataContract]
    public class ItemsInfoResponse : BaseResponse
    {
        [DataMember(Name = "productid")]
        public int Id { get; set; }

        [DataMember(Name = "producttype")]
        public int SourceType
        {
            get { return (int)SType; }
            set { SType = (SourceType)value; }
        }

        [IgnoreDataMember]
        public SourceType SType { get; set; }

        [DataMember(Name = "productname")]
        public string Name { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }


        [DataMember(Name = "originprice")]
        public decimal UnitPrice { get; set; }
        [DataMember(Name="likecount")]
        public int LikeCount { get; set; }

        [DataMember(Name = "store_id")]
        public int Store_Id { get; set; }

        [DataMember(Name = "store")]
        public StoreInfoResponse Store { get; set; }

        [DataMember(Name = "resources")]
        public List<ResourceInfoResponse> Resources { get; set; }

        /// <summary>
        /// 所有人
        /// </summary>
        [DataMember(Name = "productuser")]
        public int User_Id { get; set; }

        [IgnoreDataMember]
        public int CreatedUser { get; set; }

        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }

        [DataMember(Name = "createddate")]
        public string CreatedDateStr
        {
            get { return CreatedDate.ToString(Define.DateDefaultFormat); }
            set { }
        }

        [DataMember(Name = "promotions")]
        public List<PromotionInfo> Promotions { get; set; }
    }
}