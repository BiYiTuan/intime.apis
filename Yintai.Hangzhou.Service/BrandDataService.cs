using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yintai.Architecture.Common.Caching;
using Yintai.Architecture.Common.Helper;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.Brand;
using Yintai.Hangzhou.Contract.DTO.Request.Brand;
using Yintai.Hangzhou.Contract.DTO.Response.Brand;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class BrandDataService : BaseService, IBrandDataService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly Contract.IResourceService _resourceService;

        public BrandDataService(IBrandRepository brandRepository, Contract.IResourceService resourceService)
        {
            this._brandRepository = brandRepository;
            this._resourceService = resourceService;
        }

        public ExecuteResult<BrandInfoResponse> GetBrand(BrandDetailRequest request)
        {
            var entity = this._brandRepository.GetItem(request.BrandId);

            if (entity == null)
            {
                return new ExecuteResult<BrandInfoResponse>(null);
            }

            return new ExecuteResult<BrandInfoResponse>(MappingManager.BrandInfoResponseMapping(entity));
        }

        public ExecuteResult<List<BrandInfoResponse>> GetAll(BrandAllRequest request)
        {
            var entities = _brandRepository.GetListForAll();
            var r = MappingManager.BrandInfoResponseMapping(entities).ToList();

            return new ExecuteResult<List<BrandInfoResponse>>(r);


            //var entities = this._brandRepository.GetListForAll();
            //if (entities == null || entities.Count == 0)
            //{
            //    return new ExecuteResult<List<BrandInfoResponse>>(new List<BrandInfoResponse>(0));
            //}

            //return new ExecuteResult<List<BrandInfoResponse>>(MappingManager.BrandInfoResponseMapping(entities).ToList());
        }

        public ExecuteResult<List<BrandInfoResponse>> GetRefresh(BrandRefreshRequest request)
        {
            var entities = this._brandRepository.GetListForRefresh(request.Timestamp);

            if (entities == null || entities.Count == 0)
            {
                return new ExecuteResult<List<BrandInfoResponse>>(new List<BrandInfoResponse>(0));
            }

            return new ExecuteResult<List<BrandInfoResponse>>(MappingManager.BrandInfoResponseMapping(entities).ToList());
        }

        public ExecuteResult<GroupStructInfoResponse<BrandInfoResponse>> GetAll4Group(BrandAllRequest request)
        {

            var entities = _brandRepository.GetListForAll();
            var r = MappingManager.BrandInfoResponse4GroupMapping(entities);


            return new ExecuteResult<GroupStructInfoResponse<BrandInfoResponse>>(r);
        }

        public ExecuteResult<GroupStructInfoResponse<BrandInfoResponse>> GetRefresh4Group(BrandRefreshRequest request)
        {
            var entities = this._brandRepository.GetListForRefresh(request.Timestamp);

            if (entities == null || entities.Count == 0)
            {
                return new ExecuteResult<GroupStructInfoResponse<BrandInfoResponse>>(null);
            }

            return new ExecuteResult<GroupStructInfoResponse<BrandInfoResponse>>(MappingManager.BrandInfoResponse4GroupMapping(entities));
        }

        public ExecuteResult<BrandInfoResponse> Create(BrandCreateRequest request)
        {
            var entity = MappingManager.BrandEntityMapping(request);
            entity = this._brandRepository.Insert(entity);

            if (request.Files != null && request.Files.Count > 0)
            {
                //保存图片
                var resourceResult = _resourceService.Save(request.Files, request.AuthUid, 0, entity.Id, SourceType.BrandLogo);

                if (resourceResult != null && resourceResult.Count > 1)
                {
                    entity.Logo = Path.Combine(resourceResult[0].Domain, resourceResult[0].Name);
                }
            }

            this._brandRepository.Update(entity);

            return new ExecuteResult<BrandInfoResponse>(MappingManager.BrandInfoResponseMapping(entity));
        }

        public ExecuteResult<BrandInfoResponse> Update(BrandUpdateRequest request)
        {
            var newentity = MappingManager.BrandEntityMapping(request);
            var entity = _brandRepository.GetItem(request.BrandId);
            if (entity == null || newentity == null)
            {
                return new ExecuteResult<BrandInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            //不变的要映射
            newentity.CreatedDate = entity.CreatedDate;
            newentity.CreatedUser = entity.CreatedUser;
            newentity.Logo = entity.Logo;
            newentity.Status = entity.Status;

            MappingManager.BrandEntityMapping(newentity, entity);

            this._brandRepository.Update(entity);

            return new ExecuteResult<BrandInfoResponse>(MappingManager.BrandInfoResponseMapping(entity));
        }

        public ExecuteResult<BrandInfoResponse> AddLogo(BrandLogoAddRequest request)
        {
            //brand
            var brand = _brandRepository.GetItem(request.BrandId);
            if (brand == null)
            {
                return new ExecuteResult<BrandInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var resources = this._resourceService.Get(request.BrandId, SourceType.BrandLogo);

            //LOGO 只有一张
            if (resources != null && resources.Count > 0)
            {
                foreach (var item in resources)
                {
                    _resourceService.Del(item.Id);
                }
            }

            //add
            if (request.Files != null && request.Files.Count > 0)
            {
                //保存图片
                var resourceResult = _resourceService.Save(request.Files, request.AuthUid, 0, brand.Id, SourceType.BrandLogo);

                if (resourceResult != null && resourceResult.Count > 1)
                {
                    brand.Logo = Path.Combine(resourceResult[0].Domain, resourceResult[0].Name);
                    brand.UpdatedDate = DateTime.Now;
                    brand.UpdatedUser = request.AuthUid;
                }
            }
            else
            {
                return new ExecuteResult<BrandInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误,没有找到图片" };
            }

            _brandRepository.Update(brand);

            return new ExecuteResult<BrandInfoResponse>(MappingManager.BrandInfoResponseMapping(brand));
        }

        public ExecuteResult<BrandInfoResponse> DestroyLogo(BrandLogoDestroyRequest request)
        {
            //brand
            var brand = _brandRepository.GetItem(request.BrandId);
            if (brand == null)
            {
                return new ExecuteResult<BrandInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var resources = this._resourceService.Get(request.BrandId, SourceType.BrandLogo);

            if (resources == null || resources.Count == 0)
            {
                return new ExecuteResult<BrandInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "没有找到图片" };
            }

            //LOGO 只有一张

            foreach (var item in resources)
            {
                _resourceService.Del(item.Id);
            }

            brand.Logo = String.Empty;
            brand.UpdatedDate = DateTime.Now;
            brand.UpdatedUser = request.AuthUid;

            this._brandRepository.Update(brand);

            return new ExecuteResult<BrandInfoResponse>(MappingManager.BrandInfoResponseMapping(brand));
        }
    }
}
