using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum CouponStatus
    {
        /// <summary>
        /// 已删除（逻辑删除）
        /// </summary>
        [Description("取消")]
        Deleted = DataStatus.Deleted,
        /// <summary>
        /// 默认状态
        /// </summary>
        [Description("默认")]
        Default = DataStatus.Default,
        /// <summary>
        /// 正常状态
        /// </summary>
        [Description("领取未使用")]
        Normal = DataStatus.Normal,
        [Description("过期")]
        Expired = 2,
        [Description("已使用")]
        Used = 10

    }
}
