using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Comment
{
    [DataContract]
    public class CommentCollectionResponse : PagerInfoResponse
    {
        public CommentCollectionResponse(PagerRequest pagerRequest, int totalCount)
            : base(pagerRequest, totalCount)
        {
        }

        public CommentCollectionResponse(PagerRequest pagerRequest)
            : base(pagerRequest)
        {
        }

        [DataMember(Name = "comments")]
        public List<CommentInfoResponse> Comments { get; set; }
    }

    [DataContract]
    public class CommentInfoResponse : BaseResponse
    {
        [DataMember(Name = "commentid")]
        public int Id { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }
        [IgnoreDataMember]
        public int CreatedUser { get; set; }

        [DataMember(Name = "createddate")]
        public string CreatedDateStr
        {
            get { return this.CreatedDate.ToString(Define.DateDefaultFormat); }
            set { }
        }

        [IgnoreDataMember]
        public System.DateTime CreatedDate { get; set; }
        [IgnoreDataMember]
        public int UpdatedUser { get; set; }
        [IgnoreDataMember]
        public System.DateTime UpdatedDate { get; set; }
        [IgnoreDataMember]
        public int Status { get; set; }

        [DataMember(Name = "customer_id")]
        public int User_Id { get; set; }

        [DataMember(Name = "customer")]
        public ShowCustomerInfoResponse Customer { get; set; }

        [DataMember(Name = "sourceid")]
        public int SourceId { get; set; }

        [DataMember(Name = "sourcetype")]
        public int SourceType { get; set; }

        [DataMember(Name = "replycustomer_id")]
        public int ReplyUser { get; set; }

        [DataMember(Name = "replycustomer_nickname")]
        public string ReplyUserNickname
        {
            get;
            set;
        }

        /// <summary>
        /// 资源
        /// </summary>
        [DataMember(Name = "resources")]
        public List<ResourceInfoResponse> ResourceInfoResponses { get; set; }
    }
}
