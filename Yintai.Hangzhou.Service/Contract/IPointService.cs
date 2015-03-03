using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IPointService
    {
        PointHistoryEntity Insert(PointHistoryEntity entity);
    }
}
