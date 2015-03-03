using System;
using System.Runtime.Serialization;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Response;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    public interface IPagerInfo
    {
        /// <summary>
        /// 页码(从1起始)
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        int Size { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPaged { get; }

        /// <summary>
        /// 总记录数
        /// </summary>
        int TotalCount { get; }
    }

    [DataContract]
    public abstract class PagerInfoResponse : BaseResponse, IPagerInfo
    {
        protected PagerInfoResponse(PagerRequest request)
            : this(request, 0)
        {
        }

        protected PagerInfoResponse(PagerRequest request, int totalCount)
        {
            this.Index = request.PageIndex;
            this.Size = request.PageSize;
            this.TotalCount = totalCount;
        }

        #region Implementation of IPagerInfoResponse

        /// <summary>
        /// 索引
        /// </summary>
        [DataMember(Name = "pageindex", Order = 1)]
        public int Index { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        [DataMember(Name = "pagesize", Order = 2)]
        public int Size { get; set; }

        /// <summary>
        /// 总数据量
        /// </summary>
        [DataMember(Name = "totalcount", Order = 3)]
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember(Name = "totalpaged", Order = 4)]
        public int TotalPaged
        {
            get
            {
                if (this.Size <= 0)
                    return 0;
                return (int)Math.Ceiling(this.TotalCount / (double)this.Size);
            }
            set { }
        }

        /// <summary>
        /// 是否存在分页
        /// </summary>
        [DataMember(Name = "ispaged", Order = 5)]
        public bool IsPaged
        {
            get
            {
                return Size < TotalCount;
            }
            set { }
        }

        #endregion
    }
}