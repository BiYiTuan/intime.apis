using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum GenderType
    {
        [Description("保密")]
        Default = 0,

        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male = 1,

        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female = 2
    }
}
