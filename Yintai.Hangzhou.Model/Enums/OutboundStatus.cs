using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum OutboundStatus
    {
        [Description("已创建")]
        Created = 1,
        [Description("已发货")]
        Shipped =2,
        [Description("取消")]
        Void = -10
    }
}
