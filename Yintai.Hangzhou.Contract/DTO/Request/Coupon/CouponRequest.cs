using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Coupon
{
    public class CouponCouponRequest : AuthRequest
    {
        public int ProductId { get; set; }

        public int PromotionId { get; set; }

        public int SourceType { get; set; }

        public SourceType SType { get { return (SourceType)SourceType; } set { } }
    }

    [System.Obsolete("该对象已经过期，请使用CouponInfoGetRequest")]
    public class CouponGetRequest : AuthRequest
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }
    }

    public class CouponInfoGetRequest : AuthRequest
    {
        public int CouponId { get; set; }

        public string CouponCode { get; set; }
    }

    public class CouponInfoGetListRequest : AuthPagerInfoRequest
    {
        public int? Sort { get; set; }
        [DataMember(Name="type")]
        public CouponRequestType? Type { get; set; }
        public CouponSortOrder CouponSortOrder
        {
            get { return (CouponSortOrder)(Sort == null ? 0 : Sort.Value); }
        }
    }
    [System.Obsolete("该对象已经过期，请使用CouponInfoGetListRequest")]
    public class CustomerCouponCodeGetListRequest : AuthPagerInfoRequest
    {
        public int? Sort { get; set; }

        public CouponSortOrder CouponSortOrder
        {
            get { return (CouponSortOrder)(Sort == null ? 0 : Sort.Value); }
        }
    }
}
