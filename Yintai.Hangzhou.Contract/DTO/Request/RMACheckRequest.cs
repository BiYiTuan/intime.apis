using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class RMACompleteRequest:BaseRequest
    {
        public string RMANo { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
