using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class WxGetPay4AppTokenResponse
    {
        [DataMember(Name="partnerid")]
        public string PartnerId { get; set; }
        [DataMember(Name = "prepayid")]
        public string PrepayId { get; set; }
        [DataMember(Name = "noncestr")]
        public string NonceStr { get; set; }
        [DataMember(Name = "timestamp")]
        public long TimeStamp { get; set; }
        [DataMember(Name = "package")]
        public string Package { get; set; }
        [DataMember(Name = "sign")]
        public string Sign { get; set; }
    }
}
