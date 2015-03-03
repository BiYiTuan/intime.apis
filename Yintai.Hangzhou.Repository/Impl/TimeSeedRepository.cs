using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    /// <summary>
    /// CLR Version: 4.0.30319.296
    /// NameSpace: Yintai.Hangzhou.Repository.Impl
    /// FileName: TimeSeedRepository
    ///
    /// Created at 11/26/2012 6:43:09 PM
    /// Description: 
    /// </summary>
    public class TimeSeedRepository : RepositoryBase<TimeSeedEntity, int>, ITimeSeedRepository
    {
        #region fields

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        #endregion

        #region Overrides of RepositoryBase<TimeSeedEntity,int>

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override TimeSeedEntity GetItem(int key)
        {
            return base.Find(key);
        }


        private TimeSeedEntity CreateLimitMaxSeed(string proName, TimeSeedEntity entity, int maxSeed, string seedPre)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var outputId = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var outputFlag = new SqlParameter("@flag", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var parames = new SqlParameter[6];

            parames[0] = new SqlParameter("@date", entity.Date);
            parames[1] = new SqlParameter("@keyPre", seedPre);
            parames[2] = new SqlParameter("@maxSeed", maxSeed);
            parames[3] = new SqlParameter("@seedLength", (maxSeed.ToString(CultureInfo.InvariantCulture)).Length);
            parames[4] = outputId;
            parames[5] = outputFlag;

            var r = SqlHelper.ExecuteScalar(SqlHelper.GetConnection(), CommandType.StoredProcedure,
                                            proName, parames);

            var flag = Int32.Parse(outputFlag.Value.ToString());
            switch (flag)
            {
                case -1:
                    throw new ArgumentException("创建种子时异常:maxSeed:" + maxSeed + "_seedPre:" + seedPre + "_date:" +
                                          entity.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                case 2:
                    return null;
            }

            var id = Int32.Parse(outputId.Value.ToString());

            return id > 0 ? GetItem(id) : null;
        }

        public TimeSeedEntity CreateLimitMaxSeed(TimeSeedEntity entity, int maxSeed, string seedPre)
        {
            throw new ArgumentException("该方法已经废弃");

            //return CreateLimitMaxSeed("[dbo].[TimeSeed_Create]", entity, maxSeed, seedPre);
            //if (entity == null)
            //{
            //    throw new ArgumentNullException("entity");
            //}

            //var outputId = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };
            //var outputFlag = new SqlParameter("@flag", SqlDbType.Int) { Direction = ParameterDirection.Output };
            //var parames = new SqlParameter[6];

            //parames[0] = new SqlParameter("@date", entity.Date);
            //parames[1] = new SqlParameter("@keyPre", seedPre);
            //parames[2] = new SqlParameter("@maxSeed", maxSeed);
            //parames[3] = new SqlParameter("@seedLength", (maxSeed.ToString(CultureInfo.InvariantCulture)).Length);
            //parames[4] = outputId;
            //parames[5] = outputFlag;

            //var r = SqlHelper.ExecuteScalar(SqlHelper.GetConnection(), CommandType.StoredProcedure,
            //                                "[dbo].[TimeSeed_Create]", parames);

            //var flag = Int32.Parse(outputFlag.Value.ToString());
            //switch (flag)
            //{
            //    case -1:
            //        throw new ArgumentException("创建种子时异常:maxSeed:" + maxSeed + "_seedPre:" + seedPre + "_date:" +
            //                              entity.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            //    case 2:
            //        return null;
            //}

            //var id = Int32.Parse(outputId.Value.ToString());

            //return id > 0 ? GetItem(id) : null;
        }

        public TimeSeedEntity CreateLimitMaxSeedV2(TimeSeedEntity entity, int maxSeed, string seedPre)
        {
            return CreateLimitMaxSeed("[dbo].[TimeSeed_CreateV2]", entity, maxSeed, seedPre);
        }

        #endregion
    }
}
