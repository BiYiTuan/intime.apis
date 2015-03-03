using System.ComponentModel;
namespace Yintai.Hangzhou.Model.Enums
{
    public enum PointSourceType
    {
         [Description("默认")]
        Default = 0,

        /// <summary>
        /// 系统
        /// </summary>
         [Description("系统产生")]
        System = 1,

        /// <summary>
        /// 店铺
        /// </summary>
         [Description("店铺")]
        Store = 2,

        /// <summary>
        /// 集团
        /// </summary>
         [Description("集团")]
        Group = 3


    }
}