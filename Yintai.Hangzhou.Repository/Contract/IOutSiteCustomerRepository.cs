using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IOutSiteCustomerRepository : IRepository<OutsiteUserEntity, int>
    {
        /// <summary>
        /// ≤È—Ø «∑Ò¥Ê‘⁄
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="outsiteType"></param>
        /// <returns></returns>
        OutsiteUserEntity GetItem(string uid, int outsiteType);
    }
}