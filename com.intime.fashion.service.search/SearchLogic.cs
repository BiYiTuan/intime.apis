using System.Data.Entity;
using com.intime.fashion.common;
using com.intime.fashion.common.config;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.ES;

namespace com.intime.fashion.service.search
{
    public static class SearchLogic
    {
        private static ElasticClient _client = new ElasticClient(new ConnectionSettings(new Uri(ElasticSearchConfigurationSetting.Current.Host))
                                        .SetDefaultIndex(ElasticSearchConfigurationSetting.Current.Index)
                                        .SetMaximumAsyncConnections(10));

        public static void IndexSingle(int id, IndexSourceType type)
        {
            var serviceBase = GetService(type);
            serviceBase.IndexSingle(id);

        }
        public static bool IndexSingle<T>(T source) where T : class
        {
            if (source == null)
            {
                CommonUtil.Log.Info(
                    "source is null");
                return false;
            }
            var client = GetClient();
            var response = client.Index<T>(source);

            if (!response.IsValid)
            {
                var error = response.ConnectionStatus;
                if (error == null)
                    CommonUtil.Log.Error(response);
                else
                {
                    CommonUtil.Log.Error(error.Request);
                    CommonUtil.Log.Error(error.Result);
                }
                return false;
            }

            return true;

        }

        public static bool IndexMany<T>(IEnumerable<T> source) where T : class
        {
            if (source == null ||
                source.Count() <= 0)
                return true;
            var client = GetClient();
            var response = client.IndexMany<T>(source);
            if (!response.IsValid)
            {
                var error = response.ConnectionStatus;
                if (error == null)
                    CommonUtil.Log.Error(response);
                else
                {
                    CommonUtil.Log.Error(error.Request);
                    CommonUtil.Log.Error(error.Result);
                }
                return false;
            }

            return true;
        }
        public static void IndexSingleAsync(int id, IndexSourceType type)
        {
            Task.Factory.StartNew(() =>
            {
                IndexSingle(id, type);
            });
        }
        public static ESServiceBase GetService(IndexSourceType type)
        {
            switch (type)
            {
                case IndexSourceType.Combo:
                    return new ESComboService();
                case IndexSourceType.Product:
                    return new ESProductService();
                case IndexSourceType.Inventory:
                    return new ESInventoryService();
                case IndexSourceType.Brand:
                    return new ESBrandService();
                case IndexSourceType.IMSTag:
                    return new ESIMSTagService();
                case IndexSourceType.Store:
                    return new ESStoreService();
                case IndexSourceType.Group:
                    return new ESGroupService();
                case IndexSourceType.Tag:
                    return new EsTagService();
                default:
                    throw new ArgumentException("type mismatch");
            }
        }

        public static ElasticClient GetClient()
        {
            return _client;
        }



    }

    public class EsTagService : ESServiceSingle<ESTag>
    {
        protected override ESTag entity2Model(int entityId)
        {
            var db = Context;

            var propertyLinq = db.Set<CategoryPropertyEntity>().Where(cp => cp.IsSize == true)
                .Join(db.Set<CategoryPropertyValueEntity>(), o => o.Id, i => i.PropertyId,
                    (o, i) => new { CP = o, CPV = i });
            var prods = db.Set<TagEntity>().Where(x => x.Id == entityId)
                        .GroupJoin(propertyLinq, o => o.Id, i => i.CP.CategoryId, (o, i) => new { C = o, CP = i })
                        .Select(l => new ESTag()
                        {
                            Id = l.C.Id,
                            Name = l.C.Name,
                            Description = l.C.Description,
                            Status = l.C.Status,
                            SortOrder = l.C.SortOrder,
                            SizeType = l.C.SizeType ?? (int)CategorySizeType.FreeInput,
                            Sizes = l.CP.Select(lcp => new ESSize()
                            {
                                Id = lcp.CPV.Id,
                                Name = lcp.CPV.ValueDesc
                            })
                        });
            return prods.FirstOrDefault();
        }
    }
}
