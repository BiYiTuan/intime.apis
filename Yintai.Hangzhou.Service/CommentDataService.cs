using System;
using System.Linq;
using System.Transactions;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Contract.Comment;
using Yintai.Hangzhou.Contract.DTO.Request.Comment;
using Yintai.Hangzhou.Contract.DTO.Response.Comment;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class CommentDataService : BaseService, ICommentDataService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IRemindService _remindService;

        public CommentDataService(ICommentRepository commentRepository, IRemindService remindService)
        {
            this._commentRepository = commentRepository;
            this._remindService = remindService;
        }

        public ExecuteResult<CommentCollectionResponse> GetList(CommentListRequest request)
        {
            int totalCount;
            var data = this._commentRepository.GetPagedList(request.PagerRequest.PageIndex, request.PagerRequest.PageSize,
                                                 out totalCount, request.SortOrder, request.Timestamp, request.SourceId, request.SType);

            var result = new ExecuteResult<CommentCollectionResponse>();
            var response = new CommentCollectionResponse(request.PagerRequest, totalCount)
                {
                    Comments = MappingManager.CommentInfoResponseMapping(data, request.Version).ToList()
                };

            result.Data = response;

            return result;
        }

        public ExecuteResult<CommentInfoResponse> Create(CommentCreateRequest request)
        {
            CommentEntity entity;
            if ((request.Files == null || request.Files.Count == 0) &&
                string.IsNullOrEmpty(request.Content))
            {
                return new ExecuteResult<CommentInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "没有评论内容！" };

            }
            using (var ts = new TransactionScope())
            {
                if (request.SourceType == (int)SourceType.Comment) {
                    var repliedComment = _commentRepository.Get(l => l.Id == request.SourceId).First();
                    entity = _commentRepository.Insert(new CommentEntity() {
                        Content = request.Content,
                        CreatedDate = DateTime.Now,
                        CreatedUser = request.AuthUser.Id,
                        ReplyUser = repliedComment.User_Id,
                        SourceId = repliedComment.SourceId,
                        SourceType = repliedComment.SourceType,
                        Status = 1,
                        UpdatedDate = DateTime.Now,
                        UpdatedUser = request.AuthUser.Id,
                        User_Id = request.AuthUser.Id,
                        ReplyId = request.SourceId
                    });
                }
                else
                {
                    entity = _commentRepository.Insert(new CommentEntity
                       {
                           Content = request.Content,
                           CreatedDate = DateTime.Now,
                           CreatedUser = request.AuthUid,
                           ReplyUser = request.ReplyUser,
                           SourceId = request.SourceId,
                           SourceType = (int)request.SType,
                           Status = 1,
                           UpdatedDate = DateTime.Now,
                           UpdatedUser = request.AuthUid,
                           User_Id = request.AuthUid
                       });
                }
                //处理文件上传
                if (request.Files != null && request.Files.Count > 0)
                {
                    ServiceLocator.Current.Resolve<IResourceService>().Save(request.Files, request.AuthUid, 0, entity.Id, SourceType.CommentAudio);
                }

                ts.Complete();
            }

            return new ExecuteResult<CommentInfoResponse>(MappingManager.CommentInfoResponseMapping(entity, request.Version));
        }

        public ExecuteResult<CommentInfoResponse> Detail(CommentDetailRequest request)
        {
            var entity = this._commentRepository.GetItem(request.CommentId);

            return new ExecuteResult<CommentInfoResponse>(MappingManager.CommentInfoResponseMapping(entity, request.Version));
        }

        public ExecuteResult<CommentInfoResponse> Update(CommentUpdateRequest request)
        {
            var entity = _commentRepository.GetItem(request.CommentId);
            if (entity == null)
            {
                return new ExecuteResult<CommentInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "评论不存在" };
            }

            if (entity.User_Id != request.AuthUid)
            {
                return new ExecuteResult<CommentInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您没有权限修改其他人的评论" };
            }

            entity.Content = request.Content;
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedUser = request.AuthUid;

            using (var ts = new TransactionScope())
            {
                _commentRepository.Update(entity);

                //删除以前的语音
                //处理文件上传
                if (request.Files != null && request.Files.Count > 0)
                {
                    var r = ServiceLocator.Current.Resolve<IResourceService>();
                    var commentResources = r.Get(entity.Id, SourceType.CommentAudio);

                    var list = r.Save(request.Files, request.AuthUid, 0, entity.Id, SourceType.CommentAudio);
                    if (list != null && list.Count > 0)
                    {
                        foreach (var rs in commentResources)
                        {
                            r.Del(rs.Id);
                        }
                    }
                }

                ts.Complete();
            }

            return new ExecuteResult<CommentInfoResponse>(MappingManager.CommentInfoResponseMapping(entity, request.Version));
        }

        public ExecuteResult<CommentInfoResponse> Destroy(CommentDestroyRequest request)
        {
            var entity = this._commentRepository.GetItem(request.CommentId);
            if (entity == null)
            {
                return new ExecuteResult<CommentInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "评论不存在" };
            }

            if (entity.User_Id != request.AuthUid)
            {
                return new ExecuteResult<CommentInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您没有权限删除其他人的评论" };
            }

            using (var ts = new TransactionScope())
            {
                var r = ServiceLocator.Current.Resolve<IResourceService>();
                var commentResources = r.Get(entity.Id, SourceType.CommentAudio);

                foreach (var rs in commentResources)
                {
                    r.Del(rs.Id);
                }

                _commentRepository.Delete(entity);

                ts.Complete();
            }

            return new ExecuteResult<CommentInfoResponse>(MappingManager.CommentInfoResponseMapping(entity, request.Version));
        }

        public ExecuteResult<CommentCollectionResponse> GetListRefresh(CommentRefreshRequest request)
        {
            var data = this._commentRepository.GetList(request.PagerRequest.PageSize, request.SortOrder,
                                                           request.Timestamp, request.SourceId, request.SType);

            var result = new ExecuteResult<CommentCollectionResponse>();
            var response = new CommentCollectionResponse(request.PagerRequest)
            {
                Comments = MappingManager.CommentInfoResponseMapping(data, request.Version).ToList()
            };
            result.Data = response;

            return result;
        }
    }
}
