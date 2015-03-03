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
    public class GetMessageDetailReponse:BaseResponse
    {
        [DataMember(Name="key")]
        public string Key { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
