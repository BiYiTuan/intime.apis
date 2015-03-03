using System;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class PointService : BaseService, IPointService
    {
        private readonly IPointRepository _pointRepository;

        public PointService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        /// <summary>
        /// 添加一条积点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public PointHistoryEntity Insert(PointHistoryEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            entity.Name = entity.Name ?? String.Empty;
            entity.Description = entity.Description ?? String.Empty;

            try
            {
                return _pointRepository.Insert(entity);
            }
            catch (Exception ex)
            {
                Logger.Warn(String.Format("创建积点失败userid={0},type={1},sourceType={2},Amount={3}", entity.User_Id, entity.Type, entity.PointSourceType, entity.Amount));
                Logger.Error(ex);

                return null;
            }
        }
    }
}
