using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IFeedbackRepository : IRepository<FeedbackEntity, int>
    {
        List<FeedbackEntity> GetPagedList(PagerRequest pagerRequest, out int totalCount, FeedbackSortOrder sortOrder);
    }
}
