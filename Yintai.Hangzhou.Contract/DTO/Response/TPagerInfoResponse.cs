using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class PagerInfoResponse<T>:PagerInfoResponse
    {
          public  PagerInfoResponse(PagerRequest request)
            : base(request)
        {
        }

        public  PagerInfoResponse(PagerRequest request, int totalCount)
            : base(request, totalCount)
        {
        }

        [DataMember(Name = "items")]
        public List<T> Items { get; set; }
    }
}
