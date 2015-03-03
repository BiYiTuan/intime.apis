using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum OrderOpera
    {
        FromCustomer = 0,
        FromOperator = 1,
        FromSystem = 2,
        CustomerReceived = 3,
        CustomerVoid = 4,
        EditOrder = 5,
        CustomerPay = 6,
        PrepareShip = 7,
        Shipping,
        SystemVoid,
        CustomerReject
    }
}
