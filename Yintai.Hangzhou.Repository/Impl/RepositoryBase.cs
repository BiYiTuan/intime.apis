using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class RepositoryBase<TEntity, TKey> : EFRepository<TEntity>, IRepository<TEntity, TKey> where TEntity : BaseEntity
    {
        private DbContext _innerContext;
        //提供IOC注入方式接口
        /// <summary>
        /// EF构造
        /// </summary>
        /// <param name="context"></param>
        protected RepositoryBase(DbContext context)
            : base(context)
        {
        }

        protected RepositoryBase():this(ServiceLocator.Current.Resolve<DbContext>())
        {
        }

        public DbContext Context { get {
            if (_innerContext == null)
                _innerContext = ServiceLocator.Current.Resolve<DbContext>();
            return _innerContext;
            
        } }

        #region IRepository<TEntity, TKey> 成员


        public override void Update(TEntity entity)
        {
            base.Update(entity);

            //执行验证业务
            //context.Entry<T>(entity).GetValidationResult();
            //if (Context.Entry<TEntity>(entity).State == EntityState.Modified)
            //    Context.SaveChanges();
            //return entity;
        }

        public override void Delete(TEntity entity)
        {
            base.Delete(entity);
        }
      
        public override TEntity Find(params object[] keyValues)
        {
            return base.Find(keyValues);
        }

        public List<TEntity> FindAll()
        {
            return base.GetAll().ToList();
        }

        public IEnumerable<TEntity> AutoComplete(string name)
        {
            return base.GetAll();
        }

        #endregion

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TEntity GetItem(TKey key)
        {
            return base.Find(key);
        }

    
    }
}
