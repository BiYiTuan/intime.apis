using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Feedback;

namespace Yintai.Hangzhou.Contract.Feedback
{
    public interface IFeedbackDataService
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult Create(FeedbackCreateRequest request);
    }
}
