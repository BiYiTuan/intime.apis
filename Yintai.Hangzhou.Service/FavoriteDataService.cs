using System;
using Yintai.Architecture.Common.Caching;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Favorite;
using Yintai.Hangzhou.Contract.DTO.Response.Favorite;
using Yintai.Hangzhou.Contract.Favorite;
using Yintai.Hangzhou.Contract.Request.Favorite;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class FavoriteDataService : BaseService, IFavoriteDataService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteDataService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        #region

        private FavoriteCollectionResponse Get(float version, int userId, PagerRequest pagerRequest, CoordinateInfo coordinate, FavoriteSortOrder sortOrder, SourceType sourceType)
        {
           
                  FavoriteCollectionResponse response;
                  int totalCount;
                  if (version >= 2.1)
                  {
                      var entitys = _favoriteRepository.Get(userId, pagerRequest, out totalCount, sortOrder, sourceType);

                      var list = MappingManager.FavoriteCollectionResponseMapping(entitys, coordinate);

                      response = new FavoriteCollectionResponse(pagerRequest, totalCount) { Favorites = list };
                  }
                  else
                  {
                      var entitys = _favoriteRepository.GetPagedList(userId, pagerRequest, out totalCount, sortOrder, sourceType);

                      response = MappingManager.FavoriteCollectionResponseMapping(entitys, coordinate);
                      response.Index = pagerRequest.PageIndex;
                      response.Size = pagerRequest.PageSize;
                      response.TotalCount = totalCount;
                  }

                  return response;
        }

        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ExecuteResult<FavoriteCollectionResponse> GetFavoriteList(FavoriteListRequest request)
        {
            var pagerRequest = new PagerRequest(request.Page, request.PageSize);

            CoordinateInfo coordinate = null;
            if (request.Lng > 0 || request.Lng < 0)
            {
                coordinate = new CoordinateInfo(request.Lng, request.Lat);
            }

            var response = Get(request.Version, request.UserModel.Id, pagerRequest, coordinate,
                                                      request.SortOrder, request.SType);

            return new ExecuteResult<FavoriteCollectionResponse>(response);
        }

        #endregion

        #region Implementation of IFavoriteDataService

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult Create(FavoriteCreateRequest request)
        {
            //return new ExecuteResult();

            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<FavoriteCollectionResponse> GetFavoriteList(GetFavoriteListRequest request)
        {
            return GetFavoriteList((FavoriteListRequest)request);
        }

        public ExecuteResult<FavoriteCollectionResponse> GetDarenFavoriteList(DarenFavoriteListRequest request)
        {
            return GetFavoriteList((FavoriteListRequest)request);
        }

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult Destroy(FavoriteDestroyRequest request)
        {
            var favorEntity = _favoriteRepository.GetItem(request.FavoriteId);
            if (favorEntity == null)
            {
                return new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "没有找到该产品" };
            }

            if (favorEntity.User_Id != request.AuthUid)
            {
                return new ExecuteResult { StatusCode = StatusCode.ClientError, Message = "您没有权限删除他人的收藏" };
            }

            _favoriteRepository.Delete(favorEntity);

            return new ExecuteResult();
        }

        #endregion
    }
}