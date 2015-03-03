using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class SelfAddressResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "userid")]
        public int UserId { get; set; }
        [DataMember(Name = "shippingzipcode")]
        public string ShippingZipCode { get; set; }
        [DataMember(Name = "shippingaddress")]
        public string ShippingAddress1 { get; set; }
        [DataMember(Name = "shippingperson")]
        public string ShippingContactPerson { get; set; }
        [DataMember(Name = "shippingphone")]
        public string ShippingContactPhone { get; set; }
        [DataMember(Name = "shippingprovinceid")]
        public int ShippingProvinceId { get; set; }
        [DataMember(Name = "shippingprovince")]
        public string ShippingProvince { get; set; }
        [DataMember(Name = "shippingcityid")]
        public int? ShippingCityId { get; set; }
        [DataMember(Name = "shippingcity")]
        public string ShippingCity { get; set; }
        [DataMember(Name = "shippingdistrictid")]
        public int ShippingDistrictId { get; set; }
        [DataMember(Name = "shippingdistrict")]
        public string ShippingDistrictName { get; set; }
        [DataMember(Name = "displayaddress")]
        public string DisplayAddress { get {
            return string.Concat(ShippingProvince ?? string.Empty, ShippingCity ?? string.Empty,ShippingDistrictName??string.Empty, ShippingAddress1);
        }
            set { }
        }

    }
}
