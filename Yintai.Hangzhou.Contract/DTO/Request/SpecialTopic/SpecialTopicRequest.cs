using Yintai.Architecture.Framework.Extension;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.SpecialTopic
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.Contract.Request.SpecialTopic
    /// FileName: GetSpecialTopicListRequest
    ///
    /// Created at 11/12/2012 2:58:16 PM
    /// Description: 
    /// </summary>
    public class GetSpecialTopicListRequest : ListRequest
    {
        public string Type { get; set; }

        public int Sort { get; set; }

        public SpecialTopicSortOrder SortOrder
        {
            get { return EnumExtension.Parser<SpecialTopicSortOrder>(Sort); }
        }
    }

    public class GetSpecialTopicListForRefresh : RefreshRequest
    {
        public int Sort { get; set; }

        public SpecialTopicSortOrder SortOrder
        {
            get { return (SpecialTopicSortOrder)Sort; }
            set { Sort = (int)value; }
        }
    }

    public class GetSpecialTopicInfoRequest : BaseRequest
    {
        public int TopicId { get; set; }

        /// <summary>
        /// 当前请求的用户，可以是匿名的
        /// </summary>
        public UserModel CurrentAuthUser { get; set; }
    }
}
