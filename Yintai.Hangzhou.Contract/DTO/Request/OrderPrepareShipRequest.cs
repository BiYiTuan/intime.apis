using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class OrderPrepareShipRequest:BaseRequest
    {
        [Required(ErrorMessage="OrderNo 必选")]
        public string OrderNo { get; set; }
        [Required(ErrorMessage = "UpdateTime 必选")]
        public DateTime UpdateTime { get; set; }
        public string Memo { get; set; }
        public int StoreId { get; set; }
    }
}
