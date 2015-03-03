using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class RMAConfirmRequest:BaseRequest
    {
        [Required(ErrorMessage="RMANo 必须")]
        public string RMANo { get; set; }
        public string MailAddress { get; set; }
        [Required(ErrorMessage = "UpdatTime 必须")]
        public DateTime UpdateTime { get; set; }
        public string Memo { get; set; }
        public bool IsPass { get; set; }
    }
}
