using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum ProductSortOrder
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 创建时间倒序
        /// </summary>
        [Description("创建时间")]
        CreatedDateDesc = 1,
        /// <summary>
        /// 创建时间倒序
        /// </summary>
        [Description("创建用户")]
        CreatedUserDesc = 2,

         /// <summary>
        ///优先级排序
        /// </summary>
        [Description("优先级")]
        SortOrderDesc = 3,

        [Description("品牌")]
        SortByBrand = 4
    }
}
