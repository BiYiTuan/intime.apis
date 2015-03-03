using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class OutsiteCustomerRepository : RepositoryBase<OutsiteUserEntity, int>, IOutSiteCustomerRepository
    {
        #region Overrides of RepositoryBase<OutsiteUserEntity,int>

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override OutsiteUserEntity GetItem(int key)
        {
            return base.Find(key);
        }

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="outsiteType"></param>
        /// <returns></returns>
        public OutsiteUserEntity GetItem(string uid, int outsiteType)
        {
            return base.Get(v => v.OutsiteUserId == uid && v.OutsiteType == outsiteType).FirstOrDefault();
        }

        #endregion
    }
}