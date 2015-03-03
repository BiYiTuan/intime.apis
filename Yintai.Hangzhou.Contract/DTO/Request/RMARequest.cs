using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class RMAInfoRequest : BaseRequest
    {
        public string RMANo { get; set; }
    }
    public class RMARequest:BaseRequest
    {
        public string OrderNo { get; set; }
        public string Reason { get; set; }
        public int RMAReason { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankCard { get; set; }
        public string ContactPhone { get; set; }
        public string Products { get; set; }
        public IEnumerable<RMAProductDetailRequest> Products2 { get {
            if (string.IsNullOrEmpty(Products))
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<RMAProductDetailRequest>>(Products);
        } }
  
    }
    public class RMAUpdateRequest : BaseRequest
    {
        [Required]
        public string RMANo { get; set; }
        [Required]
        public string ContactPhone { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        public int ShipVia { get; set; }
        public string ShipViaNo { get; set; }
    }
     public class RMAProductDetailRequest
    {
        public int ProductId { get; set; }

        public string Desc { get; set; }
        [Required(ErrorMessage = "没有选择数量！")]
        [Range(1, 5, ErrorMessage = "数量不能超过5个")]
        public int Quantity { get; set; }

        public ProductPropertyValueRequest Properties { get; set; }
    }
}
