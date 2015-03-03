using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class StoreCouponDetailRequest:AuthRequest
    {
        public int StoreCouponId { get; set; }
    }
}
