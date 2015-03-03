using System.Data.Entity.Infrastructure;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Architecture.Framework;

namespace Yintai.Architecture.Common.Data.EF
{
    //    public void Application_EndRequest(Object sender, EventArgs e)
    //{
    //  IUnitOfWork unit =  IoCContainer.Resolve<IUnitOfWork>();
    //  unit.Commit();
    //  unit.Dispose();
    //}

    //public void Application_Error(Object sender, EventArgs e)
    //{
    //  IUnitOfWork unit =  IoCContainer.Resolve<IUnitOfWork>();
    //  unit.Dispose();
    //  //don't forget to treat the error here
    //}

    /// <summary>
    /// Repository
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>
    public class EFRepository<T> : IEFRepository<T> where T : BaseEntity
    {
        #region fields

        /// <summary>
        /// CmsContext
        /// </summary>
        private readonly DbContext _context;

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// DbSet
        /// </summary>
        private readonly DbSet<T> _dbset;

        private static readonly ILog _log;

        #endregion

        #region .ctor

        static EFRepository()
        {
            _log = LoggerManager.Current();
        }

      
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">传入CmsContext</param>
        /// <param name="unitOfWork"></param>
        public EFRepository(DbContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        #endregion

        #region methods

        private static DbContext GetContext(IUnitOfWork unitOfWork, DbContext context)
        {
            if (unitOfWork.Context == null)
            {
                unitOfWork.Context = context;

                //var o = ((IObjectContextAdapter)unitOfWork.Context).ObjectContext.Connection;
                //if (o.State != ConnectionState.Open)
                //{
                //    o.Open();
                //}

                //unitOfWork.Transaction = o.BeginTransaction();
            }

            return unitOfWork.Context;
        }

        #endregion

        #region IEfRepository<T>  成员

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public virtual int Count
        {
            get
            {
                return this._dbset.Count();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return this._dbset.AsExpandable().Where(filter).AsQueryable();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="total">筛选后返回记录数</param>
        /// <param name="index">指定页面索引</param>
        /// <param name="size">指定页面条数</param>
        /// <returns></returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            var skipCount = index * size;
            var resetSet = filter != null ? this._dbset.AsNoTracking().AsExpandable().Where(filter).AsQueryable() : this._dbset.AsNoTracking().AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="total">筛选后返回记录数</param>
        /// <param name="index">指定页面索引</param>
        /// <param name="size">指定页面条数</param>
        /// /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            var skipCount = (index - 1) * size;
            var resetSet = filter != null ? this._dbset.AsNoTracking().AsExpandable().Where(filter).AsQueryable() : this._dbset.AsNoTracking().AsQueryable();

            resetSet = orderBy != null ? orderBy(resetSet).AsQueryable() : resetSet.AsQueryable();
            total = resetSet.Count();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);


            return resetSet;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="total">筛选后返回记录数</param>
        /// <param name="index">指定页面索引</param>
        /// <param name="size">指定页面条数</param>
        /// ///
        /// <param name="specialSkipCount"></param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int specialSkipCount = 0)
        {
            var skipCount = specialSkipCount;
            var resetSet = filter != null ? this._dbset.AsNoTracking().AsExpandable().Where(filter).AsQueryable() : this._dbset.AsNoTracking().AsQueryable();

            resetSet = orderBy != null ? orderBy(resetSet).AsQueryable() : resetSet.AsQueryable();
            total = resetSet.Count();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);


            return resetSet;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="includeProperties">包含属性</param>
        /// <returns></returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = this._dbset;

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// 根据条件分组
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter"></param>
        /// <param name="grouping"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Get<TKey>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                         Func<IQueryable<T>, IQueryable<IGrouping<TKey, T>>> grouping = null, int takeCount = 1)
        {
            IQueryable<T> query = this._dbset;

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            var data = new List<T>();

            if (grouping != null)
            {
                var result = grouping(query);

                foreach (var item in result)
                {
                    var t = item.Take(takeCount);
                    if (t.Any())
                    {
                        data.AddRange(t);
                    }
                }

                return data.AsQueryable();
            }

            return query;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>dbset</returns>
        public IQueryable<T> GetAll()
        {
            return this._dbset.AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// 查看是否存在某种条件下记录
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public bool Contains(Expression<Func<T, bool>> filter)
        {
            return this._dbset.AsNoTracking().AsExpandable().Any(filter);
        }

        /// <summary>
        /// 根据ID获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Find(object id)
        {
            return this._dbset.Find(id);
        }

        /// <summary>
        /// Find object by keys
        /// </summary>
        /// <param name="keys">Specified the search keys</param>
        /// <returns>DbSet</returns>
        public virtual T Find(params object[] keys)
        {
            return this._dbset.Find(keys);
        }

        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>T</returns>
        public virtual T Find(Expression<Func<T, bool>> filter)
        {
            return this._dbset.AsExpandable().FirstOrDefault(filter);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        /// <returns>T</returns>
        public virtual T Insert(T entity)
        {
            var newentity = this._dbset.Add(entity);
            try
            {
                this._context.SaveChanges();
                //_unitOfWork.Commit();
                // 写数据库
            }

            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors != null)
                {
                    foreach (var err in dbEx.EntityValidationErrors)
                    {
                        foreach (var e in err.ValidationErrors)
                        {
                            _log.Error(e.PropertyName + ":" + e.ErrorMessage);
                        }
                    }
                }
                throw dbEx;
            }

            return newentity;
        }

        public virtual IQueryable<T> BatchInsert(params T[] entitys)
        {
            var newEntitys = new T[entitys.Count()];
            try
            {
                for (var i = 0; i < entitys.Count(); i++)
                {
                    var newentity = this._dbset.Add(entitys[i]);
                    newEntitys[i] = newentity;
                }
                //_unitOfWork.Commit();
                this._context.SaveChanges();
            }
            catch (Exception)
            {
                //foreach (var newEntity in newEntitys)
                //{
                //    this._dbset.Remove(newEntity);
                //}

                throw;
            }

            return new EnumerableQuery<T>(newEntitys); ;
        }

        #region delete

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id">根据ID来删除</param>
        public virtual void Delete(object id)
        {
            var entityToDelete = this._dbset.Find(id);
            this.Delete(entityToDelete);

            //_unitOfWork.Commit();
            this._context.SaveChanges();
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entityToDelete">要删除的实体</param>
        public virtual void Delete(T entityToDelete)
        {
            var entry = this._context.Entry(entityToDelete);
            if (entry.State == EntityState.Detached)
            {
                entityToDelete = Find(entityToDelete.EntityId);
            }
            else
            {
                this._context.Entry(entityToDelete).State = EntityState.Deleted;
            }

            this._dbset.Remove(entityToDelete);
            this._context.SaveChanges();
            //_unitOfWork.Commit();
        }

        /// <summary>        
        /// 根据条件删除.        
        /// </summary>        
        /// <param name="filter">删除条件</param>
        /// <returns>影响行数</returns>
        public virtual int Delete(Expression<Func<T, bool>> filter)
        {
            var objects = Get(filter);
            foreach (var obj in objects)
                this._dbset.Remove(obj);
            var t = this._context.SaveChanges();

            //_unitOfWork.Commit();

            return t;
        }

        #endregion

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entityToUpdate">要更新的实体</param>
        public virtual void Update(T entityToUpdate)
        {
            var old = this._context.Entry(entityToUpdate);
            if (old.State == EntityState.Detached)
            {
                var entry = Find(entityToUpdate.EntityId);
                entityToUpdate = Mapper.Map(entityToUpdate, entry);
            }

            _dbset.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;

            try
            {
                this._context.SaveChanges();
                //_unitOfWork.Commit();
                // 写数据库
            }

            catch (DbEntityValidationException dbEx)
            {
                if (dbEx.EntityValidationErrors != null)
                {
                    foreach (var err in dbEx.EntityValidationErrors)
                    {
                        foreach (var e in err.ValidationErrors)
                        {
                            _log.Error(e.PropertyName + ":" + e.ErrorMessage);
                        }
                    }
                }
                throw dbEx;
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        public T Create()
        {
            var entity = this._dbset.Create();
            this._context.SaveChanges();

            return entity;
        }

        public virtual IQueryable<T> GetWithRawSql(string query, params object[] parameters)
        {
            return this._dbset.SqlQuery(query, parameters).AsQueryable();
        }

        public virtual IQueryable<T> GetWithDBSql(string query, params object[] parameters)
        {
            return this._context.Database.SqlQuery<T>(query, parameters).AsQueryable();
        }

        public virtual int ExecuteSqlCommand(string sql)
        {
            return ExecuteSqlCommand(sql, new object[] { });
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return this._context.Database.ExecuteSqlCommand(sql);
            }

            return this._context.Database.ExecuteSqlCommand(sql, parameters);
        }

        #endregion
    }
}
