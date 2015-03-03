using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IBannerRepository : IRepository<BannerEntity, int>
    {
        IQueryable<BannerEntity> Get(int? sourceId, SourceType? sourceType, DataStatus? dataStatus);
    }
}