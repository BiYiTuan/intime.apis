using System;
using System.Globalization;
using System.Web;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.Extension;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Promotion
{
    public class GetPromotionBannerListRequest : BaseRequest
    {
        public int Page { get; set; }

        public int Pagesize { get; set; }

        public int Sort { get; set; }

        public double Lng { get; set; }

        public double Lat { get; set; }

        public CoordinateInfo CoordinateInfo
        {
            get { return new CoordinateInfo(Lng, Lat); }
        }

        public PromotionSortOrder SortOrder
        {
            get { return EnumExtension.Parser<PromotionSortOrder>(Sort); }
        }
    }

    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.Contract.Request.Promotion
    /// FileName: GetPromotionListRequest
    ///
    /// Created at 11/12/2012 2:58:16 PM
    /// Description: 
    /// </summary>
    public class GetPromotionListRequest : BaseRequest
    {
        public int Page { get; set; }

        public int Pagesize { get; set; }

        public int Sort { get; set; }

        public double Lng { get; set; }

        public double Lat { get; set; }

        public CoordinateInfo CoordinateInfo
        {
            get { return new CoordinateInfo(Lng, Lat); }
        }

        public PromotionSortOrder SortOrder
        {
            get { return EnumExtension.Parser<PromotionSortOrder>(Sort); }
        }

        private string _ts;

        /// <summary>
        /// 刷新时间戳
        /// </summary>
        public string RefreshTs
        {
            get { return String.IsNullOrEmpty(_ts) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : _ts; }
            set { _ts = value; }
        }

        public string Type { get; set; }

    }

    public class GetPromotionListForRefresh : RefreshRequest
    {
        public int PageSize { get; set; }

        public int Sort { get; set; }

        public double Lng { get; set; }

        public double Lat { get; set; }

        public PromotionSortOrder SortOrder
        {
            get { return EnumExtension.Parser<PromotionSortOrder>(Sort); }
        }

        public CoordinateInfo CoordinateInfo
        {
            get { return new CoordinateInfo(Lng, Lat); }
        }
    }

    public abstract class PromotionInfoRequest : AuthRequest
    {
        public int Id
        {
            get { return PromotionId ?? 0; }
            set { PromotionId = value; }
        }

        public int? PromotionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? Brandid { get; set; }

        /// <summary>
        /// ,
        /// </summary>
        public string BrandIds { get; set; }

        public int? StoreId { get; set; }

        public int? TagId { get; set; }

        public int? RecommendUser { get; set; }

        public int? RecommendSourceId
        {
            get { return RecommendUser; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] Brands
        {
            get
            {
                if (String.IsNullOrWhiteSpace(BrandIds))
                {
                    if (Brandid == null)
                    {
                        return new int[0];
                    }
                    else
                    {
                        BrandIds = Brandid.Value.ToString(CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    if (Brandid != null)
                    {
                        BrandIds = BrandIds + "," + Brandid.Value.ToString(CultureInfo.InvariantCulture);
                    }
                }

                var ts = BrandIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (ts.Length == 0)
                {
                    return new int[0];
                }

                var list = new int[ts.Length];

                for (var i = 0; i < ts.Length; i++)
                {
                    list[i] = Int32.Parse(ts[i]);
                }

                return list;
            }
            set { }
        }
    }

    public class UpdatePromotionRequest : PromotionInfoRequest
    {
    }

    public class CreatePromotionRequest : PromotionInfoRequest
    {
        public HttpFileCollectionBase Files { get; set; }
    }

    public class DestroyPromotionRequest : AuthRequest
    {
        public int PromotionId { get; set; }
    }

    public class CreateResourcePromotionRequest : AuthRequest
    {
        public int PromotionId { get; set; }

        public HttpFileCollectionBase Files { get; set; }

        public int DefaultNum { get; set; }
    }

    public class DestroyResourcePromotionRequest : AuthRequest
    {
        public int PromotionId { get; set; }

        public int ResourceId { get; set; }
    }

    public class GetPromotionInfoRequest : BaseRequest
    {
        public int Promotionid { get; set; }

        public double Lng { get; set; }

        public double Lat { get; set; }

        public CoordinateInfo CoordinateInfo
        {
            get { return new CoordinateInfo(Lng, Lat); }
        }

        /// <summary>
        /// 当前请求的用户，可以是匿名的
        /// </summary>
        public UserModel CurrentAuthUser { get; set; }
    }

    public class PromotionShareCreateRequest : CoordinateRequest
    {
        public string Description { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 1新浪微博 2腾讯 3微信
        /// </summary>
        public int OutSiteType { get; set; }

        public int Promotionid { get; set; }
    }

    public abstract class PromotionFavorRequest : CoordinateRequest
    {
        public int Promotionid { get; set; }
    }

    public class PromotionFavorCreateRequest : PromotionFavorRequest
    {
        public PromotionFavorCreateRequest()
        {
        }

        public PromotionFavorCreateRequest(PromotionFavorRequest r)
        {
            this.AuthUid = r.AuthUid;
            this.AuthUser = r.AuthUser;
            this.Method = r.Method;
            this.Promotionid = r.Promotionid;

            this.Lat = r.Lat;
            this.Lng = r.Lng;
        }
    }

    public class PromotionFavorDestroyRequest : PromotionFavorRequest
    {
        public PromotionFavorDestroyRequest()
        {
        }

        public PromotionFavorDestroyRequest(PromotionFavorRequest r)
        {
            this.AuthUid = r.AuthUid;
            this.AuthUser = r.AuthUser;
            this.Method = r.Method;
            this.Promotionid = r.Promotionid;
            this.Lat = r.Lat;
            this.Lng = r.Lng;
        }
    }

    public class PromotionCouponCreateRequest : AuthRequest
    {
        public int PromotionId { get; set; }
        public int IsPass { get; set; }
    }
}
