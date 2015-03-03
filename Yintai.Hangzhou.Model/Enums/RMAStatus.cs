using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum RMAStatus
    {
        [Description("退货申请中")]
        Created = 1,
        [Description("退货审核通过")]
        PassConfirmed = 2,
        [Description("用户确认")]
        CustomerConfirmed = 3,
        [Description("退货已签收")]
        PackageReceived = 12,
        [Description("退货完成")]
        Complete = 10,
        [Description("退货审核不通过返还客户")]
        Reject2Customer = 13,
        [Description("打印退货单并退款")]
        PrintRMA = 14,
        [Description("退货取消")]
        Void = -10,
         [Description("退货审核不通过")]
        Reject = 15
    }
}
