using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Yintai.Architecture.Common.Caching;
using Yintai.Architecture.Common.Helper;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Store;
using Yintai.Hangzhou.Contract.DTO.Response.Store;
using Yintai.Hangzhou.Contract.Store;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class StoreDataService : BaseService, IStoreDataService
    {
        private readonly IStoreRepository _storeRepsitory;

        public StoreDataService(IStoreRepository storeRepository)
        {
            this._storeRepsitory = storeRepository;
        }

        #region Implementation of IStoreService

        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ExecuteResult<StoreInfoResponse> GetStore(StoreRequest request)
        {

            var entity = _storeRepsitory.GetItem(request.StoreId);
            var response = MappingManager.StoreResponseMapping(entity);


            var result = new ExecuteResult<StoreInfoResponse>
                {
                    Data = response
                };

            return result;
        }

        public ExecuteResult<List<StoreInfoResponse>> GetAll(StoreGetAllRequest request)
        {

            var entities = _storeRepsitory.GetListForAll();
            var r = MappingManager.StoreResponseMapping(entities, request.CoordinateInfo).ToList();


            return new ExecuteResult<List<StoreInfoResponse>>(r);



            //var entities = this._storeRepsitory.GetListForAll();

            //return new ExecuteResult<List<StoreInfoResponse>>(MappingManager.StoreResponseMapping(entities, request.CoordinateInfo).ToList());
        }

        public ExecuteResult<List<StoreInfoResponse>> GetRefresh(StoreGetRefreshRequest request)
        {
            var entities = this._storeRepsitory.GetListForRefresh(request.Timestamp);

            return new ExecuteResult<List<StoreInfoResponse>>(MappingManager.StoreResponseMapping(entities, request.CoordinateInfo).ToList());
        }

        public ExecuteResult<StoreInfoResponse> CreateStore(StoreCreateRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<StoreInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = MappingManager.StoreEntityMapping(request);

            entity = this._storeRepsitory.Insert(entity);

            return new ExecuteResult<StoreInfoResponse>(MappingManager.StoreResponseMapping(entity));
        }

        public ExecuteResult<StoreInfoResponse> UpdateStore(StoreUpdateRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<StoreInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = this._storeRepsitory.GetItem(request.StoreId);
            if (entity == null)
            {
                return new ExecuteResult<StoreInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到指定storeid" };
            }

            var source = MappingManager.StoreEntityMapping(request);
            // source => target
            source.CreatedDate = entity.CreatedDate;
            source.CreatedUser = entity.CreatedUser;
            source.Status = (int)DataStatus.Normal;

            MappingManager.StoreEntityMapping(source, entity);

            this._storeRepsitory.Update(entity);

            return new ExecuteResult<StoreInfoResponse>(MappingManager.StoreResponseMapping(entity));
        }

        public ExecuteResult<StoreInfoResponse> DestroyStore(StoreDestroyRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<StoreInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = this._storeRepsitory.GetItem(request.StoreId);
            if (entity == null)
            {
                return new ExecuteResult<StoreInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到指定storeid" };
            }

            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedUser = request.AuthUid;
            entity.Status = (int)DataStatus.Normal;

            this._storeRepsitory.Delete(entity);

            return new ExecuteResult<StoreInfoResponse>(MappingManager.StoreResponseMapping(entity));
        }

        /// <summary>
        /// 获取店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StoreModel Get(int id)
        {
            var entity = this._storeRepsitory.GetItem(id);
            if (entity == null)
            {
                return null;
            }

            return MappingManager.StoreModelMapping(entity);
        }

        /// <summary>
        /// 获取店铺列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<StoreModel> GetList(List<int> ids)
        {
            var entities = this._storeRepsitory.GetListByIds(ids);
            if (entities == null || entities.Count == 0)
            {
                return new List<StoreModel>(0);
            }

            return MappingManager.StoreModelMapping(entities).ToList();
        }

        #endregion
    }
}
