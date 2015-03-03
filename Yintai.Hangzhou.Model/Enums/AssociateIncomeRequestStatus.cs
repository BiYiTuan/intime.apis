using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum AssociateIncomeRequestStatus
    {
        [Description("已申请")]
        Requesting = 1,

        [Description("申请已接收")]
        Transferring = 2,

        [Description("转账完成")]
        Transferred = 3,

        [Description("转账失败")]
        Failed = 4
    }
}
