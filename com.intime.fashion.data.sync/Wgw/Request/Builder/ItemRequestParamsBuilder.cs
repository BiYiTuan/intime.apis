using com.intime.fashion.data.sync.Wgw.Request.Builder.Stock;
using com.intime.jobscheduler.Job.Wgw;
using System;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public class ItemRequestParamsBuilder : RequestParamsBuilder
    {
        public ItemRequestParamsBuilder(ISyncRequest request)
            : base(request)
        {

        }
        private ISyncRequest BuildParameters(ProductEntity entity)
        {
            this.BuildLeafClassId(entity.Id);
            this.BuildLabel(entity.Brand_Id);
            this.BuildUpTime();
            this.Build(entity);
            Request.Put("buyLimit", WgwConfigHelper.BuyLimit);
            Request.Put("city", WgwConfigHelper.City);
            Request.Put("price_bin", Math.Ceiling(entity.Price * 100));
            Request.Put("province", WgwConfigHelper.Province);
            Request.Put("marketPrice",
                entity.UnitPrice.HasValue ? Math.Ceiling(entity.UnitPrice.Value * 100) : int.MaxValue);
            Request.Put("defStockId", entity.Id);
            Request.Put("title", entity.Name);
            Request.Put("sendType", "0");
            Request.Put("transportType", "1");
            Request.Put("desc", entity.Description);
            return Request;
        }

        protected virtual void Build(ProductEntity entity)
        {
            this.BuildImages(entity);
            var stock = new StockBuilder(Request, entity);
            stock.BuildStockInfo("stockstr");
        }

        /// <summary>
        /// 构建商品主图，为防止一次上传图片过多导致微购物返回失败，商品只上传一张主图
        /// </summary>
        /// <param name="entity"></param>
        private void BuildImages(ProductEntity entity)
        {
            using (var db = GetDbContext())
            {
                var img =
                    db.Resources.Where(t => t.SourceId == entity.Id && t.Type == 1 && t.SourceType == (int)SourceType.Product && t.Status == (int)DataStatus.Normal)
                        .OrderByDescending(t => t.SortOrder)
                        .Take(1).FirstOrDefault();
                if (img == null)
                {
                    throw new WgwSyncException(string.Format("No image for product: {0}", entity.Id));
                }
                Request.Put("uploadPicInfo1", string.Format("{0}/{1}_320x0.jpg", WgwConfigHelper.Image_BaseUrl, img.Name));
            }
        }

        /// <summary>
        /// 构建上架日期参数
        /// </summary>
        private void BuildUpTime()
        {
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.AddDays(WgwConfigHelper.UpDelay).ToUniversalTime() - st);
            Request.Put("upTime", ((Int64)(t.TotalMilliseconds)).ToString("D"));
        }

        /// <summary>
        /// 构建微购物叶级工业分类Id参数
        /// </summary>
        /// <param name="productId"></param>
        private void BuildLeafClassId(int productId)
        {
            using (var db = GetDbContext())
            {
                var cat =
                    db.Categories.Join(db.ProductMaps.Where(pm => pm.ProductId == productId), o => o.ExCatId,
                        m => m.ChannelCatId, (o, m) => o).FirstOrDefault();
                if (cat == null)
                {
                    //throw new WgwSyncException(string.Format("商品分类不存在"));
                    Request.Put("leafClassId", "123");
                    return;
                }
                var mapping = db.Map4Category.FirstOrDefault(ccm => ccm.CategoryCode == cat.ExCatCode);
                if (mapping == null)
                {
                    Request.Put("leafClassId", "123");
                    return;
                    //throw new WgwSyncException(string.Format("未映射微购物商品分类{0}({1})", cat.Name, cat.ExCatCode));
                }

                Request.Put("leafClassId", mapping.ChannelCategoryId);
            }
        }

        /// <summary>
        /// 构建商品标签参数，当前只支持商品品牌标签
        /// </summary>
        /// <param name="brandId"></param>
        private void BuildLabel(int brandId)
        {
            using (var db = GetDbContext())
            {
                var map = db.Map4Brand.FirstOrDefault(b => b.BrandId == brandId);
                Request.Put("lable",
                    map == null
                        ? string.Format("brands|{0}", "wg138629481702337")
                        : string.Format("brands|{0}", map.ChannelBrandId));
            }
        }

        public override ISyncRequest BuildParameters(object entity)
        {
            var item = entity as ProductEntity;
            if (item != null)
            {
                return BuildParameters(item);
            }
            throw new WgwSyncException("Parameter type should be ProductEntity");
        }
    }
}