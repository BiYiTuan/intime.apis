using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Impl;

namespace Yintai.Hangzhou.Repository.Contract
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.Repository.Contract
    /// FileName: IVerificationCodeRepository
    ///
    /// Created at 11/15/2012 11:32:41 AM
    /// Description: 
    /// </summary>
    public interface IVerifyCodeRepository : IRepository<VerifyCodeEntity, int>
    {
        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="type">业务类型</param>
        /// <param name="code">验证码</param>
        /// <param name="verifySource">验证来源（手机or email）</param>
        /// <param name="verifyMode">1手机 2 email</param>
        /// <returns></returns>
        VerifyCodeEntity GetItem(int userid, int type, string code, string verifySource, int verifyMode);
    }
}
