using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.intime.jobscheduler.Job.Wgw;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder.Image
{
    public class ImageBuilder
    {
        /// <summary>
        /// 构建商品图片信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="product"></param>
        public void BuildImage(ISyncRequest request, ProductEntity product)
        {
            this.BuildProductImages(product,request);
            this.BuildColorImages(product,request);
        }

        /// <summary>
        /// 构建商品图片
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        private void BuildProductImages(ProductEntity entity, ISyncRequest request)
        {
#if !DEBUG
            using (var db = DbContextHelper.GetDbContext())
            {
                int idx = 1;
                var moreImg = new StringBuilder();
                // reset images for product
                for (int i = 0; i < 5; i++)
                {
                    request.Put(string.Format("uploadPicInfo{0}", ++i), "null");
                }
                request.Put("moreImg", "null");

                var resource =
                    db.Resources.Where(
                        t =>
                            t.SourceId == entity.Id && t.Type == 1 && t.SourceType == (int) SourceType.Product &&
                            t.Status == (int) DataStatus.Normal && t.ColorId.HasValue)
                        .GroupBy(t => t.ColorId, (key, resources) => new{color = key,images = resources}).OrderByDescending(x=>x.images.Count()).FirstOrDefault();

                if (resource == null || !resource.images.Any())
                {
                    throw new WgwSyncException(string.Format("Product {0} has no images",entity.Id));
                }

                foreach (var img in resource.images)
                {
                    var url = string.Format("{0}/{1}_320x0.jpg", WgwConfigHelper.Image_BaseUrl, img.Name);
                    if (idx > 5)
                    {
                        moreImg.Append(url).Append("|");
                    }
                    else
                    {
                        request.Put(string.Format("uploadPicInfo{0}", idx), url);
                    }
                    idx += 1;
                }

                var moreImgStr = moreImg.ToString();
                var len = moreImgStr.Length;
                if (len > 0)
                {
                    request.Put("moreImg", moreImgStr.Substring(0, len - 1));
                }
            }
#else
            request.Put("uploadPicInfo1", "http://ec4.images-amazon.com/images/I/51yQ0l-qvkL._AA135_.jpg");
            request.Put("uploadPicInfo2", "http://ec8.images-amazon.com/images/I/41dBoxFHlyL._AA135_.jpg");
#endif
        }

        /// <summary>
        /// 构建库存和颜色图片
        /// </summary>
        /// <param name="item"></param>
        /// <param name="request"></param>
        private void BuildColorImages(ProductEntity item, ISyncRequest request)
        {
            using (var db = DbContextHelper.GetDbContext())
            {
                var inventories = db.Inventories.Where(i => i.ProductId == item.Id);
                var images = new List<dynamic>();

                foreach (var inventory in inventories)
                {
                    var colorValue = db.ProductPropertyValues.FirstOrDefault(t => t.Id == inventory.PColorId);
                    var color = db.ProductProperties.FirstOrDefault(t => t.Id == colorValue.PropertyId);

                    if (colorValue == null || color == null)
                    {
                        continue;
                    }

                    if (images.Any(x => x.value == colorValue.ValueDesc))
                    {
                        continue;
                    }
                    var img = db.Resources.Where(r => r.SourceId == inventory.ProductId && r.ColorId == inventory.PColorId && r.SourceType == (int)SourceType.Product && r.Status == (int)DataStatus.Normal).OrderByDescending(t=>t.SortOrder).FirstOrDefault();

                    if (img == null)
                    {
                        continue;
                    }
                    images.Add(new { property = color.PropertyDesc, value = colorValue.ValueDesc, url = string.Format("{0}/{1}_120x0.jpg", WgwConfigHelper.Image_BaseUrl, img.Name) });

                }
#if !DEBUG
                var sb = new StringBuilder();
                var colorImgStr = images.Aggregate(sb, (s, img) => sb.AppendFormat("{0}:{1}|{2};", img.property, img.value, img.url),
                   imgs => sb.ToString());

                if (colorImgStr.Length > 0)
                {
                    request.Put("stockAttrImgs", colorImgStr.Substring(0, colorImgStr.Length - 1));
                }
#else
                request.Put("stockAttrImgs", "颜色:花色|http://ec4.images-amazon.com/images/I/51%2B6RQwwhkL._SS45_.jpg;颜色:黑色|http://g-ec4.images-amazon.com/images/G/28/fanting_3P/shoes/123._SS75_V363140790_.jpg;颜色:绿色|http://ec4.images-amazon.com/images/I/41NZt4fsPrL._SS45_.jpg");
#endif
            }
        }
    }
}
