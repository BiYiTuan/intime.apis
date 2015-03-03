using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum PromotionFilterMode
    {
        Default = 0,

        /// <summary>
        /// 最新
        /// </summary>
        New = 1,

        /// <summary>
        /// 即将开始
        /// </summary>
        BeginStart = 2,

        /// <summary>
        /// 未结束
        /// </summary>
        [System.Obsolete("不建议使用")]
        NotTheEnd = 3,

        /// <summary>
        /// 进行中
        /// </summary>
        InProgress = 4
    }

    public enum PromotionSortOrder
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 最新 
        /// </summary>
        New = 1,

        /// <summary>
        /// 最热
        /// </summary>
        Hot = 2,

        /// <summary>
        /// 最近
        /// </summary>
        Near = 3,

        /// <summary>
        /// 创建时间
        /// </summary>
        CreatedDateDesc = 4,

        /// <summary>
        /// 开始时间倒序
        /// </summary>
        StartDesc = 5,

        /// <summary>
        /// 开始时间正序
        /// </summary>
        StartAsc = 6
    }
}
