using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class NewPMessageRequest:BaseRequest
    {
        public int ToUser { get; set; }
        [Required(ErrorMessage="不能发空信息")]
        [StringLength(500, ErrorMessage = "联系人长度不能超过500")]
        public string TextMsg { get; set; }

    }
}
