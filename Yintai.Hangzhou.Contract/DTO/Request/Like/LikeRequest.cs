using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Like
{
    /// <summary>
    /// 创建
    /// </summary>
    public class LikeCreateRequest : AuthRequest
    {
        /// <summary>
        /// 被喜欢(关注)人的ID
        /// </summary>
        public int LikedUserId { get; set; }
    }

    /// <summary>
    /// 删除
    /// </summary>
    public class LikeDestroyRequest : AuthRequest
    {
        public int LikedUserId { get; set; }
    }

    /// <summary>
    /// 我喜欢（关注）的列表
    /// </summary>
    public class GetLikeListRequest : AuthPagerInfoRequest
    {
        public int Sort { get; set; }

        public string Type { get; set; }

        public LikeSortOrder LikeSortOrder
        {
            get { return (LikeSortOrder)Sort; }
            set { Sort = (int)value; }
        }
    }

    /// <summary>
    /// 喜欢(关注)我的列表
    /// </summary>
    public class GetLikedListRequest : AuthPagerInfoRequest
    {
        public int Sort { get; set; }
        public LikeSortOrder LikeSortOrder
        {
            get { return (LikeSortOrder)Sort; }
            set { Sort = (int)value; }
        }
    }
}
