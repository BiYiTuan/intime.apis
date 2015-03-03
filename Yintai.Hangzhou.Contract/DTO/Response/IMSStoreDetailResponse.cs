using com.intime.fashion.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.Response;
using com.intime.fashion.common.Extension;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class IMSStoreDetailResponse:BaseResponse
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name="mobile")]
        public string Mobile { get; set; }
        [IgnoreDataMember]
        public string Logo { get; set; }
        [DataMember(Name="logo")]
        public string Logo_Absolute { get {
            if (string.IsNullOrEmpty(Logo))
                return string.Empty;
            if (Logo.StartsWith("http://"))
                return Logo;
            return string.Concat(ConfigManager.GetHttpApiImagePath(),
                    Logo,"_100x100.jpg");
        } }
        [DataMember(Name="gift_card")]
        public IEnumerable<IMSGiftCard> GiftCardSaling { get; set; }
        [DataMember(Name="combos")]
        public IEnumerable<IMSCombo> ComboSaling { get; set; }

        [DataMember(Name = "is_owner")]
        public bool Is_Owner { get; set; }
        [DataMember(Name = "is_favored")]
        public bool Is_Favored { get; set; }
        [DataMember(Name="template_id")]
        public int Template_Id { get; set; }
        [DataMember(Name="is_support_giftcard")]
        public bool IsSupportGiftCard { get; set; }
    }
    [DataContract]
    public class IMSGiftCard
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name = "desc")]
        public string Desc { get; set; }
        [IgnoreDataMember]
        public string ImageUrl { get; set; }
        [DataMember(Name="image")]
        public string Image { get {
            return ImageUrl.Image320Url();
        } }
    }
    [DataContract]
    public class IMSCombo
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name = "desc")]
        public string Desc { get; set; }
        [DataMember(Name = "unit_price")]
        public decimal Price { get; set; }
        [DataMember(Name = "discount")]
        public decimal Discount
        {
            get
            {
                return (IsInPromotion.HasValue && IsInPromotion.Value) ? DiscountAmount.Value : 0m;

            }
        }
        [DataMember(Name = "price")]
        public decimal ActualPrice
        {
            get
            {
                return Price - Discount;
            }
        }
        [DataMember(Name="image")]
        public string Image { get {
            return ImageUrl.Image320Url();
        } }
        [DataMember(Name="product_images")]
        public IEnumerable<string> ProductImages { get {
            if (ProductImageUrls == null)
                return null;
            return ProductImageUrls.Select(i => i.Image160Url());
        } }
        [DataMember(Name = "expire_in")]
        public int? ExpiredIn
        {
            get
            {
                if (!ExpireDate.HasValue)
                    return null;
                var expireSpan = (int)((ExpireDate.Value - DateTime.Now).TotalSeconds);
                return expireSpan < 0 ? 0 : expireSpan;
            }
        }
        [DataMember(Name="tags")]
        public IEnumerable<IMSTagResponse> Tags { get; set; }
        [DataMember(Name="brands")]
        public IEnumerable<dynamic> Brands { get; set; }
        [IgnoreDataMember]
        public string ImageUrl { get; set; }
        [IgnoreDataMember]
        public IEnumerable<string> ProductImageUrls { get; set; }
        [IgnoreDataMember]
        public DateTime? ExpireDate { get; set; }
        [IgnoreDataMember]
        public Nullable<bool> IsInPromotion { get; set; }
        [IgnoreDataMember]
        public Nullable<decimal> DiscountAmount { get; set; }


    }
}
