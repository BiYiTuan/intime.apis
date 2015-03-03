using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Model;

namespace Yintai.Hangzhou.Contract.Group
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Hangzhou.Contract.Group
    /// FileName: Group
    ///
    /// Created at 11/14/2012 4:22:39 PM
    /// Description: 
    /// </summary>
    public interface IGroupDataService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        GroupModel Get(int groupId);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<GroupModel> GetList(List<int> ids);
    }
}
