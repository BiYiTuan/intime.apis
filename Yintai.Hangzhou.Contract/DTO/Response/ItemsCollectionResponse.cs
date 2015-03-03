using System.Collections.Generic;
using System.Runtime.Serialization;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class ItemsCollectionResponse : PagerInfoResponse
    {
        public ItemsCollectionResponse(PagerRequest request)
            : base(request)
        {
        }

        public ItemsCollectionResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "items")]
        public List<ItemsInfoResponse> Items { get; set; }
    }
}