using System;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class BannerRepository : RepositoryBase<BannerEntity, int>, IBannerRepository
    {
        private static Expression<Func<BannerEntity, bool>> Filter(int? sourceId, SourceType? sourceType, DataStatus? dataStatus)
        {
            var filter = PredicateBuilder.True<BannerEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            if (sourceType != null)
            {
                filter = filter.And(v => v.SourceType == (int) sourceType);
            }

            if (sourceId != null)
            {
                filter = filter.And(v => v.SourceId == sourceId.Value);
            }

            return filter;
        }


        public IQueryable<BannerEntity> Get(int? sourceId, SourceType? sourceType, DataStatus? dataStatus)
        {
            return base.Get(Filter(sourceId, sourceType, dataStatus));
        }
    }
}
