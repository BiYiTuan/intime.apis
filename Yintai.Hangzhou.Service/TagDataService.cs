using System;
using System.Collections.Generic;
using System.Linq;
using Yintai.Architecture.Common.Caching;
using Yintai.Architecture.Common.Helper;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Tag;
using Yintai.Hangzhou.Contract.DTO.Response.Tag;
using Yintai.Hangzhou.Contract.Tag;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class TagDataService : BaseService, ITagDataService
    {
        private readonly ITagRepository _tagRepository;

        public TagDataService(ITagRepository tagRepository)
        {
            this._tagRepository = tagRepository;
        }

        public ExecuteResult<TagInfoResponse> Get(TagGetRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<TagInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entity = this._tagRepository.GetItem(request.TagId);

            return new ExecuteResult<TagInfoResponse>(MappingManager.TagInfoResponseMapping(entity));
        }

        public ExecuteResult<List<TagInfoResponse>> GetAll(TagGetAllRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<List<TagInfoResponse>>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }



            var entities = _tagRepository.GetListForAll();
            var r = MappingManager.TagInfoResponseMapping(entities).ToList();


            return new ExecuteResult<List<TagInfoResponse>>(r);

            //var entities = this._tagRepository.GetListForAll();

            //            return new ExecuteResult<List<TagInfoResponse>>(MappingManager.TagInfoResponseMapping(entities).ToList());

        }

        public ExecuteResult<List<TagInfoResponse>> GetRefresh(TagGetRefreshRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<List<TagInfoResponse>>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var entities = this._tagRepository.GetListForRefresh(request.Timestamp);

            return new ExecuteResult<List<TagInfoResponse>>(MappingManager.TagInfoResponseMapping(entities).ToList());
        }

        public ExecuteResult<TagInfoResponse> Create(TagCreateRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<TagInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            //判断用户权限

            var tagEntity = MappingManager.TagInfoRequestMapping(request);
            tagEntity.CreatedDate = DateTime.Now;
            tagEntity.CreatedUser = request.AuthUid;
            tagEntity.UpdatedDate = DateTime.Now;
            tagEntity.UpdatedUser = request.AuthUid;

            var entity = this._tagRepository.Insert(tagEntity);

            return new ExecuteResult<TagInfoResponse>(MappingManager.TagInfoResponseMapping(entity));
        }

        public ExecuteResult<TagInfoResponse> Update(TagUpdateRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<TagInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var tagOld = this._tagRepository.GetItem(request.TagId);
            if (tagOld == null)
            {
                return new ExecuteResult<TagInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            tagOld.Description = request.Description;
            tagOld.Name = request.Name;
            tagOld.SortOrder = request.SortOrder;
            //tagOld.Status = request.Status;
            tagOld.UpdatedDate = DateTime.Now;
            tagOld.UpdatedUser = request.AuthUid;

            this._tagRepository.Update(tagOld);

            return Get(new TagGetRequest
                {
                    TagId = tagOld.Id
                });
        }

        public ExecuteResult<TagInfoResponse> Destroy(TagDestroyRequest request)
        {
            if (request == null)
            {
                return new ExecuteResult<TagInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            var tag = _tagRepository.GetItem(request.TagId);

            if (tag == null)
            {
                return new ExecuteResult<TagInfoResponse>(null) { StatusCode = StatusCode.ClientError, Message = "参数错误" };
            }

            tag.UpdatedDate = DateTime.Now;
            tag.UpdatedUser = request.AuthUid;
            tag.Status = (int)DataStatus.Deleted;

            _tagRepository.Delete(tag);

            return new ExecuteResult<TagInfoResponse>(MappingManager.TagInfoResponseMapping(tag));
        }
    }
}