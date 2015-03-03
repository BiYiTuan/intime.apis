using System;
using System.Collections.Generic;
using System.Linq;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class RoleRepository : RepositoryBase<RoleEntity, int>, IRoleRepository
    {
        public override RoleEntity GetItem(int key)
        {
            return base.Find(key);
        }

        public RoleEntity GetItemByRoleName(string roleName)
        {
            return base.Get(v => v.Name == roleName).SingleOrDefault();
        }

        public RoleEntity GetItemByRoleVal(int val)
        {
            return base.Get(v => v.Val == val).SingleOrDefault();
        }

        public List<RoleEntity> GetListByIds(List<int> ids)
        {
            return base.Get(v => ids.Any(s => s == v.Id)).ToList();
        }

        public List<RoleEntity> GetList()
        {
            return base.FindAll();
        }
        public IEnumerable<RoleEntity> LoadAllEagerly()
        {
            return Context.Set<RoleEntity>().Include("RoleAccessRights")
                .Include("RoleAccessRights.AdminAccessRight")
                .AsEnumerable();
        }
        public RoleEntity UpdateWithRights(RoleEntity roleEntity)
        {
            var updatingEntity = Find(roleEntity.Id);
            updatingEntity.Name = roleEntity.Name;
            updatingEntity.Val = roleEntity.Val;

            ICollection<RoleAccessRightEntity> oldRights = updatingEntity.RoleAccessRights;
            List<RoleAccessRightEntity> toDeleted = oldRights.Except(roleEntity.RoleAccessRights, new RoleAccessRightEntityComparer()).ToList();
            //deleted
            foreach (var diff in toDeleted) {
                var diffRight = oldRights.FirstOrDefault(e => e.Id == diff.Id);
                if (diffRight != null)
                {
                    Context.Set<RoleAccessRightEntity>().Remove(diffRight);
                }
            }
            //inserted
            foreach (var diff in roleEntity.RoleAccessRights.Except(oldRights, new RoleAccessRightEntityComparer())) {
                oldRights.Add(new RoleAccessRightEntity() { 
                     AccessRightId = diff.AccessRightId
                     , RoleId = diff.RoleId
                });
            }
            Update(updatingEntity);
            return Find(roleEntity.Id);
        }

        public void InsertWithUserRelation(int User, string[] p)
        {
            if (Context.Set<UserEntity>().Find(User) == null ||
                p.Length==0)
                return;
            foreach (var role in p)
            {
                Context.Set<UserRoleEntity>().Add(new UserRoleEntity()
                {
                    User_Id = User
                    ,
                    Role_Id = int.Parse(role)
                    ,
                    Status = 1
                    ,
                    CreatedDate = DateTime.Now
                });
            }
            Context.SaveChanges();
        }
        public void DeleteRolesOfUserId(int Id)
        {
            var roles = Context.Set<UserRoleEntity>().Where(r => r.User_Id == Id);
            foreach (var role in roles)
            {
                Context.Set<UserRoleEntity>().Remove(role);
            }
            Context.SaveChanges();
        }

        public void UpdateWithUserRelation(int User, string[] p)
        {
            List<UserRoleEntity> oldRights = Context.Set<UserRoleEntity>().Where(ur=>ur.User_Id==User).ToList();
            var newRights = from nr in p
                            select new UserRoleEntity() {
                                 Role_Id = int.Parse(nr)
                                 , User_Id = User
                            };
            List<UserRoleEntity> toDeleted = oldRights.Except(newRights, new UserRoleEntityComparer()).ToList();
            //deleted
            foreach (var diff in toDeleted)
            {
                var diffRight = oldRights.FirstOrDefault(e => e.Id == diff.Id);
                if (diffRight != null)
                {
                    Context.Set<UserRoleEntity>().Remove(diffRight);
                }
            }
            //inserted
            foreach (var diff in newRights.Except(oldRights, new UserRoleEntityComparer()))
            {
                Context.Set<UserRoleEntity>().Add(new UserRoleEntity()
                {
                    User_Id  = diff.User_Id
                    ,
                    Role_Id = diff.Role_Id
                    ,
                    Status = 1
                    ,
                    CreatedDate = DateTime.Now
                });
            }
            Context.SaveChanges();
        }
        public IEnumerable<UserEntity> FindAllUsersHavingRoles()
        {
            return (from ur in Context.Set<UserRoleEntity>()
                   from u in Context.Set<UserEntity>()
                   where ur.User_Id == u.Id
                   select u).Distinct();
        }
        public IEnumerable<AdminAccessRightEntity> LoadAllRights()
        {
            return Context.Set<AdminAccessRightEntity>().AsEnumerable();
        }
        private class RoleAccessRightEntityComparer : EqualityComparer<RoleAccessRightEntity>
        {

            public override bool Equals(RoleAccessRightEntity x, RoleAccessRightEntity y)
            {
                return x.AccessRightId == y.AccessRightId &&
                    x.RoleId == y.RoleId;
            }

            public override int GetHashCode(RoleAccessRightEntity obj)
            {
                int hashValue = (obj.RoleId << obj.AccessRightId.ToString().Length + obj.AccessRightId).GetHashCode();
                return hashValue;
            }
        }
        private class UserRoleEntityComparer : EqualityComparer<UserRoleEntity>
        {

            public override bool Equals(UserRoleEntity x, UserRoleEntity y)
            {
                return x.User_Id == y.User_Id &&
                    x.Role_Id == y.Role_Id;
            }

            public override int GetHashCode(UserRoleEntity obj)
            {
                int hashValue = (obj.Role_Id << obj.User_Id.ToString().Length + obj.User_Id).GetHashCode();
                return hashValue;
            }
        }

    }
}
