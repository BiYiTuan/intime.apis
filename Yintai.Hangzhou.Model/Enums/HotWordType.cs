using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum HotWordType
    {
        [Description("关键词")]
        Words = 1,
        [Description("品牌")]
        BrandStruct = 2,
        [Description("门店")]
        Stores = 3
    }
}
