using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum AssociateIncomeStatus
    {
        [Description("不可用")]
        Create = 0,
        [Description("冻结")]
        Frozen = 1,
        [Description("失效")]
        Void = 2,
        [Description("有效")]
        Avail = 3
    }
}
