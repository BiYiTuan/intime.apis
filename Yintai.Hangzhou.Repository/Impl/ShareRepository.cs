using System;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class ShareRepository : RepositoryBase<ShareHistoryEntity, int>, IShareRepository
    {

        #region Overrides of RepositoryBase<ShareHistoryEntity,int>

        /// <summary>
        /// ≤È’“key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override ShareHistoryEntity GetItem(int key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}