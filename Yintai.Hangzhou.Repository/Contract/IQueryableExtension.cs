using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Repository.Contract
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> WhereWithPageSort<T>(this IQueryable<T> linq, out int total, int index = 0, int size = 50, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            linq = orderBy != null ? orderBy(linq).AsQueryable() : linq.AsQueryable();
            total = linq.Count();
            var skipCount = (index - 1) * size;
            skipCount = skipCount < 0 ? 0 : skipCount;
            linq = linq.Skip(skipCount).Take(size);
            return linq;
        }
    }
}
