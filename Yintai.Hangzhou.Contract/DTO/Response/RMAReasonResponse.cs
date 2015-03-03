using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class RMAReasonResponse
    {
        [DataMember(Name="reason")]
        public string Reason { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
