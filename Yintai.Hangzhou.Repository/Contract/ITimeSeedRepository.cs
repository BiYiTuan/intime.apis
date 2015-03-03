using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ITimeSeedRepository : IRepository<TimeSeedEntity, int>
    {
        /// <summary>
        ///  最小种子是 小时 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="maxSeed"></param>
        /// <param name="seedPre"></param>
        /// <returns></returns>
        [System.Obsolete("请使用CreateLimitMaxSeedV2版本，以天为单位，这个方法废弃")]
        TimeSeedEntity CreateLimitMaxSeed(TimeSeedEntity entity, int maxSeed, string seedPre);

        /// <summary>
        ///  最小种子是 天
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="maxSeed"></param>
        /// <param name="seedPre"></param>
        /// <returns></returns>
        TimeSeedEntity CreateLimitMaxSeedV2(TimeSeedEntity entity, int maxSeed, string seedPre);
    }
}
