using System;
using System.Linq;
using Yintai.Architecture.Common.Caching;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.Extension;
using Yintai.Hangzhou.Contract.DTO.Response.HotKey;
using Yintai.Hangzhou.Contract.HotWord;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public class HotwordDataService : BaseService, IHotwordDataService
    {
        private readonly IHotWordRepository _repository;

        public HotwordDataService(IHotWordRepository repository)
        {
            _repository = repository;
        }

        public ExecuteResult<HotWordCollectionResponse> GetCollection()
        {

            var response = new HotWordCollectionResponse();
            var groupEntities = _repository.Get(v => v.Status == (int)DataStatus.Normal).Select(v => new
            {
                v.Word,
                v.Type,
                v.SortOrder
            }).GroupBy(v => v.Type).ToList();

            var words = groupEntities.Where(v => v.Key == (int)HotWordType.Words).ToList();
            var brands = groupEntities.Where(v => v.Key == (int)HotWordType.BrandStruct).ToList();

            if (words.Count > 0)
            {
                var t = words[0].OrderByDescending(v => v.SortOrder).Select(v => v.Word).ToList();
                response.Words = t;
            }

            if (brands.Count > 0)
            {
                var t = brands[0].OrderByDescending(v => v.SortOrder).Select(v => JsonExtension.FromJson<BrandWordsInfo>(v.Word)).ToList();

                if (t.Count > 0)
                {
                    response.BrandWords = t;
                }
            }


            var result = new ExecuteResult<HotWordCollectionResponse> { Data = response };

            return result;
        }
    }
}
