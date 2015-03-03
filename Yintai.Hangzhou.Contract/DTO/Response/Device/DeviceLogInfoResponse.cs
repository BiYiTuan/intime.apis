using System.Runtime.Serialization;

namespace Yintai.Hangzhou.Contract.DTO.Response.Device
{
    [DataContract]
    public class DeviceLogInfoResponse
    {
        [IgnoreDataMember]
        public int Id { get; set; }
        [IgnoreDataMember]
        public int User_Id { get; set; }
        [DataMember(Name = "devicetoken")]
        public string DeviceToken { get; set; }
        [IgnoreDataMember]
        public int Type { get; set; }

        [DataMember(Name = "lng")]
        public decimal Longitude { get; set; }

        [DataMember(Name = "lat")]
        public decimal Latitude { get; set; }
        [DataMember(Name = "uid")]
        public string DeviceUid { get; set; }

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