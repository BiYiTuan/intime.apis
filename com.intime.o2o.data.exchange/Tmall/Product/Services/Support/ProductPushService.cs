using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;

using com.intime.o2o.data.exchange.Tmall.Core;
using com.intime.o2o.data.exchange.Tmall.Core.Support;
using com.intime.o2o.data.exchange.Tmall.Product.Mappers;
using com.intime.o2o.data.exchange.Tmall.Product.Mappers.Support;
using com.intime.o2o.data.exchange.Tmall.Product.Models;
using com.intime.o2o.data.exchange.Tmall.Product.Tools;
using Common.Logging;
using Top.Api;
using Top.Api.Request;
using Top.Api.Util;
using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;
using System.Collections.Generic;
using System.Net.Http;

namespace com.intime.o2o.data.exchange.Tmall.Product.Services.Support
{
    /// <summary>
    /// 商品推送服务
    /// </summary>
    public class ProductPushService : IProductPushService
    {
        #region fileds

        private readonly ITopClientFactory _topClientFactory = new DefaultTopClientFactory();
        private readonly IBrandMapper _brandMapper = new DefaultBrandMapper();
        private readonly ICategoryMapper _categoryMapper = new DefaultCategoryMapper();
        private readonly IProductMapper _productMapper = new DefaultProductMapper();
        private readonly IItemMapper _itemMapper = new DefaultItemMapper();
        private readonly ISchemaMapper _schemaMapper = new DefaultSchemaMapper();
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #endregion

        #region .ctor

        public ProductPushService()
        {

        }

        public ProductPushService(ITopClientFactory topClientFactory, IBrandMapper brandMapper, ICategoryMapper categoryMapper, IProductMapper productMapper, ISchemaMapper schemaMapper, IItemMapper itemMapper)
        {
            _topClientFactory = topClientFactory;
            _brandMapper = brandMapper;
            _categoryMapper = categoryMapper;
            _productMapper = productMapper;
            _schemaMapper = schemaMapper;
            _itemMapper = itemMapper;
        }

        #endregion

        #region 产品相关接口

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="productSchema">商品实体</param>
        /// <param name="consumerKey">消费者Key</param>
        /// <returns>添加状态报告</returns>
        public ResultInfo<long> AddProduct(ESProduct productSchema, string consumerKey)
        {
            /**================================================
             * 品牌映射关系
             ==================================================*/
            var brandId = _brandMapper.ToChannel(productSchema.Brand.Id);

            if (brandId == null)
            {
                var errorMessage = string.Format("商品品牌没有映射，productId:{0},brandId:{1}", productSchema.Id,
                    productSchema.Brand.Id);
                Log.Error(errorMessage);
                return Error<long>(errorMessage, "-10001");
            }

            /**================================================
            * 分类的映射关系
            ==================================================*/
            var categoryId = _categoryMapper.ToChannel(productSchema.Tag.Id);

            if (categoryId == null)
            {
                var errorMessage = string.Format("商品分类没有映射，productId:{0},categoryId:{1}", productSchema.Id,
                    productSchema.Tag.Id);
                Log.Error(errorMessage);
                return Error<long>(errorMessage, "-10002");
            }

            /**================================================
             * 模版组装校验
             *  // 创建templatekey,{模版名称}+{分类}+{品牌}
            ==================================================*/

            var templateKey = string.Format("tmall.schema.product.{0}.{1}", categoryId, brandId);

            if (!_schemaMapper.ExistsTemplate(templateKey))
            {
                var errorMessage = string.Format("产品上传数据模版不存在，template:{0}", templateKey);
                Log.Error(errorMessage);
                return Error<long>(errorMessage, "-10003");
            }

            // 根据门店获取Client
            var topClient = _topClientFactory.Get(consumerKey);

            // 获取最大5张图片处理
            var imgs = UploadImgs(productSchema.Resource, 1, topClient);

            // 转化商品数据Schema
            var xmlData = CreateXmlData(templateKey, new Hashtable
            {
                { "product", productSchema },
                {"imgs",imgs},
                {"consumerKey",consumerKey}
            });

            Log.Error(xmlData);

            /**================================================
            * 匹配产品，如果已经添加直接返回ProductId
           ==================================================*/
            var query = new TmallProductSchemaMatchRequest
            {
                CategoryId = categoryId,
                Propvalues = xmlData
            };

            var rsp = topClient.Execute(query, _topClientFactory.GetSessionKey(consumerKey));

            Log.InfoFormat("Match产品返回结果,{0}-{1}", rsp.ErrMsg, rsp.SubErrMsg);

            if (!rsp.IsError)
            {
                Log.Error("**********");
                Log.Error(rsp.MatchResult);
                var matchedProductId = rsp.MatchResult;
                if (!string.IsNullOrEmpty(matchedProductId))
                {
                    // 保存上传商品和成功后的商品Id的关系
                    var pId = Convert.ToInt64(matchedProductId);
                    _productMapper.Save(productSchema.Id, pId);

                    return Ok(pId);
                }
            }

            /**================================================
            * 添加新商品
            ==================================================*/

            // 请求Taobao接口
            var result = topClient.Execute(new TmallProductSchemaAddRequest()
            {
                BrandId = brandId,
                CategoryId = categoryId,
                XmlData = xmlData
            }, _topClientFactory.GetSessionKey(consumerKey));


            Log.Error(string.Format("{0}-{1}", result.ErrMsg, result.SubErrMsg));
            if (result.IsError)
            {
                return Error<long>(string.Format("{0}-{1}", result.ErrMsg, result.SubErrMsg), result.ErrCode);
            }


            // 解析XMl返回结果提取productId
            var root = XElement.Load(new StringReader(result.AddProductResult));

            var field = (from item in root.Elements("field")
                         where item.Attribute("id").Value == "product_id"
                         select item).FirstOrDefault();

            if (field == null)
            {
                var errorMessage = string.Format("解析productId出错，result:{0}", result.AddProductResult);
                Log.Error(errorMessage);
                return Error<long>(errorMessage, "-10004");
            }

            var productId = Convert.ToInt64(field.Value);

            // 保存上传商品和成功后的商品Id的关系
            _productMapper.Save(productSchema.Id, productId);

            return Ok(productId);
        }

        [Obsolete("暂时先不使用")]
        public ResultInfo<bool> UpdateProduct(ESProduct productSchema, string consumerKey)
        {
            #region

            /**================================================
             * 获取商品Id映射关系
             ==================================================*/
            var productId = _productMapper.ToChannel(productSchema.Id);

            if (productId == null)
            {
                var errorMessage = string.Format("商品Id没有映射，或没有上传商品，productId:{0}", productSchema.Id);
                Log.Error(errorMessage);
                return Error<bool>(errorMessage, "-10005");
            }

            /**================================================
             * 品牌映射关系
             ==================================================*/
            var brandId = _brandMapper.ToChannel(productSchema.Brand.Id);

            if (brandId == null)
            {
                var errorMessage = string.Format("商品品牌没有映射，productId:{0},brandId:{1}", productSchema.Id,
                    productSchema.Brand.Id);
                Log.Error(errorMessage);
                return Error<bool>(errorMessage, "-10001");
            }

            /**================================================
            * 分类的映射关系
            ==================================================*/
            var categoryId = _categoryMapper.ToChannel(productSchema.CategoryId);

            if (categoryId == null)
            {
                var errorMessage = string.Format("商品分类没有映射，productId:{0},cateory:{1}", productSchema.Id,
                    productSchema.CategoryId);
                Log.Error(errorMessage);
                return Error<bool>(errorMessage, "-10002");
            }

            // 创建templatekey,{模版名称}+{分类}+{品牌}
            var templateKey = string.Format("tmall.schema.product.{0}.{1}", categoryId, brandId);

            if (!_schemaMapper.ExistsTemplate(templateKey))
            {
                var errorMessage = string.Format("商品上传数据模版不存在，tempalte:{0}", templateKey);
                Log.Error(errorMessage);
                return Error<bool>(errorMessage, "-10003");
            }

            // 转化商品数据Schema
            var xmlData = CreateXmlData("tmall.schema.product", new Hashtable
            {
                { "product", productSchema }
            });

            // 根据门店获取Client
            var topClient = _topClientFactory.Get(consumerKey);

            // 请求Taobao接口
            var result = topClient.Execute(new TmallProductSchemaUpdateRequest()
            {
                ProductId = productId,
                XmlData = xmlData
            });

            if (result.IsError)
            {
                return Error<bool>(result.SubErrMsg, result.ErrCode);
            }

            return Ok(true);

            #endregion
        }

        #endregion

        #region 商品相关接口

        public ResultInfo<long> AddItem(IEnumerable<ESStock> items, ESProduct product, string consumerKey)
        {
            #region 校验参数

            // 校验参数
            if (product == null)
            {
                var errorMsg = string.Format("产品为空,请检查参数");
                Log.Error(errorMsg);
                return Error<long>(errorMsg, "408");
            }

            if (items == null)
            {
                var errorMsg = string.Format("单品列表为空,productId:{0}", product.Id);
                Log.Error(errorMsg);
                return Error<long>(errorMsg, "408");
            }

            #endregion

            #region 获取productId,categoryId的映射关系


            // 获取产品Id和分类Id
            var productId = _productMapper.ToChannel(product.Id);
            var categoryId = _categoryMapper.ToChannel(product.Tag.Id);

            #endregion

            #region 处理花色和颜色的映射问题

            /**
             * 处理花色和颜色的映射问题
             */
            var esStocks = items as IList<ESStock> ?? items.ToList();

            //TODO:临时修改,解决同款下面花色名称和尺码名称相同问题
            foreach (ESStock t in esStocks)
            {
                t.ColorDesc = string.Format("{0}/{1}", t.ColorDesc, t.ColorValueId);
                t.SizeDesc = string.Format("{0}/{1}", t.SizeDesc, t.SizeValueId);
            }

            var colors = esStocks.Select(p => p.ColorDesc).Distinct().ToList();
            var sizes = esStocks.Select(p => p.SizeDesc).Distinct().ToList();

            #endregion

            #region 商品数量和价格

            //商品总数量
            var total = esStocks.Sum(p => p.Amount);

            // 价格
            var minPrice = esStocks.Min(p => p.Price);

            #endregion

            // 根据门店获取Client
            var topClient = _topClientFactory.Get(consumerKey);

            #region 图片处理
            /**==============================================
            * 获取最大5张图片处理
            ================================================*/
            var resources = UploadImgsForItems(product.Resource, esStocks, topClient);

            var colorResources = resources as ColorResource[] ?? resources.ToArray();

            var imgs = colorResources.Select(p => p.Url).Take(5);

            var colorImgs = new Dictionary<string, string>();

            foreach (var colorImg in colorResources)
            {
                if (string.IsNullOrEmpty(colorImg.ColorDesc)) continue;

                Console.Write("ColorDesc:{0},ColorValueId:{1},Url:{2}", colorImg.ColorDesc, colorImg.ColorValueId, colorImg.Url);

                if (!colorImgs.ContainsKey(colorImg.ColorDesc))
                {
                    colorImgs.Add(colorImg.ColorDesc, colorImg.Url);
                }
            }

            #endregion

            var context = new Hashtable
            {
                {"product", product},
                {"items", items},
                {"consumerKey", consumerKey},
                {"colors", colors},
                {"skuImgs", colorImgs},
                {"sizes", sizes},
                {"total", total},
                {"imgs", imgs},
                {"minPrice", minPrice}
            };

            // 创建templatekey,{模版名称}+{分类}+{品牌}
            var templateKey = string.Format("tmall.schema.item.{0}", categoryId);

            if (!_schemaMapper.ExistsTemplate(templateKey))
            {
                var errorMessage = string.Format("商品上传数据模版不存在，tempalte:{0}", templateKey);
                Log.Error(errorMessage);
                return Error<long>(errorMessage, "-20003");
            }


            // 转化商品数据Schema
            var xmlData = CreateXmlData(templateKey, context);

            Log.Error(xmlData);
            // 请求Taobao接口
            var result = topClient.Execute(new TmallItemSchemaAddRequest()
            {
                ProductId = productId,
                CategoryId = categoryId,
                XmlData = xmlData
            }, _topClientFactory.GetSessionKey("intime"));

            Log.Error(result.ErrMsg);

            if (result.IsError)
            {
                return Error<long>(string.Format("{0}-{1}", result.ErrMsg, result.SubErrMsg), result.ErrCode);
            }

            // 转化itemId
            var itemId = Convert.ToInt64(result.AddItemResult);
            //TODO:tianjia item save

            return Ok(itemId);
        }

        public ResultInfo<bool> UpdateItem(IEnumerable<ESStock> items, ESProduct product, string consumerKey)
        {
            #region 校验参数

            // 校验参数
            if (product == null)
            {
                var errorMsg = string.Format("产品为空,请检查参数");
                Log.Error(errorMsg);
                return Error<bool>(errorMsg, "2408");
            }

            if (items == null)
            {
                var errorMsg = string.Format("单品列表为空,productId:{0}", product.Id);
                Log.Error(errorMsg);
                return Error<bool>(errorMsg, "2408");
            }

            #endregion

            #region 获取productId,categoryId的映射关系


            // 获取产品Id和分类Id
            var productId = _productMapper.ToChannel(product.Id);
            var categoryId = _categoryMapper.ToChannel(product.Tag.Id);
            var itemId = _itemMapper.ToChannel(product.Id);

            #endregion

            #region 处理花色和颜色的映射问题

            /**
             * 处理花色和颜色的映射问题
             */
            var esStocks = items as IList<ESStock> ?? items.ToList();

            //TODO:临时修改,解决同款下面花色名称和尺码名称相同问题
            foreach (ESStock t in esStocks)
            {
                t.ColorDesc = string.Format("{0}/{1}", t.ColorDesc, t.ColorValueId);
                t.SizeDesc = string.Format("{0}/{1}", t.SizeDesc, t.SizeValueId);
            }

            var colors = esStocks.Select(p => p.ColorDesc).Distinct().ToList();
            var sizes = esStocks.Select(p => p.SizeDesc).Distinct().ToList();

            #endregion

            #region 商品数量和价格

            //商品总数量
            var total = esStocks.Sum(p => p.Amount);

            // 价格
            var minPrice = esStocks.Min(p => p.Price);

            #endregion

            // 根据门店获取Client
            var topClient = _topClientFactory.Get(consumerKey);

            #region 图片处理
            /**==============================================
            * 获取最大5张图片处理
            ================================================*/
            var resources = UploadImgsForItems(product.Resource, esStocks, topClient);

            var colorResources = resources as ColorResource[] ?? resources.ToArray();

            var imgs = colorResources.Select(p => p.Url).Take(5);

            var colorImgs = new Dictionary<string, string>();

            foreach (var colorImg in colorResources)
            {
                if (string.IsNullOrEmpty(colorImg.ColorDesc)) continue;

                Console.Write("ColorDesc:{0},ColorValueId:{1},Url:{2}", colorImg.ColorDesc, colorImg.ColorValueId, colorImg.Url);

                if (!colorImgs.ContainsKey(colorImg.ColorDesc))
                {
                    colorImgs.Add(colorImg.ColorDesc, colorImg.Url);
                }
            }

            #endregion

            var context = new Hashtable
            {
                {"product", product},
                {"items", items},
                {"consumerKey", consumerKey},
                {"colors", colors},
                {"skuImgs", colorImgs},
                {"sizes", sizes},
                {"total", total},
                {"itemId",itemId},
                {"imgs", imgs},
                {"minPrice", minPrice}
            };

            // 创建templatekey,{模版名称}+{分类}+{品牌}
            var templateKey = string.Format("tmall.schema.item.{0}", categoryId);

            if (!_schemaMapper.ExistsTemplate(templateKey))
            {
                var errorMessage = string.Format("商品更新数据模版不存在，tempalte:{0}", templateKey);
                Log.Error(errorMessage);
                return Error<bool>(errorMessage, "-20003");
            }


            // 转化商品数据Schema
            var xmlData = CreateXmlData(templateKey, context);

            Log.Error(xmlData);
            // 请求Taobao接口
            var result = topClient.Execute(new TmallItemSchemaUpdateRequest()
            {
                ItemId = itemId,
                ProductId = productId,
                CategoryId = categoryId,
                XmlData = xmlData
            }, _topClientFactory.GetSessionKey("intime"));

            Log.Error(result.ErrMsg);

            if (result.IsError)
            {
                return Error<bool>(string.Format("{0}-{1}", result.ErrMsg, result.SubErrMsg), result.ErrCode);
            }

            var updateResult = Convert.ToBoolean(result.UpdateItemResult);

            return Ok(updateResult);
        }

        #endregion

        #region 帮助方法

        private string CreateXmlData(string schemaName, Hashtable proprties)
        {
            /**================================================
            * 添加公共方法,用于所有模版使用
            ================================================*/
            proprties.Add("_brand", _brandMapper);
            proprties.Add("_category", _categoryMapper);
            proprties.Add("_product", _productMapper);
            proprties.Add("_util", new Tools.UtilTool());
            proprties.Add("_store", new StoreTool());
            proprties.Add("_tag", new TagTool());

            // 转化商品数据Schema
            return _schemaMapper.Map(schemaName, proprties);
        }

        private ResultInfo<T> Error<T>(string errMsg, string errCode)
        {
            return new ResultInfo<T>()
            {
                ErrMsg = errMsg,
                ErrCode = errCode
            };
        }

        private ResultInfo<T> Ok<T>(T data)
        {
            return new ResultInfo<T>()
            {
                Data = data
            };
        }

        #region 图片处理

        /// <summary>
        /// 处理商品资源问题并进行上传
        /// </summary>
        /// <param name="resources">资源列表</param>
        /// <param name="topN">处理图片条数</param>
        /// <param name="topClient"></param>
        /// <returns></returns>
        private IEnumerable<string> UploadImgs(IEnumerable<ESResource> resources, int topN, ITopClient topClient)
        {
            var imgs = new List<string>();

            var pictures = resources.Where(p => p.Status == 1).OrderByDescending(p => p.SortOrder).Take(topN);


            foreach (var resource in pictures)
            {
                try
                {
                    var path = FetchRemotePic(resource);
                    var img = UploadImg2Tmall(path,
                        topClient);

                    if (img != null)
                    {
                        imgs.Add(img);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return imgs;
        }

        private IEnumerable<ColorResource> UploadImgsForItems(IEnumerable<ESResource> resources, IList<ESStock> items, ITopClient topClient)
        {
            var imgs = new List<ColorResource>();

            var sourcePictures =
                resources.Where(r => r.Status == 1).OrderByDescending(x => x.SortOrder).ThenBy(x => x.ColorId);

            foreach (var resource in sourcePictures)
            {
                try
                {
                    var path = FetchRemotePic(resource);
                    var img = UploadImg2Tmall(path,
                        topClient);

                    if (img != null)
                    {
                        var extItem = items.FirstOrDefault(p => resource.ColorId.HasValue && p.ColorValueId == resource.ColorId);

                        if (extItem != null)
                        {
                            imgs.Add(new ColorResource()
                            {
                                ColorDesc = extItem.ColorDesc,
                                ColorValueId = extItem.ColorValueId,
                                Url = img
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

            return imgs;
        }


        /// <summary>
        /// 获取远程资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <returns>本地资源地址</returns>
        private static string FetchRemotePic(ESResource resource)
        {
            string exPicDomain = ConfigurationManager.AppSettings["EXPIC_DOMAIN"];

            var url = string.Format("{0}/{1}_640x0.jpg", exPicDomain.TrimEnd('/'), resource.Name);
            Log.ErrorFormat("下载图片:{0}", url);
            var client = new HttpClient();
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tmp", DateTime.Today.ToString("yyyyMMdd"));
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var path = string.Format("{0}/{1}.jpg", directory, Guid.NewGuid());
            client.GetAsync(url)
               .ContinueWith(request =>
               {
                   var response = request.Result;
                   response.EnsureSuccessStatusCode();
                   using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                   {
                       response.Content.CopyToAsync(fileStream).Wait();
                       fileStream.Flush();
                   }
               }).Wait();
            return path;
        }

        private string UploadImg2Tmall(string path, ITopClient topClient)
        {
            // 上传商品到TMall
            var uploadImg = topClient.Execute(new PictureUploadRequest()
            {
                ImageInputTitle = "图片说明",
                Img = new FileItem(path),
                PictureCategoryId = 0,
            }, _topClientFactory.GetSessionKey("intime"));

            if (!uploadImg.IsError)
            {
                Console.WriteLine("upload OK!");
                // 清理上传成功图片
                File.Delete(path);
                return uploadImg.Picture.PicturePath;
            }

            Log.Error(string.Format("{0}-{1}", uploadImg.ErrMsg, uploadImg.SubErrMsg));
            Log.Error("上传图片失败,path:" + path);

            // 清理上传成功图片
            File.Delete(path);
            return null;
        }

        #endregion

        #endregion
    }
}
