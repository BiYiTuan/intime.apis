namespace Yintai.Hangzhou.Model.Enums
{
    public enum CouponBusinessStatus
    {
        /// <summary>
        ///  
        /// </summary>
        Default = 0,

        /// <summary>
        /// 未使用 = 未过期
        /// Unused = UnExpired
        /// </summary>
        [System.Obsolete("还未启用")]
        Unused = 1,

        /// <summary>
        /// 使用的
        /// </summary>
        [System.Obsolete("还未启用")]
        Use = 2,

        /// <summary>
        /// 正常（未过期） = 使用 + 未使用 
        /// </summary>
        Normal = 3,

        /// <summary>
        /// 过期 = 未使用 
        /// </summary>
        Expired = 4,

        /// <summary>
        /// 未过期 = 未使用
        /// </summary>
        UnExpired = 5,

        /// <summary>
        /// 所有
        /// </summary>
        All = 6
    }
}