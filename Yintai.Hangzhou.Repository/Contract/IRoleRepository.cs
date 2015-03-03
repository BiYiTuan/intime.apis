using System.Collections.Generic;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IRoleRepository : IRepository<RoleEntity, int>
    {
        RoleEntity GetItemByRoleName(string roleName);

        RoleEntity GetItemByRoleVal(int val);

        List<RoleEntity> GetListByIds(List<int> ids);

        List<RoleEntity> GetList();

        IEnumerable<RoleEntity> LoadAllEagerly();

        RoleEntity UpdateWithRights(RoleEntity roleEntity);

        void InsertWithUserRelation(int User, string[] p);

        void DeleteRolesOfUserId(int Id);

        void UpdateWithUserRelation(int User, string[] p);

        IEnumerable<UserEntity> FindAllUsersHavingRoles();

        IEnumerable<AdminAccessRightEntity> LoadAllRights();
    }
}
