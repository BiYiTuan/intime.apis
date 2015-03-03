using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Yintai.Hangzhou.Contract.Response;
using com.intime.fashion.common.Extension;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class IMSProductDetailResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "brand_id")]
        public int Brand_Id { get; set; }
        [DataMember(Name = "brand_name")]
        public string Brand_Name { get; set; }
        [DataMember(Name = "desc")]
        public string Description { get; set; }
        [DataMember(Name = "create_date")]
        public System.DateTime CreatedDate { get; set; }
        [DataMember(Name = "price")]
        public decimal Price { get; set; }
        [DataMember(Name = "category_id")]
        public int Tag_Id { get; set; }
        [DataMember(Name = "category_name")]
        public string Category_Name { get; set; }
        [DataMember(Name = "sku_code")]
        public string SkuCode { get; set; }
        [DataMember(Name = "product_type")]
        public Nullable<int> ProductType { get; set; }
        [DataMember(Name = "image")]
        public string Image
        {
            get
            {
                if (Images != null && Images.Count() > 0)
                    return Images.First().Url;
                return string.Empty;
            }
        }
        [DataMember(Name = "is_online")]
        public bool IsOnline { get; set; }
        [DataMember(Name = "image_id")]
        public int Image_Id
        {
            get
            {
                if (Images != null && Images.Count() > 0)
                    return Images.First().Id;
                return 0;
            }
        }
        [DataMember(Name = "unitprice")]
        public decimal UnitPrice { get; set; }
        [DataMember(Name = "images")]
        public IEnumerable<IMSSelfImageResponse> Images { get; set; }
        [DataMember(Name = "tags")]
        public IEnumerable<IMSTagResponse> IMS_Tags { get; set; }

    }

    [DataContract]
    public class IMSProductSelfDetailResponse : IMSProductDetailResponse
    {
        [DataMember(Name = "sizes")]
        public IEnumerable<IMSProductSizeResponse> Sizes { get; set; }
        [DataMember(Name = "sales_code")]
        public string SalesCode { get; set; }
        [DataMember(Name = "color_str")]
        public string ColorStr { get; set; }
        [DataMember(Name = "tags")]
        public IEnumerable<int> IMS_Tags { get; set; }

    }
    [DataContract]
    public class IMSProductSizeResponse
    {

        [DataMember(Name = "id")]
        public int SizeValueId { get; set; }
        [DataMember(Name = "name")]
        public string SizeName { get; set; }
        [DataMember(Name = "inventory")]
        public int Inventory { get; set; }
    }
    [DataContract]
    public class IMSSelfImageResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "url")]
        public string Url
        {
            get
            {
                return Name.Image320Url();
            }
        }

        [IgnoreDataMember]
        public string Name { get; set; }
    }
    [DataContract]
    public class IMSTagResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}

