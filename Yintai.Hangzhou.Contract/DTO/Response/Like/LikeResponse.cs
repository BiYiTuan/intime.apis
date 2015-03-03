using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response.Like
{
    /// <summary>
    /// 喜欢(关注)
    /// </summary>
    [DataContract(Name = "like")]
    public class LikeCoutomerResponse :BaseResponse
    {
        /// <summary>
        /// like id
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// like or liked user_id
        /// </summary>
        [DataMember(Name = "user")]
        public ShowCustomerInfoResponse CustomerInfoResponse { get; set; }
    }

    /// <summary>
    /// 喜欢(关注)
    /// </summary>
    [DataContract]
    public class LikeCoutomerCollectionResponse : PagerInfoResponse
    {
        public LikeCoutomerCollectionResponse(PagerRequest request)
            : base(request)
        {
        }

        public LikeCoutomerCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "likes")]
        public List<ShowCustomerInfoResponse> LikeUsers { get; set; }
    }
}
