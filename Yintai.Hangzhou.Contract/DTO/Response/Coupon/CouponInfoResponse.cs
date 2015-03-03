using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.Product;
using Yintai.Hangzhou.Contract.DTO.Response.Promotion;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Contract.Response.Promotion;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Response.Coupon
{
    [DataContract]
    public class CouponCodeCollectionResponse : PagerInfoResponse
    {
        public CouponCodeCollectionResponse(PagerRequest request)
            : base(request)
        {
        }

        public CouponCodeCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "couponcodes")]
        public List<CouponCodeResponse> CouponCodeResponses { get; set; }
    }

    [DataContract]
    public class CouponInfoCollectionResponse : PagerInfoResponse
    {
        public CouponInfoCollectionResponse(PagerRequest request)
            : base(request)
        {
        }

        public CouponInfoCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "couponcodes")]
        public List<CouponInfoResponse> CouponInfoResponses { get; set; }
    }

    /// <summary>
    /// coupon 详情，在返回详情中使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class CouponInfoResponse : CouponResponse
    {
        //[DataMember(Name = "startdate")]
        //public string StartDateStr {
        //    get { return StartDate.ToString(Define.DateDefaultFormat); }
        //    set { } 
        //}

        //[IgnoreDataMember]
        //public DateTime StartDate { get; set; }

        //[DataMember(Name = "enddate")]
        //public string EndDateStr
        //{
        //    get { return EndDate.ToString(Define.DateDefaultFormat); }
        //    set { }
        //}

        //[IgnoreDataMember]
        //public DateTime EndDate { get; set; }

        [DataMember(Name = "product")]
        public ProductInfoResponse ProductInfoResponse { get; set; }

        [DataMember(Name = "promotion")]
        public PromotionInfoResponse PromotionInfoResponse { get; set; }
    }

    [DataContract]
    public abstract class CouponResponse : BaseResponse
    {
        [DataMember(Name = "code")]
        public string CouponId { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "userid")]
        public int User_Id { get; set; }

        [IgnoreDataMember]
        public int CreatedUser { get; set; }

        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }

        [DataMember(Name = "createddate")]
        public string CreatedDateStr
        {
            get { return this.CreatedDate.ToString(Define.DateDefaultFormat); }
            set { }
        }

        [DataMember(Name = "sourcetype")]
        public int SourceType
        {
            get { return (int)ProductType; }
            set {  }
        }

        [IgnoreDataMember]
        public SourceType Stype { get; set; }

        [IgnoreDataMember]
        public int Status { get; set; }

        [DataMember(Name = "status")]
        public int Status_s
        {
            get
            {
                if (Status == (int)CouponStatus.Normal
                    && ValidEndDate < DateTime.Now)
                    return (int)CouponStatus.Expired;
                else
                    return Status;
            }
            set { }
        }

        [DataMember(Name = "stroe_id")]
        public int FromStore { get; set; }

        //[DataMember(Name = "stroe")]
        //public StoreInfoResponse Store { get; set; }

        [DataMember(Name = "showcustomer_id")]
        public int FromUser { get; set; }

        //[DataMember(Name = "showcustomer")]
        //public ShowCustomerInfoResponse ShowCustomerInfoResponse { get; set; }

        [IgnoreDataMember]
        //[DataMember(Name = "product_id")]
        public int FromProduct { get; set; }

        //[IgnoreDataMember]
        ////[DataMember(Name = "product")]
        //public ProductInfoResponse ProductInfoResponse { get; set; }

        [IgnoreDataMember]
        //[DataMember(Name = "promotion_id")]
        public int FromPromotion { get; set; }

        [DataMember(Name = "productid")]
        public int ProductId { get; set; }

        [DataMember(Name = "productname")]
        public string ProductName { get; set; }

        [DataMember(Name = "productdescription")]
        public string ProductDescription { get; set; }

        [DataMember(Name = "producttype")]
        public int ProductType { get; set; }

        [IgnoreDataMember]
        public System.DateTime ValidStartDate { get; set; }

        [IgnoreDataMember]
        public System.DateTime ValidEndDate { get; set; }

        [DataMember(Name = "validstartdate")]
        public string ValidStartDateStr
        {
            get { return ValidStartDate.ToString(Define.DateDefaultFormat); }
            set { }
        }

        [DataMember(Name = "validenddate")]
        public string ValidEndDateStr
        {
            get { return ValidEndDate.ToString(Define.DateDefaultFormat); }
            set { }
        }
    }

    /// <summary>
    /// coupon code 包含在 source中使用
    /// </summary>
    [DataContract]
    public class CouponCodeResponse : CouponResponse
    {
        [IgnoreDataMember]
        public byte[] Pass { get; set; }

        [DataMember(Name = "pass")]
        public string PassStr
        {
            get { return Pass == null || Pass.Length == 0 ? String.Empty : Convert.ToBase64String(Pass); }
            set { }
        }
    }
}
