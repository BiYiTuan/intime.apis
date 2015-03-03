using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum PointType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 注册
        /// </summary>
         [Description("注册")]
        Register = 1,

        /// <summary>
        /// 邀请其他好友消费
        /// </summary>
         [Description("邀请好友")]
        InviteConsumption = 2,

        /// <summary>
        /// 被邀请消费赠送
        /// </summary>
         [Description("消费赠送")]
        BeConsumption = 3,

        /// <summary>
        /// 奖励
        /// </summary>
         [Description("奖励")]
        Reward = 4,

        /// <summary>
        /// 消费 这个是扣除的 是负的
        /// </summary>
         [Description("使用积点")]
        Consumption = 5,

        [Description("作废代金券")]
        VoidCoupon = 6,
        [Description("转换到集团")]
        Convert2Group = 7
    }
}
