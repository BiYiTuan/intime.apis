using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class GetProductInfo4PResponse : BaseResponse
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
        /// 说明
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }


        /// <summary>
        /// 销售价
        /// </summary>
        [DataMember(Name="price")]
        public decimal Price { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        [DataMember(Name = "originprice")]
        public decimal UnitPrice { get; set; }


        [DataMember(Name="salecolors")]
        public IEnumerable<SaleColorPropertyResponse> SaleColors { get; set; }

        [DataMember(Name="rmapolicy")]
        public string RMAPolicy { get; set; }
        [DataMember(Name="supportpayments")]
        public IEnumerable<PaymentResponse> SupportPayments { get; set; }
        
        [DataMember(Name = "dimension")]
        public ResourceInfoResponse DimensionResource { get; set; }

        [DataMember(Name="brandid")]
        public int BrandId { get; set; }
        [DataMember(Name = "brandname")]
        public string BrandName { get; set; }
        [DataMember(Name = "brand2name")]
        public string Brand2Name { get; set; }
        [DataMember(Name="skucode")]
        public string SkuCode { get; set; }
        [DataMember(Name = "product_type")]
        public int ProductType_I { get {
            return ProductType ?? (int)Yintai.Hangzhou.Model.Enums.ProductType.FromSystem;
        } }
        [DataMember(Name="is_online")]
        public bool IsOnline { get {
            return SaleColors != null &&
                SaleColors.Count() > 0 &&
                SaleColors.Any(s => s.Sizes != null && s.Sizes.Count() > 0);
        } }
        [IgnoreDataMember]
        public Nullable<int> ProductType { get; set; }
       
    }
    [DataContract]
    public class SaleColorPropertyResponse
    {
        [DataMember(Name="colorid")]
        public int ColorId { get; set; }
        [DataMember(Name = "colorname")]
        public string ColorName { get; set; }
        [DataMember(Name = "resource")]
        public ResourceInfoResponse Resource { get; set; }
        [DataMember(Name = "sizes")]
        public IEnumerable<SaleSizePropertyResponse> Sizes { get; set; }
    }
    [DataContract]
    public class SaleSizePropertyResponse
    {
        [DataMember(Name = "sizeid")]
        public int SizeId { get; set; }
        [DataMember(Name = "sizename")]
        public string SizeName { get; set; }
        [DataMember(Name="is4sale")]
        public bool Is4Sale { get; set; }
    }
   
    
}
