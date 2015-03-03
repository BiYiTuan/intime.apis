using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IVUserRoleRepository
    {
        /// <summary>
        /// 获取指定用户的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<VUserRoleEntity> GetList(int userid);

        /// <summary>
        /// 获取指定用户的权限
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        List<VUserRoleEntity> GetList(List<int> userids);
    }
}
