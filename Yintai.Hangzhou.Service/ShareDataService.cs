using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Share;
using Yintai.Hangzhou.Contract.Share;

namespace Yintai.Hangzhou.Service
{
    public class ShareDataService : BaseService, IShareDataService
    {
        #region Implementation of IShareDataService

        public ExecuteResult Create(ShareCreateRequest request)
        {
            return new ExecuteResult();
            throw new NotImplementedException();
        }

        #endregion
    }
}
