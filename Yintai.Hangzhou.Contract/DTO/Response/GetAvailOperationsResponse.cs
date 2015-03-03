using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class GetAvailOperationsResponse:BaseResponse
    {
        [DataMember(Name = "isfavored")]
        public bool IsFavored { get; set; }
        [DataMember(Name="ifcancoupon")]
        public bool IfCanCoupon { get; set; }
        [DataMember(Name="ifcantalk")]
        public bool IfCanTalk { get; set; }
        [DataMember(Name = "is4sale")]
        public bool Is4Sale { get; set; }
    }
}
