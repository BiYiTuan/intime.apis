using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class VerifyCodeRepository : RepositoryBase<VerifyCodeEntity, int>, IVerifyCodeRepository
    {
        #region Overrides of RepositoryBase<VerifyCodeEntity,int>

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override VerifyCodeEntity GetItem(int key)
        {
            return base.Find(key);
        }

        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="type">业务类型</param>
        /// <param name="code">验证码</param>
        /// <param name="verifySource">验证来源（手机or email）</param>
        /// <param name="verifyMode">1手机 2 email</param>
        /// <returns></returns>
        public VerifyCodeEntity GetItem(int userid, int type, string code, string verifySource, int verifyMode)
        {
            var entity = base.Get(
                v =>
                v.User_Id == userid && v.Type == type && v.Code == code &&
                v.VerifySource == verifySource && v.VerifyMode == verifyMode).FirstOrDefault();

            return entity;
        }

        #endregion
    }
}
