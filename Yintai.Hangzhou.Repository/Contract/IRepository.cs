using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IRepository<TEntity, in TKey> : IEFRepository<TEntity>
        where TEntity : BaseEntity
    {

        /// <summary>
        /// 全部
        /// </summary>
        /// <returns></returns>
        List<TEntity> FindAll();

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity GetItem(TKey key);

        /// <summary>
        /// Autocomplete interface
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TEntity> AutoComplete(string query);
        /// <summary>
        /// Context should open to utilize the iquerable feature
        /// </summary>
        DbContext Context { get; }
     }
}
