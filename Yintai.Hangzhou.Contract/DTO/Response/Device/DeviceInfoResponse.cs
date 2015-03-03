using System.Runtime.Serialization;

namespace Yintai.Hangzhou.Contract.DTO.Response.Device
{
    [DataContract]
    public class DeviceInfoResponse
    {
        [IgnoreDataMember]
        public int Id { get; set; }
        [IgnoreDataMember]
        public int User_Id { get; set; }
        [DataMember(Name = "devicetoken")]
        public string Token { get; set; }
        [IgnoreDataMember]
        public int Type { get; set; }
        [IgnoreDataMember]
        public int Status { get; set; }
        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }
        [IgnoreDataMember]
        public int CreatedUser { get; set; }
        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }
        [IgnoreDataMember]
        public int UpdatedUser { get; set; }
    }
}
