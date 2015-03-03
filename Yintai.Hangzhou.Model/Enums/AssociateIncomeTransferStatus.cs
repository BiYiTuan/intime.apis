using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum AssociateIncomeTransferStatus
    {
        [Description("已申请")]
        NotStart = 0,
         [Description("申请已接收")]
        RequestSent = 1,
         [Description("转账完成")]
        Complete = 2,
         [Description("转账失败")]
        Fail = 3
    }
}
