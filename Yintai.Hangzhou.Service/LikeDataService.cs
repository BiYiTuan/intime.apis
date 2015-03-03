using System;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Like;
using Yintai.Hangzhou.Contract.DTO.Response.Like;
using Yintai.Hangzhou.Contract.Like;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class LikeDataService : BaseService, ILikeDataService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserService _userService;

        public LikeDataService(ILikeRepository likeRepository, IUserService userService)
        {
            this._likeRepository = likeRepository;
            this._userService = userService;
        }

        #region Implementation of ILikeDataService

        /// <summary>
        /// 喜欢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<LikeCoutomerResponse> Like(LikeCreateRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            if (request.AuthUid == request.LikedUserId)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您不能关注自己" };
            }

            //检查
            var d = _likeRepository.GetItem(request.AuthUid, request.LikedUserId);
            if (d != null)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您已经关注了" };
            }

            //该用户是否存在
            var likedUser = _userService.Get(request.LikedUserId);
            if (likedUser == null)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            ////检查被喜欢人是否是达人或店长
            //if (likedUser.Level == UserLevel.Daren || ((likedUser.UserRole & UserRole.Manager) != 0))
            //{
                var data = _likeRepository.Insert(new LikeEntity
                {
                    CreatedDate = DateTime.Now,
                    CreatedUser = request.AuthUid,
                    LikedUserId = request.LikedUserId,
                    LikeUserId = request.AuthUid,
                    Status = (int)DataStatus.Normal,
                    Type = 0,
                    UpdatedDate = DateTime.Now,
                    UpdatedUser = request.LikedUserId
                });

                //TODO:LIKEADD
                this._userService.LikeAdd(request.AuthUid, request.LikedUserId);

                var response = MappingManager.LikeInfoResponseMapping(data, LikeType.ILike);

                return new ExecuteResult<LikeCoutomerResponse>(response);
            //}

            //return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您不能关注该用户" };
        }

        /// <summary>
        /// 获取我喜欢的列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<LikeCoutomerCollectionResponse> GetILikeList(GetLikeListRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<LikeCoutomerCollectionResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            int totalCount;
            var data = this._likeRepository.GetPagedListForILike(request.PagerRequest, out totalCount, request.AuthUid,
                                                     request.LikeSortOrder);

            var response = new LikeCoutomerCollectionResponse(request.PagerRequest, totalCount)
                {
                    LikeUsers = MappingManager.LikeInfoResponseMapping(data, LikeType.ILike)
                };

            return new ExecuteResult<LikeCoutomerCollectionResponse>(response);
        }

        /// <summary>
        /// 获取喜欢我的列表（被喜欢）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<LikeCoutomerCollectionResponse> GetLikeMeList(GetLikeListRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<LikeCoutomerCollectionResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            int totalCount;
            var data = this._likeRepository.GetPagedListForLikeMe(request.PagerRequest, out totalCount, request.AuthUid,
                                                     request.LikeSortOrder);

            var response = new LikeCoutomerCollectionResponse(request.PagerRequest, totalCount)
            {
                LikeUsers = MappingManager.LikeInfoResponseMapping(data, LikeType.LikeMe)
            };

            return new ExecuteResult<LikeCoutomerCollectionResponse>(response);
        }

        /// <summary>
        /// 删除喜欢
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<LikeCoutomerResponse> Destroy(LikeDestroyRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }
            var likeEntity = _likeRepository.GetItem(request.AuthUid, request.LikedUserId);

            if (likeEntity == null)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            //当前请求的用户是被关注的人 但不是发起喜欢的用户
            if (likeEntity.LikeUserId != request.AuthUid && likeEntity.LikedUserId == request.AuthUid)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您不能删除关注您的用户" };
            }

            //当前请求的用户 ！= 发起喜欢的用户
            if (likeEntity.LikeUserId != request.AuthUid)
            {
                return new ExecuteResult<LikeCoutomerResponse>(null) { StatusCode = StatusCode.ClientError, Message = "您不能删除其他用户的关注" };
            }

            this._likeRepository.Delete(likeEntity);
            //TODO:LIKED --
            this._userService.LikeSubtract(request.AuthUid, likeEntity.LikedUserId);

            return new ExecuteResult<LikeCoutomerResponse>(MappingManager.LikeInfoResponseMapping(likeEntity, LikeType.ILike));
        }

        #endregion
    }
}