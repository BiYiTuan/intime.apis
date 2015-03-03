using System;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Models;

namespace Yintai.Architecture.Common.Data.EF
{
    /// <summary>
    /// IRepository接口
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>
    public interface IEFRepository<T> where T : BaseEntity
    {
        /// <summary>        
        /// Get the total objects count.        
        /// </summary>        
        int Count { get; }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null);

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="Key">主键</typeparam>
        /// <param name="filter">条件</param>
        /// <param name="total">筛选后返回记录数</param>
        /// <param name="index">指定页面索引</param>
        /// <param name="size">指定页面条数</param>
        /// <returns></returns>
        IQueryable<T> Get(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="Key">主键</typeparam>
        /// <param name="filter">条件</param>
        /// <param name="total">筛选后返回记录数</param>
        /// <param name="index">指定页面索引</param>
        /// <param name="size">指定页面条数</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        IQueryable<T> Get(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="includeProperties">包含属性</param>
        /// <returns></returns>
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// 根据ID获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find(object id);

        /// <summary>
        /// 查看是否存在某种条件下记录
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        bool Contains(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Find object by keys
        /// </summary>
        /// <param name="keys">Specified the search keys</param>
        /// <returns></returns>
        T Find(params object[] keys);

        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        /// <returns name="T">返回更新后的实体</returns>
        T Insert(T entity);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entitys">实体</param>
        /// <returns></returns>
        IQueryable<T> BatchInsert(params T[] entitys);

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id">根据ID来删除</param>
        void Delete(object id);

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        void Delete(T entity);

        /// <summary>        
        /// 根据条件删除.        
        /// </summary>        
        /// <param name="filter">删除条件</param> 
        /// <returns>int</returns>
        int Delete(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        void Update(T entity);

        /// <summary>
        /// 创建
        /// </summary>
        T Create();
    }
}