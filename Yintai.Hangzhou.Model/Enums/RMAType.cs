using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum RMAType
    {
        [Description("线上申请")]
        FromOnline =1,
        [Description("现场申请")]
        FromOffline =2
    }
}
