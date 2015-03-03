using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.HotKey
{
    [DataContract]
    public class HotWordCollectionResponse : BaseResponse
    {
        [DataMember(Name = "words")]
        public List<string> Words { get; set; }

        [DataMember(Name = "brandwords")]
        public List<BrandWordsInfo> BrandWords { get; set; }
    }

    [DataContract]
    public class BrandWordsInfo
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
