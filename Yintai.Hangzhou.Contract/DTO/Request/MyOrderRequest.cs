using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class MyOrderRequest : PagerInfoRequest
    {
        public OrderRequestType Type { get; set; }
    }
    public enum OrderRequestType
    {
        WaitForPay = 0,
        OnGoing = 1,
        Complete = 2,
        Void = 3
    }
}
