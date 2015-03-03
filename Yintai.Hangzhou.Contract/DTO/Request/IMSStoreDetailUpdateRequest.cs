using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class IMSStoreDetailUpdateRequest:BaseRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
