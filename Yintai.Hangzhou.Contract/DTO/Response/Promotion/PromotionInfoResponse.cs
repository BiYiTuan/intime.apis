using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.DTO.Response.Store;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Promotion
{
    [DataContract]
    public class PromotionCollectionResponse : PagerInfoResponse
    {
        public PromotionCollectionResponse(PagerRequest request)
            : base(request)
        {
        }

        public PromotionCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "promotions")]
        public List<PromotionInfoResponse> Promotions { get; set; }
    }

    [DataContract]
    public class PromotionInfo : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [IgnoreDataMember]
        public int CreatedUser { get; set; }
        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }
        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }
        [IgnoreDataMember]
        public int UpdatedUser { get; set; }

        [IgnoreDataMember]
        public System.DateTime StartDate { get; set; }

        [IgnoreDataMember]
        public System.DateTime EndDate { get; set; }

        [DataMember(Name = "startdate")]
        public string StartDateStr
        {
            get
            {
                return this.StartDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set { }
        }

        [DataMember(Name = "enddate")]
        public string EndDateStr
        {
            get
            {
                return this.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set { }
        }

        [IgnoreDataMember]
        public int Status { get; set; }

        [System.Obsolete("请使用RecommendUser")]
        [IgnoreDataMember]
        public int RecommendSourceId { get; set; }

        [IgnoreDataMember]
        public int RecommendSourceType { get; set; }

        [DataMember(Name = "promotionuserid")]
        public int RecommendUser { get; set; }

        [IgnoreDataMember]
        public int PromotionSourceType { get; set; }

        [DataMember(Name = "likecount")]
        public int LikeCount { get; set; }

        [DataMember(Name = "favoritecount")]
        public int FavoriteCount { get; set; }

        [DataMember(Name = "sharecount")]
        public int ShareCount { get; set; }

        [DataMember(Name = "couponcount")]
        public int InvolvedCount { get; set; }

        [DataMember(Name = "store_id")]
        public int Store_Id { get; set; }

        [DataMember(Name = "tagid")]
        public int Tag_Id { get; set; }

        [DataMember(Name = "isproductbinded")]
        public Nullable<bool> IsProdBindable { get; set; }

        [DataMember(Name = "limitcount")]
        public Nullable<int> PublicationLimit { get; set; }

        [DataMember(Name = "ispublication")]
        public bool IsPublication
        {
            get
            {
                if (PublicationLimit != null && PublicationLimit != -1)
                {
                    if (InvolvedCount >= PublicationLimit)
                    {
                        return false;
                    }
                }
                //time
                if (StartDate > DateTime.Now)
                {
                    return false;
                }

                if (EndDate < DateTime.Now)
                {
                    return false;
                }

                return true;
            }
            set { }
        }
    }

    [DataContract]
    public class PromotionInfoResponse : PromotionInfo
    {
        [DataMember(Name = "promotionuser")]
        public ShowCustomerInfoResponse ShowCustomer { get; set; }

        [DataMember(Name = "store")]
        public StoreInfoResponse StoreInfoResponse { get; set; }

        [DataMember(Name = "resources")]
        public List<ResourceInfoResponse> ResourceInfoResponses { get; set; }

        [DataMember(Name = "coupon")]
        public CouponCodeResponse CouponCodeResponse { get; set; }

        [DataMember(Name = "brandids")]
        public List<int> BrandIds { get; set; }

        /// <summary>
        /// 当前用户是否已经收藏过
        /// </summary>
        [DataMember(Name = "isfavorited")]
        public bool CurrentUserIsFavorited { get; set; }

        /// <summary>
        /// 当前用户是否已经领取优惠券
        /// </summary>
        [DataMember(Name = "isreceived")]
        public bool CurrentUserIsReceived { get; set; }
    }
}