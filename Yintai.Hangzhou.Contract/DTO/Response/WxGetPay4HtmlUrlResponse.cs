using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class WxGetPay4HtmlUrlResponse
    {
        [DataMember(Name = "payurl")]
        public string PayUrl { get; set; }

    }
}
