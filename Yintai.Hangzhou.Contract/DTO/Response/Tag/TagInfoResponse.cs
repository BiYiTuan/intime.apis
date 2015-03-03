using System.Runtime.Serialization;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Tag
{
    [DataContract]
    public class TagInfoResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "sortorder")]
        public int SortOrder { get; set; }
    }
}
