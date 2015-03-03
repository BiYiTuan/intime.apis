using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IProductBulkRepository
    {
        IEnumerable<ProductStageEntity> FindUploadsByGroupId(int p);

        IEnumerable<ResourceStageEntity> FindUploadImgsByGroupId(int p);

        T Entry<T>() where T:BaseEntity;

        T Insert<T>(T entity) where T:BaseEntity;

        IEnumerable<ProductValidateResult> Validate(int customerId, int jobId);

        void BulkInsertProduct(System.Data.DataTable dt, int jobId,IEnumerable<string> cols);

        IEnumerable<ProductPublishResult> Publish(int customerId, int jobId);

        IEnumerable<T> List<T>(Expression<Func<T, bool>> filter) where T:BaseEntity;

        void BulkDelete(int jobId);

        T Find<T>(int id) where T:BaseEntity;

        void Update<T>(T entity) where T:BaseEntity;

        void Delete<T>(int id) where T:BaseEntity;

        void BulkInsertProduct(System.Data.DataTable dt, System.Data.DataTable propertyDt, int jobId, IEnumerable<string> cols,IEnumerable<string> pcols);

        DbContext Context { get; }
    }
}
