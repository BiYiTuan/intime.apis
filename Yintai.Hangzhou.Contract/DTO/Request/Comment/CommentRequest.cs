using System.Web;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Comment
{
    public class CommentListRequest : ListRequest
    {
        /// <summary>
        /// 来源ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType SType
        {
            get { return (SourceType)SourceType; }
            set { }
        }

        public int Sort { get; set; }

        public CommentSortOrder SortOrder
        {
            get { return (CommentSortOrder)Sort; }
            set { }
        }

        public string Type { get; set; }
    }

    public class CommentCreateRequest : AuthRequest
    {
        /// <summary>
        /// 来源ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType SType
        {
            get { return (SourceType)SourceType; }
        }

        public string Content { get; set; }

        public int ReplyUser { get; set; }

        public HttpFileCollectionBase Files { get; set; }
    }

    public class CommentDetailRequest : AuthRequest
    {
        public int CommentId { get; set; }
    }

    public class CommentUpdateRequest : AuthRequest
    {
        public int CommentId { get; set; }

        public string Content { get; set; }

        public HttpFileCollectionBase Files { get; set; }

    }

    public class CommentDestroyRequest : AuthRequest
    {
        public int CommentId { get; set; }
    }

    public class CommentRefreshRequest : RefreshRequest
    {
        /// <summary>
        /// 来源ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType SType
        {
            get { return (SourceType)SourceType; }
            set { }
        }

        public int Sort { get; set; }

        public CommentSortOrder SortOrder
        {
            get { return (CommentSortOrder)Sort; }
            set { }
        }
    }
}
