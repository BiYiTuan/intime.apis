using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Mvc;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
   public  class IMSProductCreateRequest:BaseRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "品牌必填")]
        public int Brand_Id { get; set; }
        [Required(ErrorMessage="销售码为空")]
        [MinLength(2,ErrorMessage="销售码长度不正确")]
        public string Sales_Code { get; set;}
        [Required(ErrorMessage = "货号为空")]
        [MinLength(2, ErrorMessage = "货号长度不正确")]
        public string Sku_Code { get; set; }
        [Required(ErrorMessage = "价格必填")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "分类必填")]
        public int Category_Id { get; set; }
        public SizePropertyValue[] Sizes { get; set; }
        public decimal? UnitPrice { get; set; }

        public int[] Image_Ids { get; set; }
        public string Color_Str { get; set; }
        public string Desc { get; set; }
        public bool CreateCombo { get; set; }
        public int[] Tag_Ids { get; set; }
        public string Name { get; set; }
    }
   public class SizePropertyValue
   {
       public int? Id { get; set; }
       [Required(ErrorMessage = "尺码为空")]
       public string Name { get; set; }
       
       public int Inventory { get; set; }
   }
}
