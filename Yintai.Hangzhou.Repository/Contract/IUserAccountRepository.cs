using System.Collections.Generic;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IUserAccountRepository : IRepository<UserAccountEntity, int>
    {
        /// <summary>
        /// 获取用户账户
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="accountType">accountType</param>
        /// <returns></returns>
        UserAccountEntity GetItem(int userId, AccountType accountType);

        /// <summary>
        /// 获取当前用户的账户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserAccountEntity> GetUserAccount(int userId);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        List<UserAccountEntity> GetListByUserIds(List<int> userIds);

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountType"></param>
        /// <param name="amount"></param>
        void SetAmount(int userId, AccountType accountType, decimal amount);

        /// <summary>
        /// 账户加操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountType"></param>
        /// <param name="amount"></param>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        int AddCount(int userId, AccountType accountType, decimal amount, int? updateUser);

        /// <summary>
        /// 账户减操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountType"></param>
        /// <param name="amount"></param>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        int SubCount(int userId, AccountType accountType, decimal amount, int? updateUser);
    }
}