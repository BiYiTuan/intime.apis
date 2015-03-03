using System;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Feedback;
using Yintai.Hangzhou.Contract.Feedback;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Service
{
    public class FeedbackDataService : BaseService, IFeedbackDataService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackDataService(IFeedbackRepository feedbackRepository)
        {
            this._feedbackRepository = feedbackRepository;
        }

        public ExecuteResult Create(FeedbackCreateRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = this._feedbackRepository.Insert(new FeedbackEntity
                 {
                     Contact = String.IsNullOrEmpty(request.Contact) ? String.Empty : request.Contact,
                     Content = String.IsNullOrEmpty(request.Content) ? String.Empty : request.Content,
                     CreatedDate = DateTime.Now,
                     CreatedUser = request.AuthUid,
                     UpdatedDate = DateTime.Now,
                     UpdatedUser = request.AuthUid,
                     Status = (int)DataStatus.Normal,
                     User_Id = request.AuthUid
                 });

            return new ExecuteResult() { StatusCode = StatusCode.Success, Message = "感谢您的意见反馈,谢谢" };
        }
    }
}
