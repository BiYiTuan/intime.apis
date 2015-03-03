using System.Collections.Generic;

namespace Yintai.Hangzhou.WebApiCore.Areas.Gg.ViewModels
{
    /// <summary>
    /// 分页显示ViewModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedListViewModel<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 商品总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 列表数据
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
