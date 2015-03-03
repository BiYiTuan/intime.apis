using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Product
{
    public class GetProductListRequest : ListRequest
    {
        public string Type { get; set; }

        public int Sort { get; set; }

        public ProductSortOrder ProductSortOrder
        {
            get { return (ProductSortOrder)Sort; }
            set { Sort = (int)value; }
        }

        [DataMember(Name = "lng")]
        public double Lng { get; set; }

        [DataMember(Name = "lat")]
        public double Lat { get; set; }

        private int? _tagId;

        /// <summary>
        /// 专题
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public int? PromotionId { get; set; }

        /// <summary>
        /// 当小于1时 默认为全部
        /// </summary>
        [DataMember(Name = "tagid")]
        public int? TagId
        {
            get { return _tagId; }
            set
            {
                _tagId = value < 1 ? null : value;
            }
        }

        private int? _brandId;

        public int? BrandId
        {
            get { return _brandId; }
            set
            {
                _brandId = value < 1 ? null : value;
            }
        }

        [IgnoreDataMember]
        public CoordinateInfo CoordinateInfo
        {
            get
            {
                if (Lng > 0 || Lng < 0)
                {
                    if (Lat > 0 || Lat < 0)
                    {
                        return null;
                    }
                }

                return new CoordinateInfo(Lng, Lat);
            }
        }

        /// <summary>
        /// 达人用户Id
        /// </summary>
        public UserModel UserModel { get; set; }
    }

    public class GetProductRefreshRequest : RefreshRequest
    {
        [DataMember(Name = "lng")]
        public double Lng { get; set; }

        [DataMember(Name = "lat")]
        public double Lat { get; set; }

        private int? _tagId;

        /// <summary>
        /// 当小于1时 默认为全部
        /// </summary>
        [DataMember(Name = "tagid")]
        public int? TagId
        {
            get { return _tagId; }
            set
            {
                _tagId = value < 1 ? null : value;
            }
        }

        private int? _brandId;

        public int? BrandId
        {
            get { return _brandId; }
            set
            {
                _brandId = value < 1 ? null : value;
            }
        }

        public int Sort { get; set; }

        /// <summary>
        /// 专题
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public int? PromotionId { get; set; }

        public ProductSortOrder ProductSortOrder
        {
            get { return (ProductSortOrder)Sort; }
            set { Sort = (int)value; }
        }

        [IgnoreDataMember]
        public CoordinateInfo CoordinateInfo
        {
            get
            {
                if (Lng > 0 || Lng < 0)
                {
                    if (Lat > 0 || Lat < 0)
                    {
                        return null;
                    }
                }

                return new CoordinateInfo(Lng, Lat);
            }
        }
    }

    public class GetProductInfoRequest : BaseRequest
    {
        public int ProductId { get; set; }

        [DataMember(Name = "lng")]
        public double Lng { get; set; }

        [DataMember(Name = "lat")]
        public double Lat { get; set; }

        [IgnoreDataMember]
        public CoordinateInfo CoordinateInfo
        {
            get
            {
                if (Lng > 0 || Lng < 0)
                {
                    if (Lat > 0 || Lat < 0)
                    {
                        return null;
                    }
                }

                return new CoordinateInfo(Lng, Lat);
            }
        }

        /// <summary>
        /// 当前请求的用户，可以是匿名的
        /// </summary>
        public UserModel CurrentAuthUser { get; set; }
    }

    [DataContract]
    public class ProductInfoRequest : AuthRequest
    {
        [DataMember]
        public int Id
        {
            get { return ProductId; }
            set { ProductId = value; }
        }

        public int ProductId { get; set; }


        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int BrandId { get; set; }

        [IgnoreDataMember]
        public int Brand_Id
        {
            get { return BrandId; }
            set { BrandId = value; }
        }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// 推荐理由
        /// </summary>
        [DataMember]
        public string Reason { get; set; }

        [IgnoreDataMember]
        public string RecommendedReason
        {
            get { return Reason; }
            set { Reason = value; }
        }

        [DataMember]
        public int FavoriteCount { get; set; }
        //优惠
        [DataMember]
        public string Favorable { get; set; }

        [DataMember]
        public int RecommendUser { get; set; }

        [DataMember]
        public int RecommendSourceType { get; set; }

        [IgnoreDataMember]
        public RecommendSourceType RSourceType
        {
            get { return (RecommendSourceType)RecommendSourceType; }
            set { this.RecommendSourceType = (int)value; }
        }

        [DataMember]
        public int RecommendSourceId { get; set; }

        [IgnoreDataMember]
        public int Store_Id
        {
            get { return StoreId; }
            set { StoreId = value; }
        }
        [DataMember]
        public int StoreId { get; set; }

        [DataMember]
        public int Tag_Id
        {
            get { return TagId; }
            set { TagId = value; }
        }

        [DataMember]
        public int TagId { get; set; }

        public bool IsHasImage { get; set; }

        public int SortOrder { get; set; }
    }

    [DataContract]
    public class CreateProductRequest : ProductInfoRequest
    {
        public HttpFileCollectionBase Files { get; set; }
        public string Property { get; set; }
        public string Dimension { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool? Is4Sale { get; set; }
        public string UPCCode { get; set; }
        public IEnumerable<TagPropertyModel> PropertyModel
        {
            get
            {
                if (string.IsNullOrEmpty(Property))
                    return null;
                return JsonConvert.DeserializeObject<IEnumerable<TagPropertyModel>>(Property);
            }
        }
    }

    [DataContract]
    public class UpdateProductRequest : ProductInfoRequest
    {
    }

    public class DestroyProductRequest : AuthRequest
    {
        public int ProductId { get; set; }
    }

    public class CreateResourceProductRequest : AuthRequest
    {
        public int ProductId { get; set; }

        public int DefaultNum { get; set; }

        public HttpFileCollectionBase Files { get; set; }
    }

    public class DestroyResourceProductRequest : AuthRequest
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public int ResourceId { get; set; }
    }

    public class CreateShareProductRequest : AuthRequest
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 分享理由
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 分享名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 外站类型
        /// </summary>
        public int Outsitetype { get; set; }

        /// <summary>
        /// 外站类型
        /// </summary>
        public OutsiteType OsiteType
        {
            get { return (OutsiteType)Outsitetype; }
            set { }
        }
    }

    public abstract class FavorProductRequest : AuthRequest
    {
        public int ProductId { get; set; }
    }

    public class CreateFavorProductRequest : FavorProductRequest
    {
        public CreateFavorProductRequest()
        {
        }

        public CreateFavorProductRequest(FavorProductRequest r)
        {
            this.ProductId = r.ProductId;
            this.Method = r.Method;
            this.AuthUid = r.AuthUid;
            this.AuthUser = r.AuthUser;
            this.Token = r.Token;
        }
    }

    public class DestroyFavorProductRequest : FavorProductRequest
    {
        public DestroyFavorProductRequest()
        {
        }

        public DestroyFavorProductRequest(FavorProductRequest r)
        {
            this.ProductId = r.ProductId;
            this.Method = r.Method;
            this.AuthUid = r.AuthUid;
            this.AuthUser = r.AuthUser;
            this.Token = r.Token;
        }
    }

    public class CreateCouponProductRequest : AuthRequest
    {
        public int ProductId { get; set; }
        public int? PromotionId { get; set; }
        public int IsPass { get; set; }
    }

    public class SearchProductRequest : ListRequest
    {
        public string K { get; set; }

        public int? T { get; set; }

        public int? Sort { get; set; }

        public ProductSortOrder SortOrder
        {
            get { return Sort == null ? ProductSortOrder.Default : (ProductSortOrder)Sort; }
            set { }
        }
    }
}
