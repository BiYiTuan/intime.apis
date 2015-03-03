using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.SpecialTopic
{
    [DataContract]
    public class SpecialTopicCollectionResponse : PagerInfoResponse
    {
        public SpecialTopicCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "specialtopics")]
        public List<SpecialTopicInfoResponse> SpecialTopics { get; set; }
    }

    [DataContract]
    public abstract class SpecialTopicInfo : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "createddate")]
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToString(Define.DateDefaultFormat);
            }
            set { }
        }

        [DataMember(Name = "updateddate")]
        public string UpdatedDateStr
        {
            get
            {
                return this.UpdatedDate.ToString(Define.DateDefaultFormat);
            }
            set { }
        }

        /// <summary>
        /// 资源
        /// </summary>
        [DataMember(Name = "resources")]
        public List<ResourceInfoResponse> ResourceInfoResponses { get; set; }

        [IgnoreDataMember]
        public int Type { get; set; }
        [IgnoreDataMember]
        public int Status { get; set; }
        [IgnoreDataMember]
        public int CreatedUser { get; set; }
        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }
        [IgnoreDataMember]
        public int UpdatedUser { get; set; }
        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }
    }

    [DataContract]
    public class SpecialTopicInfoResponse : SpecialTopicInfo
    {
        /// <summary>
        /// 当前用户是否已经收藏过
        /// </summary>
        [DataMember(Name = "isfavorited")]
        public bool CurrentUserIsFavorited { get; set; }
    }
}
