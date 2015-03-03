using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request.Feedback
{
    public class FeedbackCreateRequest : AuthRequest
    {
        public string Content { get; set; }

        public string Contact { get; set; }
    }
}
