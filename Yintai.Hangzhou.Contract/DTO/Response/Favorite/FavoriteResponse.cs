using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.DTO.Response.Store;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Favorite
{
    [DataContract]
    public class FavoriteInfoResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "productid")]
        public int FavoriteSourceId { get; set; }
        [DataMember(Name = "producttype")]
        public int FavoriteSourceType { get; set; }

        [IgnoreDataMember]
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

        [IgnoreDataMember]
        public string Description { get; set; }

        [IgnoreDataMember]
        public int Status { get; set; }

        [DataMember(Name = "productname")]
        public string FavoriteSourceName { get; set; }


        [DataMember(Name = "store_id")]
        public int StoreId { get; set; }

        [DataMember(Name = "store")]
        public StoreInfoResponse Store { get; set; }

        [DataMember(Name = "resources")]
        public List<ResourceInfoResponse> Resources { get; set; }

        [DataMember(Name = "promotions")]
        public List<PromotionInfo> Promotions { get; set; }
    }

    [DataContract]
    public class FavoriteCollectionResponse : PagerInfoResponse
    {
        public FavoriteCollectionResponse(PagerRequest request)
            : base(request)
        {
        }

        public FavoriteCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "favorite")]
        public List<FavoriteInfoResponse> Favorites { get; set; }
    }
}
