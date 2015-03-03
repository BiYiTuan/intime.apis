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
    public class TagPropertyDetailResponse : BaseResponse
    {
        [DataMember(Name = "propertyid")]
        public int PropertyId { get; set; }
        [DataMember(Name = "propertyname")]
        public string PropertyName { get; set; }
        [DataMember(Name = "values")]
        public IEnumerable<TagPropertyValueDetailResponse> Values { get; set; }
    }
    [DataContract]
    public class TagPropertyValueDetailResponse
    {
        [DataMember(Name = "valueid")]
        public int ValueId { get; set; }
        [DataMember(Name = "valuename")]
        public string ValueName { get; set; }
    }
}
