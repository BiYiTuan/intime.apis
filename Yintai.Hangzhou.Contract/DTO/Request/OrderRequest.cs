using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class OrderRequest : AuthRequest
    {
        public string Order { get; set; }
        public OrderRequestModel OrderModel
        {
            get
            {
                return JsonConvert.DeserializeObject<OrderRequestModel>(Order);
            }
        }
       

    }
    public class OrderRequestModel:IValidatableObject
    {
        [Required]
        public IEnumerable<OrderProductDetailRequest> Products { get; set; }
       
        
        public IEnumerable<ProductPropertyValueRequest> Properties { get; set; }
        [Required(ErrorMessage = "送货方式没有选择")]
        public ShippingAddressModel ShippingAddress { get; set; }
        [Required(ErrorMessage="支付方式没有选择")]
        public PaymentModel Payment { get; set; }
        public bool NeedInvoice { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceDetail { get; set; }
        public string Memo { get; set; }
        public int? StoreId { get; set; }
        public int? ComboId { get; set; }

        public int ShippingType { get {
            if (ShippingAddress == null ||ShippingAddress.ShippingZipCode==null)
                return (int)ShipType.Self;
            return (int)ShipType.TrdParty;
        } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NeedInvoice)
            {
                if (string.IsNullOrEmpty(InvoiceTitle))
                    yield return new ValidationResult("发票抬头不能为空！");
            }
        }

       
    }
    public class ProductPropertyValueRequest
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int? SizeValueId { get; set; }
        public string SizeValueName { get; set; }

        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int? ColorValueId { get; set; }
        public string ColorValueName { get; set; }
    }
    public class OrderProductDetailRequest
    {
        public int ProductId { get; set; }

        public string Desc { get; set; }
        [Required(ErrorMessage = "没有选择数量！")]
        [Range(1, 5, ErrorMessage = "数量不能超过5个")]
        public int Quantity { get; set; }

        public int? StoreId { get; set; }
        public int? SectionId { get; set; }

        public ProductPropertyValueRequest Properties { get; set; }

         public string ProductDesc { get {
            if (Properties == null)
                return string.Empty;
           var description = string.Format("{0}:{1},{2}:{3}", "颜色", Properties.ColorValueName,"尺码",Properties.SizeValueName)
              ;
           if (description.Length > 0)
               description = description.TrimEnd(',');
           return description;
                    
        } }
    }
}
