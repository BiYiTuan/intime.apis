using System.Collections.Generic;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class UserRoleRepository : RepositoryBase<UserRoleEntity, int>, IUserRoleRepository
    {
        public override UserRoleEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public List<UserRoleEntity> GetListByRoleId(int roleId)
        {
            return base.Get(v => v.Role_Id == roleId && v.Status == (int)DataStatus.Normal).ToList();
        }

        public List<UserRoleEntity> GetListByUserId(int userId)
        {
            return base.Get(v => v.User_Id == userId && v.Status == (int)DataStatus.Normal).ToList();
        }



        public IEnumerable<RoleEntity> FindRolesByUserId(int p)
        {
            var result = from ru in Context.Set<UserRoleEntity>()
                         where ru.User_Id == p
                         from r in Context.Set<RoleEntity>()
                         where ru.Role_Id == r.Id
                         select r;
            return result;
        }
    }
}