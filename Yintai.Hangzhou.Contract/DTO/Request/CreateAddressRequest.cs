using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class CreateAddressRequest:BaseRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage="联系人不能为空")]
        [StringLength(10,ErrorMessage="联系人长度不能超过10")]
        public string ShippingContactPerson { get; set; }
        [Required(ErrorMessage = "联系人地址")]
        [StringLength(500, ErrorMessage = "联系人长度不能超过500")]
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "联系人号码不能为空")]
        [StringLength(20, ErrorMessage = "联系人长度不能超过20")]
        public string ShippingContactPhone { get; set; }
        [Required(ErrorMessage = "邮编不能为空")]
        public string ShippingZipCode { get; set; }
        [Required(ErrorMessage = "地址省不能为空")]
        public string ShippingProvince { get; set; }
        [Required(ErrorMessage="地址省份不能为空")]
        public int ShippingProvinceId { get; set; }
        [Required(ErrorMessage ="地址城市不能为空")]
        public string ShippingCity { get; set; }
        [Required(ErrorMessage = "地址城市不能为空")]
        public int? ShippingCityId { get; set; }
        [Required(ErrorMessage = "地址地区不能为空")]
        public int? ShippingDistrictId { get; set; }
         [Required(ErrorMessage = "地址地区不能为空")]
        public string ShippingDistrict { get; set; }

       
    }
}
