using System.Collections.Generic;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IUserRoleRepository : IRepository<UserRoleEntity, int>
    {
        List<UserRoleEntity> GetListByRoleId(int roleId);

        List<UserRoleEntity> GetListByUserId(int userId);

        IEnumerable<RoleEntity> FindRolesByUserId(int p);
    }
}