using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class GetConversationRequest:PagerInfoRequest
    {
        public int UserId { get; set; }
        public int LastConversationId { get; set; }
    }
}
