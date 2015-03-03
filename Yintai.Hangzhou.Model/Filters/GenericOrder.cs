using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Filters
{
    public enum GenericOrder
    {
        [Description("创建时间")]
        OrderByCreateDate = 0,
        [Description("创建用户")]
        OrderByCreateUser = 1,
        [Description("名称")]
        OrderByName = 2
    }
}
