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
    public class PaymentResponse:BaseResponse
    {
        [DataMember(Name="code")]
        public string Code { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "iscod")]
        public bool IsCOD { get; set; }
        [DataMember(Name="supportmobile")]
        public bool MobAvail { get {
            if (!AvailChannels.HasValue)
                return true;
            return (AvailChannels.Value & 1)==1;
        } }
              [DataMember(Name = "supportpc")]
        public bool PcAvail
        {
            get
            {
                if (!AvailChannels.HasValue)
                    return true;
                return (AvailChannels.Value & 2) == 2;
            }
        }
        [IgnoreDataMember]
        public int? AvailChannels { get; set; }
    }
}
