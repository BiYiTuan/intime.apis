using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class NewPMessageResponse:BaseResponse
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
         [DataMember(Name = "isvoice")]
        public bool IsVoice { get; set; }
         [DataMember(Name = "msg")]
        public string TextMsg { get; set; }
        [DataMember(Name = "createdate")]
        public System.DateTime CreateDate { get; set; }
        [IgnoreDataMember]
        public int FromUser { get; set; }
        [DataMember(Name="fromuser")]
        public UserInfoResponse FromUserModel { get; set; }
        [IgnoreDataMember]
        public int ToUser { get; set; }
        [DataMember(Name = "touser")]
        public UserInfoResponse ToUserModel { get; set; }
         [DataMember(Name = "isauto")]
        public bool IsAuto { get; set; }
    }
}
