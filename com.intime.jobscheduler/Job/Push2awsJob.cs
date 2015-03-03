using Common.Logging;
using Nest;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Diagnostics;
using System.Linq;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;

using com.intime.fashion.common;
using com.intime.fashion.service;
using com.intime.fashion.common.config;
using com.intime.fashion.service.search;


namespace com.intime.jobscheduler.Job
{
    [DisallowConcurrentExecution]
    public class Push2awsJob : IJob
    {
        private bool _isActiveOnly = false;

        public void Execute(IJobExecutionContext context)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            JobDataMap data = context.JobDetail.JobDataMap;

            var needRebuild = data.ContainsKey("needrebuild") ? data.GetBooleanValue("needrebuild") : false;

            var benchDate = BenchDate(context);

            var client = SearchLogic.GetClient();
            if (client == null)
            {
                log.Info("client create faile");
                return;
            }

            if (needRebuild)
            {

                _isActiveOnly = true;
            }
            IndexGroup(benchDate);
            IndexBrand(client, benchDate);
            IndexHotwork(client, benchDate);
            IndexStore(client, benchDate);
            IndexTag(client, benchDate);
            IndexResource(client, benchDate);
            IndexProds(client, benchDate, null);
            IndexInventory(client, benchDate);
            //IndexPros(client, benchDate, null);
            IndexIMSTag(client, benchDate);
            IndexStorePromotion(client, benchDate);

        }

        private void IndexIMSTag(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.IMS_Tag.Where(p => p.CreateDate >= benchDate || p.UpdateDate >= benchDate);
                if (_isActiveOnly)
                    linq = linq.Where(p => p.Status == (int)DataStatus.Normal);
                var prods = linq.Select(p => p);

                int totalCount = prods.Count();

                var service = SearchLogic.GetService(IndexSourceType.IMSTag);

                while (cursor < totalCount)
                {
                    foreach (var target in prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size))
                    {
                        using (var tls = new ScopedLifetimeDbContextManager())
                        {
                            if (service.IndexSingle(target.Id))
                                successCount++;
                        }
                    }
                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} ims tags in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));
        }

        private void IndexInventory(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var inventories =
                    db.Inventories.Where(x => x.UpdateDate >= benchDate)
                        .GroupBy(x => x.ProductId)
                        .Join(db.Inventories, p => p.Key, i => i.ProductId, (k, i) => i)
                        .Join(db.Products, i => i.ProductId, p => p.Id,
                            (i, p) => new { stock = i, price = p.Price, labelprice = p.UnitPrice })
                            .Join(db.ProductPropertyValues.Where(ppv => ppv.Status == (int)DataStatus.Normal), s => s.stock.PColorId, ppv => ppv.Id, (s, ppv) => new
                            {
                                s.stock,
                                s.price,
                                s.labelprice,
                                color = ppv.ValueDesc
                            }).Join(db.ProductPropertyValues.Where(ppv => ppv.Status == (int)DataStatus.Normal), s => s.stock.PSizeId, ppv => ppv.Id, (s, ppv) => new
                            {
                                s.stock,
                                s.price,
                                s.labelprice,
                                s.color,
                                size = ppv.ValueDesc
                            });


                int totalCount = inventories.Count();
                while (cursor < totalCount)
                {
                    var linq = from l in inventories.OrderByDescending(p => p.stock.Id).Skip(cursor).Take(size).ToList()
                               select new ESStock()
                               {
                                   ProductId = l.stock.ProductId,
                                   Amount = l.stock.Amount,
                                   ColorValueId = l.stock.PColorId,
                                   SizeValueId = l.stock.PSizeId,
                                   Id = l.stock.Id,
                                   LabelPrice = l.labelprice.HasValue ? l.labelprice.Value : 999999,
                                   Price = l.price,
                                   UpdateDate = DateTime.Now,
                                   ColorDesc = l.color,
                                   SizeDesc = l.size
                               };
                    var result = client.IndexMany(linq);
                    if (!result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} inventory in {1} => {2} docs/s", successCount, sw.Elapsed,
                successCount / sw.Elapsed.TotalSeconds));
        }

        private void IndexStorePromotion(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var prods = db.StorePromotions.Where(r => r.CreateDate >= benchDate || r.UpdateDate >= benchDate)
                            .GroupJoin(db.PointOrderRules.Where(r => r.Status != (int)DataStatus.Deleted), o => o.Id, i => i.StorePromotionId,
                                    (o, i) => new { S = o, R = i });

                int totalCount = prods.Count();
                while (cursor < totalCount)
                {
                    var linq = from l in prods.OrderByDescending(p => p.S.Id).Skip(cursor).Take(size).ToList()
                               select new ESStorePromotion().FromEntity<ESStorePromotion>(l.S, s =>
                               {
                                   s.ExchangeRule = JsonConvert.SerializeObject(l.R.Select(r => new
                                   {
                                       rangefrom = r.RangeFrom,
                                       rangeto = r.RangeTo,
                                       ratio = r.Ratio
                                   }));
                               });
                    var result = client.IndexMany(linq);
                    if (!result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} store promotions in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }

        private void IndexResource(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.Resources.AsQueryable(); ;
                if (_isActiveOnly)
                    linq = linq.Where(l => l.Status == (int)DataStatus.Normal);
                var prods = from r in linq
                            where (r.CreatedDate >= benchDate || r.UpdatedDate >= benchDate)
                            select new ESResource()
                            {
                                Id = r.Id,
                                Status = r.Status,
                                Domain = r.Domain,
                                Name = r.Name,
                                SortOrder = r.SortOrder,
                                IsDefault = r.IsDefault,
                                Type = r.Type,
                                Width = r.Width,
                                Height = r.Height,
                                SourceId = r.SourceId,
                                SourceType = r.SourceType,
                                ColorId = r.ColorId
                            };

                int totalCount = prods.Count();

                while (cursor < totalCount)
                {
                    var result = client.IndexMany(prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size));
                    if (result != null && !result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} resources in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));
            //update related source if any affected
            if (successCount > 0 && CascadPush)
            {
                //index related products
                log.Info("index products affected by related resources ");
                IndexProds(client, null,
                    (p, db) =>
                    {
                        return p.Where(prod => (from r in db.Resources
                                                where r.SourceId == prod.Id
                                                && (r.CreatedDate >= benchDate || r.UpdatedDate >= benchDate)
                                                && r.SourceType == (int)SourceType.Product
                                                select r.Id).Any());
                    });
                //index related promotions
                log.Info("index promotions affected by related resources ");
                IndexPros(client, null,
                    (p, db) =>
                    {
                        return p.Where(prod => (from r in db.Resources
                                                where r.SourceId == prod.Id
                                                && (r.CreatedDate >= benchDate || r.UpdatedDate >= benchDate)
                                                && r.SourceType == (int)SourceType.Promotion
                                                select r.Id).Any());
                    });
                //index related specialtopics
                log.Info("index specialtopics affected by related resources ");
                IndexSpecialTopic(client, null,
                    (p, db) =>
                    {
                        return p.Where(prod => (from r in db.Resources
                                                where r.SourceId == prod.Id
                                                && (r.CreatedDate >= benchDate || r.UpdatedDate >= benchDate)
                                                && r.SourceType == (int)SourceType.SpecialTopic
                                                select r.Id).Any());
                    });
            }
        }

        private void IndexUser(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var prods = from p in db.Users
                            where (p.CreatedDate >= benchDate || p.UpdatedDate >= benchDate)
                            let resource = from r in db.Resources
                                           where r.SourceId == p.Id && r.SourceType == (int)SourceType.CustomerPortrait
                                           select new ESResource()
                                           {
                                               Domain = r.Domain,
                                               Name = r.Name,
                                               SortOrder = r.SortOrder,
                                               IsDefault = r.IsDefault,
                                               Type = r.Type,
                                               Width = r.Width,
                                               Height = r.Height
                                           }
                            select new ESUser()
                            {
                                Id = p.Id,
                                Status = p.Status,
                                Thumnail = resource.FirstOrDefault(),
                                Nickie = p.Nickname,
                                Level = p.UserLevel

                            };

                int totalCount = prods.Count();

                while (cursor < totalCount)
                {
                    var result = client.IndexMany(prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size));
                    if (!result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} users in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }


        private void IndexHotwork(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.HotWords.AsQueryable();
                if (_isActiveOnly)
                    linq = db.HotWords.Where(b => b.Status == (int)DataStatus.Normal);
                var words = from p in linq
                            where (p.CreatedDate >= benchDate || p.UpdatedDate >= benchDate)
                            select p;
                var prods = from p in words.ToList()
                            select new ESHotword()
                            {
                                Id = p.Id,
                                SortOrder = p.SortOrder,
                                Status = p.Status,
                                Type = p.Type,
                                Word = p.Type == 1 ? p.Word : JsonConvert.DeserializeObject<dynamic>(p.Word).name,
                                BrandId = p.Type == 1 ? 0 : JsonConvert.DeserializeObject<dynamic>(p.Word).id
                            };

                int totalCount = prods.Count();

                while (cursor < totalCount)
                {
                    var result = client.IndexMany(prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size));
                    if (!result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} hotwords in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }

        private void IndexTag(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.Tags.AsQueryable();
                if (_isActiveOnly)
                    linq = linq.Where(p => p.Status == (int)DataStatus.Normal);
                var propertyLinq = db.Set<CategoryPropertyEntity>().Where(cp => cp.IsSize == true)
                                   .Join(db.Set<CategoryPropertyValueEntity>(), o => o.Id, i => i.PropertyId, (o, i) => new { CP = o, CPV = i });
                var prods = linq.Where(p => p.CreatedDate >= benchDate || p.UpdatedDate >= benchDate)
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

                int totalCount = prods.Count();

                while (cursor < totalCount)
                {
                    var result = client.IndexMany(prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size));
                    if (!result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} tags in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }


        private void IndexGroup(DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.Groups.AsQueryable();
                if (_isActiveOnly)
                    linq = linq.Where(p => p.Status == (int)DataStatus.Normal);
                var prods = from s in linq
                            where (s.CreatedDate >= benchDate || s.UpdatedDate >= benchDate)
                            select s;

                int totalCount = prods.Count();

                var service = SearchLogic.GetService(IndexSourceType.Group);

                while (cursor < totalCount)
                {
                    foreach (var target in prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size))
                    {
                        using (var tls = new ScopedLifetimeDbContextManager())
                        {
                            if (service.IndexSingle(target.Id))
                                successCount++;
                        }
                    }
                    cursor += size;
                }


            }
            sw.Stop();
            log.Info(string.Format("{0} stores in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }

        private void IndexStore(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.Stores.AsQueryable();
                if (_isActiveOnly)
                    linq = linq.Where(p => p.Status == (int)DataStatus.Normal);
                var prods = from s in linq
                            where (s.CreatedDate >= benchDate || s.UpdatedDate >= benchDate)
                            select s;

                int totalCount = prods.Count();

                var service = SearchLogic.GetService(IndexSourceType.Store);

                while (cursor < totalCount)
                {
                    foreach (var target in prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size))
                    {
                        using (var tls = new ScopedLifetimeDbContextManager())
                        {
                            if (service.IndexSingle(target.Id))
                                successCount++;
                        }
                    }
                    cursor += size;
                }


            }
            sw.Stop();
            log.Info(string.Format("{0} stores in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }

        private void IndexBrand(ElasticClient client, DateTime benchDate)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.Brands.Where(p => p.CreatedDate >= benchDate || p.UpdatedDate >= benchDate);
                if (_isActiveOnly)
                    linq = linq.Where(p => p.Status == (int)DataStatus.Normal);
                var prods = linq.Select(p => p);

                int totalCount = prods.Count();

                var service = SearchLogic.GetService(IndexSourceType.Brand);

                while (cursor < totalCount)
                {
                    foreach (var target in prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size))
                    {
                        using (var tls = new ScopedLifetimeDbContextManager())
                        {
                            if (service.IndexSingle(target.Id))
                                successCount++;
                        }
                    }
                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} brands in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));
            //update related source if any affected
            if (successCount > 0 && CascadPush)
            {
                //index related products
                log.Info("index products affected by related brands ");
                IndexProds(client, null,
                    (p, db) =>
                    {
                        return p.Where(prod => (from r in db.Brands
                                                where (r.CreatedDate >= benchDate || r.UpdatedDate >= benchDate)
                                                && r.Id == prod.Brand_Id
                                                select r.Id).Any());
                    });
            }
        }


        private void IndexSpecialTopic(ElasticClient client, DateTime? benchDate, Func<IQueryable<SpecialTopicEntity>, YintaiHangzhouContext, IQueryable<SpecialTopicEntity>> whereCondition)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.SpecialTopics.AsQueryable();
                if (_isActiveOnly)
                    linq = linq.Where(s => s.Status == (int)DataStatus.Normal);
                if (benchDate.HasValue)
                    linq = linq.Where(p => p.CreatedDate >= benchDate.Value || p.UpdatedDate >= benchDate.Value);
                else if (whereCondition != null)
                {
                    linq = whereCondition(linq, db);

                }
                var prods = from p in linq
                            let resource = (from r in db.Resources
                                            where r.SourceId == p.Id
                                            && r.SourceType == 9
                                            select new ESResource()
                                            {
                                                Domain = r.Domain,
                                                Name = r.Name,
                                                SortOrder = r.SortOrder,
                                                IsDefault = r.IsDefault,
                                                Type = r.Type,
                                                Width = r.Width,
                                                Height = r.Height
                                            })
                            select new ESSpecialTopic()
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Status = p.Status,
                                CreatedDate = p.CreatedDate,
                                CreateUser = p.CreatedUser,
                                Resource = resource,
                                Type = p.Type,
                                TargetValue = p.TargetValue
                            };

                int totalCount = prods.Count();

                while (cursor < totalCount)
                {
                    var result = client.IndexMany(prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size));
                    if (!result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} special topics in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));
            if (successCount > 0 && CascadPush)
            {
                //index related products
                log.Info("index products affected by related specialtopic ");
                IndexProds(client, null,
                    (p, db) =>
                    {
                        return p.Where(prod => (from pro in db.SpecialTopics
                                                from ppr in db.SpecialTopicProductRelations
                                                where ppr.SpecialTopic_Id == pro.Id
                                                && (pro.CreatedDate >= benchDate || pro.UpdatedDate >= benchDate)
                                                && ppr.Product_Id == prod.Id
                                                select ppr.Product_Id).Any());
                    });
            }
        }
        protected virtual DateTime BenchDate(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap;
            return data.ContainsKey("benchdate") ? data.GetDateTimeValue("benchdate") : DateTime.Today.AddDays(-1);
        }
        protected virtual bool CascadPush
        {
            get
            {
                return false;
            }
        }
        private void IndexPros(ElasticClient client, DateTime? benchDate, Func<IQueryable<PromotionEntity>, YintaiHangzhouContext, IQueryable<PromotionEntity>> whereCondition)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            int successCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.Promotions.AsQueryable();
                if (_isActiveOnly)
                    linq = linq.Where(p => p.Status == (int)DataStatus.Normal);
                if (benchDate.HasValue)
                    linq = linq.Where(p => p.CreatedDate >= benchDate.Value || p.UpdatedDate >= benchDate.Value);
                else if (whereCondition != null)
                {
                    linq = whereCondition(linq, db);

                }
                var prods = from p in linq
                            join s in db.Stores on p.Store_Id equals s.Id
                            let resource = (from r in db.Resources
                                            where r.SourceId == p.Id
                                            && r.SourceType == 2
                                            select new ESResource()
                                            {
                                                Domain = r.Domain,
                                                Name = r.Name,
                                                SortOrder = r.SortOrder,
                                                IsDefault = r.IsDefault,
                                                Type = r.Type,
                                                Width = r.Width,
                                                Height = r.Height
                                            })
                            select new ESPromotion()
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                CreatedDate = p.CreatedDate,
                                StartDate = p.StartDate,
                                EndDate = p.EndDate,
                                FavoriteCount = p.FavoriteCount,
                                IsTop = p.IsTop,
                                Status = p.Status,
                                CreateUserId = p.CreatedUser,
                                Store = new ESStore()
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    Description = s.Description,
                                    Address = s.Location,
                                    Location = new Location
                                    {
                                        Lon = s.Longitude,
                                        Lat = s.Latitude
                                    },
                                    GpsAlt = s.GpsAlt,
                                    GpsLat = s.GpsLat,
                                    GpsLng = s.GpsLng,
                                    Tel = s.Tel
                                },
                                Resource = resource,
                                ShowInList = p.IsMain.HasValue ? p.IsMain.Value : true,
                                PublicCode = p.PublicProCode,
                                InvolvedCount = p.InvolvedCount,
                                IsProdBindable = p.IsProdBindable ?? false,
                                LikeCount = p.LikeCount,
                                PublicationLimit = p.PublicationLimit ?? -1,
                                ShareCount = p.ShareCount
                            };

                int totalCount = prods.Count();


                while (cursor < totalCount)
                {
                    var result = client.IndexMany(prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size));
                    if (!result.IsValid)
                    {
                        foreach (var item in result.Items)
                        {
                            if (string.IsNullOrEmpty(item.Error))
                                successCount++;
                            else
                                log.Info(string.Format("id index failed:{0}", item.Id));
                        }
                    }
                    else
                        successCount += result.Items.Count();

                    cursor += size;
                }

            }
            sw.Stop();
            log.Info(string.Format("{0} promotions in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));
            if (successCount > 0 && CascadPush)
            {
                //index related products
                log.Info("index products affected by related promotions ");
                IndexProds(client, null,
                    (p, db) =>
                    {
                        return p.Where(prod => (from pro in db.Promotions
                                                from ppr in db.Promotion2Product
                                                where ppr.ProId == pro.Id
                                                && (pro.CreatedDate >= benchDate || pro.UpdatedDate >= benchDate)
                                                && ppr.ProdId == prod.Id
                                                select ppr.ProdId).Any());
                    });
            }
        }

        private void IndexProds(ElasticClient client, DateTime? benchDate, Func<IQueryable<ProductEntity>, YintaiHangzhouContext, IQueryable<ProductEntity>> whereCondition)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            int cursor = 0;
            int successCount = 0;
            int size = JobConfig.DEFAULT_PAGE_SIZE;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = new YintaiHangzhouContext("YintaiHangzhouContext"))
            {
                var linq = db.Products.AsQueryable();
                if (_isActiveOnly)
                    linq = linq.Where(p => p.Status == (int)DataStatus.Normal);
                if (benchDate.HasValue)
                    linq = linq.Where(p => p.CreatedDate >= benchDate.Value || p.UpdatedDate >= benchDate.Value);
                else if (whereCondition != null)
                {
                    linq = whereCondition(linq, db);

                }

                var prods = from p in linq
                            select p;

                int totalCount = prods.Count();
                var service = SearchLogic.GetService(IndexSourceType.Product);
                while (cursor < totalCount)
                {
                    foreach (var target in prods.OrderByDescending(p => p.Id).Skip(cursor).Take(size))
                    {
                        using (var tls = new ScopedLifetimeDbContextManager())
                        {
                            if (service.IndexSingle(target.Id))
                                successCount++;
                        }
                    }

                    cursor += size;
                }
            }
            sw.Stop();
            log.Info(string.Format("{0} products in {1} => {2} docs/s", successCount, sw.Elapsed, successCount / sw.Elapsed.TotalSeconds));

        }

    }
}
