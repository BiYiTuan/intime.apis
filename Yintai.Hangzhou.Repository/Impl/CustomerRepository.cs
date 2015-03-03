using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class CustomerRepository : RepositoryBase<UserEntity, int>, ICustomerRepository
    {
        public CustomerRepository()
        {
        }


        #region methods

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        private static Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>> OrderBy(CustomerSortOrder sort)
        {
            Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>> orderBy;
            switch (sort)
            {
                case CustomerSortOrder.LastLoginDate:
                    orderBy = v => v.OrderByDescending(s => s.LastLoginDate);
                    break;
                //最新
                default:
                    orderBy = v => v.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            return orderBy;
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="dataStatus">数据状态</param>
        /// <param name="userLevel">等级</param>
        /// <param name="storeId"></param>
        /// <param name="mobile">手机</param>
        /// <returns></returns>
        private static Expression<Func<UserEntity, bool>> Filter(DataStatus? dataStatus, UserLevel? userLevel, int? storeId, string mobile = null)
        {
            var filter = PredicateBuilder.True<UserEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus.Value);
            }

            if (storeId != null)
            {
                filter = filter.And(v => v.Store_Id == storeId.Value);
            }

            //TODO: 这块值得商榷
            if (userLevel != null)
            {
                filter = filter.And(v => v.UserLevel == (int)(userLevel));
            }

            if (!String.IsNullOrWhiteSpace(mobile))
            {
                filter = filter.And(v => v.Mobile == mobile);
            }

            return filter;
        }

        #endregion

        #region Overrides of RepositoryBase<UserEntity,int>

        /// <summary>
        /// 查找key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override UserEntity GetItem(int key)
        {
            return base.Find(key);
        }

        /// <summary>
        /// 获取指定的用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<UserEntity> GetListByIds(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<UserEntity>(0);
            }

            return base.Get(v => ids.Any(s => s == v.Id)).ToList();
        }

        public UserEntity GetItem(string name, string password)
        {
            var t =
                base.Get(v => String.Compare(v.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                    .SingleOrDefault();
            if (t == null)
            {
                return null;
            }

            if (t.Password == password)
            {
                return t;
            }

            return null;
        }

        public List<UserEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, CustomerSortOrder sortOrder)
        {
            return base.Get(Filter(DataStatus.Normal, null, null), out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, OrderBy(sortOrder)).ToList();
        }

        public List<UserEntity> GetPagedListForNickName(PagerRequest pagerRequest, out int totalCount, CustomerSortOrder sortOrder, string nickName,
                                            string mobile)
        {
            var f = Filter(DataStatus.Normal, null, null, mobile);
            if (!String.IsNullOrWhiteSpace(nickName))
            {
                f = f.And(v => v.Nickname.StartsWith(nickName));
            }

            return base.Get(f, out totalCount, pagerRequest.PageIndex, pagerRequest.PageSize, OrderBy(sortOrder)).ToList();
        }

        public int SetLoginDate(int userId, DateTime dateTime)
        {
            var parames = new List<SqlParameter>
                {
                    new SqlParameter("@LastLoginDate", dateTime),
                    new SqlParameter("@Id", userId),
                };

            const string sql = @"UPDATE [dbo].[User]
SET    [LastLoginDate] = @LastLoginDate
WHERE  Id = @Id;";

            var i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sql, parames.ToArray());

            return i;
        }

        public int SetCardBinded(int userId, bool? binded)
        {
            var parames = new List<SqlParameter>
                {
                    new SqlParameter("@IsCardBinded", binded),
                    new SqlParameter("@UserId", userId),
                };

            const string sql = @"UPDATE [dbo].[User]
SET    [IsCardBinded] = @IsCardBinded
WHERE  Id = @UserId;";

            var i = SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.Text, sql, parames.ToArray());

            return i;
        }

        public override void Delete(object id)
        {

            ServiceLocator.Current.Resolve<ILog>().Warn(String.Format("删除用户??{0}", id));

            base.Delete(id);
        }

        public override int Delete(Expression<Func<UserEntity, bool>> filter)
        {
            //TODO:??????????????
            ServiceLocator.Current.Resolve<ILog>().Warn(String.Format("删除用户??{0}", filter));

            return base.Delete(filter);
        }

        #endregion
    }
}
