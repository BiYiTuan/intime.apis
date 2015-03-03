using System;
using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ICustomerRepository : IRepository<UserEntity, int>
    {
        /// <summary>
        /// 获取指定的用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<UserEntity> GetListByIds(List<int> ids);

        /// <summary>
        /// 获取制定item,name不区分大小写
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserEntity GetItem(string name, string password);

        /// <summary>
        /// 顾客分页
        /// </summary>
        /// <param name="pagerRequest">page</param>
        /// <param name="totalCount">记录数</param>
        /// <param name="sortOrder">排序方式</param>
        /// <returns></returns>
        List<UserEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, CustomerSortOrder sortOrder);

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="pagerRequest"></param>
        /// <param name="totalCount"></param>
        /// <param name="sortOrder"></param>
        /// <param name="nickName">前缀搜索</param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        List<UserEntity> GetPagedListForNickName(PagerRequest pagerRequest, out int totalCount,
                                       CustomerSortOrder sortOrder, string nickName, string mobile);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dateTime"></param>
        int SetLoginDate(int userId, DateTime dateTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="binded"></param>
        int SetCardBinded(int userId, bool? binded);
    }
}
