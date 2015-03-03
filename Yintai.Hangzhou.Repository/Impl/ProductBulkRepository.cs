using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Yintai.Architecture.Framework;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class ProductBulkRepository:IProductBulkRepository
    {
        private DbContext _context;
        private const string CONNECT_STRING = "YintaiHangzhouContext";
        public ProductBulkRepository(DbContext context)
        {
            _context = context;
        }
        public IEnumerable<Data.Models.ProductStageEntity> FindUploadsByGroupId(int p)
        {
            return _context.Set<ProductStageEntity>().Where(e => e.UploadGroupId == p).AsEnumerable();
        }

        public IEnumerable<Data.Models.ResourceStageEntity> FindUploadImgsByGroupId(int p)
        {
            return _context.Set<ResourceStageEntity>().Where(e => e.UploadGroupId == p).AsEnumerable();
        }

        public T Entry<T>() where T : Architecture.Common.Models.BaseEntity
        {
            return _context.Set<T>().Create();
        }

        public T Insert<T>(T entity) where T : Architecture.Common.Models.BaseEntity
        {
            var newentity = _context.Set<T>().Add(entity);
            this._context.SaveChanges();

            return newentity;
        }

        public IEnumerable<ProductValidateResult> Validate(int customerId, int jobId)
        {
            using (var unWrapContext = new YintaiHangzhouContext())
            {
                var result = unWrapContext.Database.SqlQuery<ProductValidateResult>(@"exec dbo.ProductStageValidate @inUser,@jobId",
                      new[] {new SqlParameter("inUser", customerId)
                         ,new SqlParameter("jobId",jobId)
                });
                return result.ToArray();
            }
        }

        public void BulkInsertProduct(System.Data.DataTable dt, int jobId,IEnumerable<string> cols)
        {
            using (SqlConnection destinationConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECT_STRING].ConnectionString))
            {
                using (var bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    foreach (var col in cols)
                    {
                        bulkCopy.ColumnMappings.Add(col, col);
                    }
                    bulkCopy.DestinationTableName = "dbo.ProductStage";
                    destinationConnection.Open();
                    bulkCopy.WriteToServer(dt);
                }
            }

            var jobEntity = _context.Set<ProductUploadJobEntity>().Find(jobId);
            jobEntity.Status = (int)ProUploadStatus.ProductsOnStage;
            _context.SaveChanges();
        }
        public void BulkInsertProduct(System.Data.DataTable dt, System.Data.DataTable propertyDt, int jobId, IEnumerable<string> cols, IEnumerable<string> pcols)
        {
            if (propertyDt != null && propertyDt.Rows.Count > 0)
            {
                using (SqlConnection destinationConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECT_STRING].ConnectionString))
                {
                    using (var bulkCopy = new SqlBulkCopy(destinationConnection))
                    {
                        foreach (var col in pcols)
                        {
                            bulkCopy.ColumnMappings.Add(col, col);
                        }
                        bulkCopy.DestinationTableName = "dbo.ProductPropertyStage";
                        destinationConnection.Open();
                        bulkCopy.WriteToServer(propertyDt);
                    }
                }
            }
            BulkInsertProduct(dt, jobId, cols);
           
        }
        public IEnumerable<ProductPublishResult> Publish(int customerId, int jobId)
        {
            using (var unWrapContext = new YintaiHangzhouContext())
            {
                return unWrapContext.Database.SqlQuery<ProductPublishResult>(@"exec dbo.ProductStagePublish2 @inUser,@jobId"
                         , new[] { new SqlParameter("inUser",customerId )
                     ,new SqlParameter("jobId",jobId)}).ToList();
            }
        }

        public IEnumerable<T> List<T>(Expression<Func<T, bool>> filter) where T : Architecture.Common.Models.BaseEntity
        {
            return _context.Set<T>().Where(filter).ToList();
        }

        public void BulkDelete(int jobId)
        {
            using (var unWrapContext = new YintaiHangzhouContext())
            {
                unWrapContext.Database.ExecuteSqlCommand(@"exec dbo.ProductBulkDelete @jobId",
                 new[] { new SqlParameter("@jobId", jobId) });
            }
        }

        public T Find<T>(int id) where T : Architecture.Common.Models.BaseEntity
        {
            return _context.Set<T>().Find(id);
        }

        public void Update<T>(T entity) where T : Architecture.Common.Models.BaseEntity
        {
            var old = this._context.Entry(entity);
            if (old.State == EntityState.Detached)
            {
                var entry = Find<T>((int)entity.EntityId);
                entity = Mapper.Map(entity, entry);
            }

            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            this._context.SaveChanges();

        }

        public void Delete<T>(int id) where T : Architecture.Common.Models.BaseEntity
        {
            var entity = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public DbContext Context { get {
            return _context;
        } }
    }
}
