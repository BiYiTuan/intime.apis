using System.ComponentModel;
namespace Yintai.Hangzhou.Model.Enums
{
    public enum SourceType
    {
        [Description("不明")]
        Default = 0,

        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product = 1,

        /// <summary>
        /// 活动
        /// </summary>
        [Description("活动")]
        Promotion = 2,

        /// <summary>
        /// 评论
        /// </summary>
        [Description("评论")]
        Comment = 3,

        /// <summary>
        /// 客户肖像（LOGO） （资源使用）
        /// </summary>
        [Description("用户图片")]
        CustomerPortrait = 4,

        /// <summary>
        /// 品牌logo （资源使用）
        /// </summary>
        [Description("品牌logo")]
        BrandLogo = 5,

        /// <summary>
        /// 店铺LOGO（资源使用）
        /// </summary>
        [Description("百货店logo")]
        StoreLogo = 6,

        /// <summary>
        /// 店铺
        /// </summary>
        Store = 7,

        /// <summary>
        /// 客户
        /// </summary>
        Customer = 8,

        /// <summary>
        /// 专题
        /// </summary>
        [Description("专题")]
        SpecialTopic = 9,

        [Description("评论语音")]
        CommentAudio = 10,

        BannerPromotion = 11,
        [Description("用户背景图")]
        CustomerThumbBackground = 12,
        //ProductAudio = 12,

        PMessage = 13,

        GiftCard = 14,
        Combo = 15,
        Inventory = 100
    }
}