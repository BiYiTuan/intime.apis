using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Card
{
    public class BindingRequest : AuthRequest
    {
        public string CardNo { get; set; }

        public string Password { get; set; }
    }

    public class GetCardInfoRequest : AuthRequest
    {
        public string CardNo { get; set; }
    }
}
