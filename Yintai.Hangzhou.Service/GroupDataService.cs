using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Contract.Group;
using Yintai.Hangzhou.Model;

namespace Yintai.Hangzhou.Service
{
    public class GroupDataService:BaseService,IGroupDataService
    {
        #region Implementation of IGroupDataService

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public GroupModel Get(int groupId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<GroupModel> GetList(List<int> ids)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
