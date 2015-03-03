using System.Linq.Expressions;
using com.intime.fashion.data.sync.Wgw.Request.Item;
using Common.Logging;
using System;
using System.Linq;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Executor
{
    public class BrandSyncExecutor : ExecutorBase
    {
        private int _pageNo = 0;
        private int _succeedCount = 0;
        private int _failedCount = 0;

        public BrandSyncExecutor(DateTime benchTime, ILog logger)
            : base(benchTime, logger)
        {
        }

        protected override int SucceedCount
        {
            get
            {
                return _succeedCount;
            }
        }
        protected override int FailedCount { get { return _failedCount; } }

        protected override void ExecuteCore(dynamic extraParameter = null)
        {
            LoadAndMapBrand();
        }

        private void LoadAndMapBrand()
        {
            var request = new GetBrandListRequest();
            request.Put("pageSize", 800);
            request.Put("pageNo", _pageNo++);
            var rsp = Client.Execute<dynamic>(request);
            bool succeed = rsp.errorCode == 0;
            if (!succeed)
            {
                Logger.Error(string.Format("Failed to load brand info {0}", rsp.errorMessage));
            }
            else
            {
                int cnt = rsp.vecBrandInfo.Count;
                while (cnt > 0 && succeed)
                {
                    foreach (var brand in rsp.vecBrandInfo)
                    {
                        try
                        {
                            if (Map4BrandEntity(brand))
                            {
                                _succeedCount += 1;
                            }
                            else
                            {
                                _failedCount += 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            _failedCount += 1;
                            Logger.Error(ex);
                        }
                    }
                    request.Remove("sign");
                    request.Put("pageNo", _pageNo++);
                    var response = Client.Execute<dynamic>(request);
                    cnt = response.vecBrandInfo.Count;
                    succeed = response.errorCode == 0;
                }
            }
        }

        private bool Map4BrandEntity(dynamic brand)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                string wgAppid = brand.wg_appid;
                string brandName = brand.brandname;
                brandName = brandName.Trim().ToUpper();
                string brandalias = brand.brandalias;
                brandalias = brandalias.Trim().ToUpper();
                Expression<Func<BrandEntity, bool>> whereCondition = b => b.Name == brandName;
                whereCondition.Or(m => m.EnglishName == brandName).Or(m => m.Description == brandName);

                if (!string.IsNullOrEmpty(brandalias))
                {
                    whereCondition.Or(m => m.Name == brandalias)
                        .Or(m => m.Description == brandalias)
                        .Or(m => m.EnglishName == brandalias);
                }

                var brands =
                    db.Brands.Where(whereCondition);

                if (brands.Any())
                {
                    foreach (var b in brands)
                    {
                        MapBrand(b, wgAppid);
                    }

                    return true;
                }

                return false;
            }
        }

        private void MapBrand(BrandEntity brand, string wgAppid)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                if (db.Map4Brand.Any(t =>t.BrandId == brand.Id && t.ChannelBrandId == wgAppid &&t.Channel == ConstValue.WGW_CHANNEL_NAME))
                {
                    //Logger.Info(string.Format("品牌 {0} ID = {1} 已映射", brand.Name, wgAppid));
                    return ;
                }

                db.Map4Brand.Add(new Map4BrandEntity()
                {
                    Channel = ConstValue.WGW_CHANNEL_NAME,
                    BrandId = brand.Id,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    ChannelBrandId = wgAppid,
                });
                Logger.Info(string.Format("Map brand ID = {0}To WGW:{1}", brand.Id, wgAppid));
                db.SaveChanges();
            }
        }
    }
}
