using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.Brand;
using Yintai.Hangzhou.Contract.DTO.Response.Coupon;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.DTO.Response.Store;
using Yintai.Hangzhou.Contract.DTO.Response.Tag;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Product
{
    [DataContract]
    public class ProductCollectionResponse : PagerInfoResponse
    {
        public ProductCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "products")]
        public List<ProductInfoResponse> Products { get; set; }
    }

    [DataContract]
    public abstract class ProductInfo : BaseResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 品牌ID
        /// </summary>
        [DataMember(Name = "brand_id")]
        public int Brand_Id { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [DataMember(Name = "brand")]
        public BrandInfoResponse BrandInfoResponse { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }

        [DataMember(Name = "createddate")]
        public string CreatedDateStr
        {
            get { return this.CreatedDate.ToString(Define.DateDefaultFormat); }
            set { }
        }
        [IgnoreDataMember]
        public int CreatedUser { get; set; }
        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }
        [IgnoreDataMember]
        public int UpdatedUser { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        [IgnoreDataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        [DataMember(Name = "price")]
        public string PriceStr
        {
            get { return Price.ToString("F2"); }
            set { }
        }

        [DataMember(Name = "originprice")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 推荐理由
        /// </summary>
        [DataMember(Name = "recommendedreason")]
        public string RecommendedReason { get; set; }

        /// <summary>
        /// 优惠
        /// </summary>
        [DataMember(Name = "favorable")]
        public string Favorable { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [DataMember(Name = "recommenduser_id")]
        public int RecommendUser { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [DataMember(Name = "recommenduser")]
        public ShowCustomerInfoResponse RecommendUserInfoResponse { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [IgnoreDataMember]
        public int Status { get; set; }

        /// <summary>
        /// 商铺ID
        /// </summary>
        [DataMember(Name = "stroe_id")]
        public int Store_Id { get; set; }

        /// <summary>
        /// 商铺
        /// </summary>
        [DataMember(Name = "store")]
        public StoreInfoResponse StoreInfoResponse { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        [DataMember(Name = "tag_id")]
        public int Tag_Id { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [DataMember(Name = "tag")]
        public TagInfoResponse TagInfoResponse { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        [DataMember(Name = "resources")]
        public List<ResourceInfoResponse> ResourceInfoResponses { get; set; }

        [DataMember(Name = "likecount")]
        public int LikeCount { get; set; }

        [DataMember(Name = "sharecount")]
        public int ShareCount { get; set; }

        [DataMember(Name = "couponcount")]
        public int InvolvedCount { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>
        [DataMember(Name = "favoritecount")]
        public int FavoriteCount { get; set; }
    }

    [DataContract]
    public class ProductInfoResponse : ProductInfo
    {
        /// <summary>
        /// 优惠券
        /// </summary>
        [DataMember(Name = "coupon")]
        public CouponCodeResponse CouponCodeResponse { get; set; }

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

        /// <summary>
        /// 活动
        /// </summary>
        [DataMember(Name = "promotions")]
        public List<PromotionInfo> Promotions { get; set; }
    }
}