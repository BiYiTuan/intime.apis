using System;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Contract.DTO.Request.Tag
{
    public class TagGetRequest : BaseRequest
    {
        /// <summary>
        /// 店铺Id
        /// </summary>
        public int TagId { get; set; }
    }

    public class TagInfoRequest : AuthRequest
    {
        public int TagId { get; set; }

        public int Id
        {
            get { return TagId; }
            set { TagId = value; }
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public int Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public int CreatedUser { get; set; }
    }

    public class TagMethodRequest : TagInfoRequest
    {
    }

    /// <summary>
    /// 创建 tag
    /// </summary>
    public class TagCreateRequest : TagMethodRequest
    {

    }

    /// <summary>
    /// Update tag
    /// </summary>
    public class TagUpdateRequest : TagMethodRequest
    {

    }

    public class TagDestroyRequest : AuthRequest
    {
        public int TagId { get; set; }
    }

    public class TagGetAllRequest : BaseRequest
    {
        public string Type { get; set; }
        public DateTime Refreshts { get; set; }

        public Timestamp Timestamp
        {
            get { return new Timestamp() { Ts = Refreshts, TsType = TimestampType.Old }; }
        }
    }

    public class TagGetRefreshRequest : RefreshRequest
    {
        public DateTime RTs { get; set; }
    }
}
