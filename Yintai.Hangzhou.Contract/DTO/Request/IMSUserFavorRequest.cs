using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class IMSUserFavorRequest:BaseRequest
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public int StoreId { get; set; }
    }
}
