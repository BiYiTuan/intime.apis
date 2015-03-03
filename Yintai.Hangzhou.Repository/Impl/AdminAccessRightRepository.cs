using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class AdminAccessRightRepository:RepositoryBase<AdminAccessRightEntity, int>,IAdminAccessRightRepository
    {
        public void InsertIfNotPresent(IEnumerable<Data.Models.AdminAccessRightEntity> entity)
        {
            if (entity.Count() == 0)
                return;
            foreach (var right in entity)
            {
                var inStoreRight = Get(r => r.ControllName == right.ControllName && r.ActionName == right.ActionName);
                if (inStoreRight.FirstOrDefault()!= null)
                    continue;
                AdminAccessRightEntity willInsertRight = new AdminAccessRightEntity() { 
                     Name = string.Format("{0}\\{1}",right.ControllName,right.ActionName)
                     ,ControllName = right.ControllName
                     ,ActionName = right.ActionName
                     , Description = "auto imported"
                     , InDate = DateTime.Now
                     , InUser = right.InUser
                     , Status = 1
                };
                Context.Set<AdminAccessRightEntity>().Add(willInsertRight);
            }
            Context.SaveChanges();

        }

        public override AdminAccessRightEntity GetItem(int key)
        {
            return base.Find(key);
        }
    }
}
