using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Yintai.Architecture.Common.Models
{
    /// <summary>
    /// 可分页的
    /// </summary>
    public interface IPagedable
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

    /// <summary>
    /// 可分页迭代器
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public interface IPagedEnumerable<out T> : IEnumerable<T>, IPagedable
    {
    }

    //[DataContract(Name = "pager")]
    public class PagerInfo<T> : List<T>, IPagedEnumerable<T>
    {
        #region fields

        #endregion

        #region .ctor

        public PagerInfo()
        {
        }

        public PagerInfo(PagerRequest request)
            : this(request, 0)
        {
        }

        public PagerInfo(PagerRequest request, int totalCount)
        {
            this.Index = request.PageIndex;
            this.Size = request.PageSize;
            this.TotalCount = totalCount;
        }

        #endregion

        #region properties

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

        #region methods

        #endregion
    }

    /// <summary>
    /// 分页请求
    /// </summary>
    [DataContract]
    public class PagerRequest
    {
        private readonly int _maxPageSize;

        public PagerRequest()
            : this(40)
        {
        }

        public PagerRequest(int maxPageSize)
        {
            this._maxPageSize = maxPageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerRequest"/> class.
        /// </summary>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        public PagerRequest(int pageNumber, int pageSize)
            : this(pageNumber, pageSize, 40)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerRequest"/> class.
        /// </summary>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="maxPageSize"> </param>
        public PagerRequest(int pageNumber, int pageSize, int maxPageSize)
            : this(maxPageSize)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageNumber;
        }

        /// <summary>
        /// local pageNumber
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// local pageSize
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// 页大小
        /// </summary>
        [DataMember]
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }

            private set
            {
                this._pageSize = (value < 1 ? 1 : (value > _maxPageSize ? _maxPageSize : value));
            }
        }

        /// <summary>
        /// 页码
        /// </summary>
        [DataMember]
        public int PageIndex
        {
            get
            {
                return this._pageIndex;
            }

            private set
            {
                this._pageIndex = value < 1 ? 1 : value;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("[pagerequest:");

            sb.AppendFormat("pi_{0}|", PageIndex.ToString(CultureInfo.InvariantCulture));
            sb.AppendFormat("ps_{0}", PageSize.ToString(CultureInfo.InvariantCulture));

            sb.Append("]");

            return sb.ToString();
        }
    }
}