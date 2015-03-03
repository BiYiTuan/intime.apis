using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Yintai.Hangzhou.Contract.DTO.Response;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;
using Yintai.Hangzhou.WebSupport.Mvc;
using com.intime.fashion.common.Extension;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.WebSupport.Binder;
using com.intime.fashion.service;
using Yintai.Architecture.Framework.ServiceLocation;
using com.intime.fashion.common.message;
using com.intime.fashion.common.message.Messages;
using com.intime.fashion.service.contract;

namespace Yintai.Hangzhou.WebApiCore.Areas.Ims.Controllers
{
    public class ProductController : RestfulController
    {
        private IResourceService _resourceService;
        private IProductRepository _productRepo;
        private IProductPropertyRepository _productPropertyRepo;
        private IProductPropertyValueRepository _productPropertyValueRepo;
        private IInventoryRepository _inventoryRepo;
        private IResourceRepository _resourceRepo;
        private IEFRepository<ProductCode2StoreCodeEntity> _productCodeRepo;
        private IComboService _comboService;
        private IEFRepository<Product2IMSTagEntity> _productTagRepo;
        public ProductController(IResourceService resourceService
            , IProductRepository productRepo
            , IProductPropertyRepository productPropertyRepo
            , IProductPropertyValueRepository productPropertyValueRepo
            , IInventoryRepository inventoryRepo
            , IResourceRepository resourceRepo
            , IEFRepository<ProductCode2StoreCodeEntity> productCodeRepo
            ,IComboService comboService
            ,IEFRepository<Product2IMSTagEntity> productTagRepo)

        {
            _resourceService = resourceService;
            _productRepo = productRepo;
            _productPropertyRepo = productPropertyRepo;
            _productPropertyValueRepo = productPropertyValueRepo;
            _inventoryRepo = inventoryRepo;
            _resourceRepo = resourceRepo;
            _productCodeRepo = productCodeRepo;
            _comboService = comboService;
            _productTagRepo = productTagRepo;
        }
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Create(Yintai.Hangzhou.Contract.DTO.Request.IMSProductCreateRequest request, int authuid)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            if (request.Image_Ids==null || request.Image_Ids.Length <= 0)
                return this.RenderError(r => r.Message = "必须传入一个图片");
            
            var categoryEntity = Context.Set<TagEntity>().Find(request.Category_Id);
            if (categoryEntity == null)
                return this.RenderError(r => r.Message = "分类不存在");

            if (request.Sizes == null || request.Sizes.Length < 1)
                return this.RenderError(r => r.Message = "必须传入一个尺码");

            var brandEntity = Context.Set<BrandEntity>().Find(request.Brand_Id);
            if (brandEntity == null)
                return this.RenderError(r => r.Message = "品牌不存在");
            
            request.UnitPrice = (!request.UnitPrice.HasValue || request.UnitPrice <= 0) ? request.Price : request.UnitPrice;
            if (request.UnitPrice < request.Price)
                return this.RenderError(r => r.Message = "商品原价必须大于等于销售价！");
            if (string.IsNullOrEmpty(request.Color_Str))
                request.Color_Str = "均色";
            if (string.IsNullOrWhiteSpace(request.Name))
                request.Name  = string.Format("{0}-{1}-{2}", brandEntity.Name, categoryEntity.Name, request.Sku_Code);
            if (string.IsNullOrEmpty(request.Desc))
                request.Desc = request.Name;
            var assocateEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid).First();
            using (var ts = new TransactionScope())
            {
                //step1: create product 
               
                var productEntity = _productRepo.Insert(new ProductEntity()
                {
                    Brand_Id = request.Brand_Id,
                    CreatedDate = DateTime.Now,
                    CreatedUser = authuid,
                    Description = request.Desc,
                    FavoriteCount = 0,
                    InvolvedCount = 0,
                    Is4Sale = true,
                    IsHasImage = true,
                    MoreDesc = string.Empty,
                    Name = request.Name,
                    Price = request.Price,
                    ProductType = (int)ProductType.FromSelf,
                    RecommendedReason = string.Empty,
                    RecommendUser = authuid,
                    SkuCode = request.Sku_Code,
                    SortOrder = 1,
                    Status = (int)DataStatus.Normal,
                    Tag_Id = request.Category_Id,
                    UnitPrice = request.UnitPrice,
                    UpdatedDate = DateTime.Now,
                    UpdatedUser = authuid,
                    Store_Id = assocateEntity.StoreId,
                    Favorable = string.Empty

                });
                _productCodeRepo.Insert(new ProductCode2StoreCodeEntity()
                {
                    Status = (int)DataStatus.Normal,
                    ProductId = productEntity.Id,
                    StoreId = assocateEntity.StoreId,
                    StoreProductCode = request.Sales_Code,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authuid,
                    SectionId = assocateEntity.SectionId
                });
                if (request.Tag_Ids != null &&
                    request.Tag_Ids.Length > 0)
                {
                    foreach (var tag in request.Tag_Ids)
                    {
                        _productTagRepo.Insert(new Product2IMSTagEntity() { 
                             IMSTagId = tag,
                             ProductId = productEntity.Id
                        });
                    }
                }
                //step2: create product color property
                var propertyEntity = _productPropertyRepo.Insert(new ProductPropertyEntity()
                {
                    IsColor = true,
                    IsSize = false,
                    ProductId = productEntity.Id,
                    PropertyDesc = "颜色",
                    SortOrder = 1,
                    Status = (int)DataStatus.Normal,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authuid
                });
                var propertyvalueEntity = _productPropertyValueRepo.Insert(new ProductPropertyValueEntity()
                {
                    CreateDate = DateTime.Now,
                    PropertyId = propertyEntity.Id,
                    Status = (int)DataStatus.Normal,
                    UpdateDate = DateTime.Now,
                    ValueDesc = request.Color_Str
                });
                //step3: create product size property
                var sizePropertyEntity = _productPropertyRepo.Insert(new ProductPropertyEntity()
                {
                    IsColor = false,
                    IsSize = true,
                    ProductId = productEntity.Id,
                    SortOrder = 1,
                    PropertyDesc = "尺码",
                    Status = (int)DataStatus.Normal,
                    UpdateDate = DateTime.Now,
                    UpdateUser = authuid
                });

                foreach (var size in request.Sizes)
                {   
                    var sizevalueEntity = _productPropertyValueRepo.Insert(new ProductPropertyValueEntity()
                    {
                        CreateDate = DateTime.Now,
                        PropertyId = sizePropertyEntity.Id,
                        UpdateDate = DateTime.Now,
                        ValueDesc = size.Name,
                        Status = (int)DataStatus.Normal
                    });
                    _inventoryRepo.Insert(new InventoryEntity()
                    {
                        Amount = size.Inventory,
                        ProductId = productEntity.Id,
                        PColorId = propertyvalueEntity.Id,
                        PSizeId = sizevalueEntity.Id,
                        UpdateDate = DateTime.Now,
                        UpdateUser = authuid

                    });

                }
             
                //step4: create image
                bool haveValidImage = false;
                ResourceEntity resourceEntity = null;
                int sortOrder = 1;
                foreach (var image in request.Image_Ids.Reverse())
                {
                    resourceEntity = Context.Set<ResourceEntity>().Find(image);
                    if (resourceEntity == null)
                        continue;
                    haveValidImage = true;
                    resourceEntity.SourceType = (int)SourceType.Product;
                    resourceEntity.SourceId = productEntity.Id;
                    resourceEntity.ColorId = propertyvalueEntity.Id;
                    resourceEntity.SortOrder = sortOrder++;
                    _resourceRepo.Update(resourceEntity);
                }
                //step5: if create combo auto
                int? comboId = null;
                if (request.CreateCombo)
                {
                   IMS_ComboEntity comboEntity =  _comboService.CreateComboFromProduct(productEntity,assocateEntity);
                   comboId = comboEntity.Id;
                }
                if (haveValidImage)
                {
                   
                    ts.Complete();

                   

                    return this.RenderSuccess<dynamic>(c => c.Data = new
                    {
                        id = productEntity.Id,
                        image = resourceEntity.Name.Image320Url(),
                        price = productEntity.Price,
                        brand_name = brandEntity.Name,
                        category_name = categoryEntity.Name,
                        combo_id = comboId

                    });
                }
                else
                    return this.RenderError(r=>r.Message="没有传入正确的图片");
               
            }

        }

    
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Update(Yintai.Hangzhou.Contract.DTO.Request.IMSProductCreateRequest request, int authuid)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.Where(v => v.Errors.Count() > 0).First();
                return this.RenderError(r => r.Message = error.Errors.First().ErrorMessage);
            }
            var productEntity = Context.Set<ProductEntity>().Find(request.Id);
            if (productEntity == null)
                return this.RenderError(r => r.Message = "商品不存在");
            if (productEntity.CreatedUser != authuid)
                return this.RenderError(r => r.Message = "无权操作该商品");
            var files = Request.Files;
            bool needUpdateImage = false;
            if (request.Image_Ids.Length >0 )
                needUpdateImage = true;
            var categoryEntity = Context.Set<TagEntity>().Find(request.Category_Id);
            if (categoryEntity == null)
                return this.RenderError(r => r.Message = "分类不存在");
            if (request.Sizes == null || request.Sizes.Length < 1)
                return this.RenderError(r => r.Message = "必须传入一个尺码");
    
            var brandEntity = Context.Set<BrandEntity>().Find(request.Brand_Id);
            if (brandEntity == null)
                return this.RenderError(r => r.Message = "品牌不存在");
            request.UnitPrice = (!request.UnitPrice.HasValue || request.UnitPrice <= 0) ? request.Price : request.UnitPrice;
            if (request.UnitPrice < request.Price)
                return this.RenderError(r => r.Message = "商品原价必须大于等于销售价！");
            if (string.IsNullOrWhiteSpace(request.Name))
                request.Name = string.Format("{0}-{1}-{2}", brandEntity.Name, categoryEntity.Name, request.Sku_Code);
            if (string.IsNullOrEmpty(request.Desc))
                request.Desc = request.Name;
            using (var ts = new TransactionScope())
            {
                //step1: update product 

                productEntity.Brand_Id = request.Brand_Id;
                productEntity.UpdatedDate = DateTime.Now;
                productEntity.UpdatedUser = authuid;
                productEntity.SkuCode = request.Sku_Code;
                productEntity.Name = request.Name;
                productEntity.Description = request.Desc;
                productEntity.Price = request.Price;
                productEntity.UnitPrice = request.UnitPrice;
                productEntity.Tag_Id = request.Category_Id;
                _productRepo.Update(productEntity);

                var productcodeEntity = Context.Set<ProductCode2StoreCodeEntity>().Where(pc => pc.ProductId == request.Id && pc.Status == (int)DataStatus.Normal).First();
                productcodeEntity.StoreProductCode = request.Sales_Code;
                productcodeEntity.UpdateDate = DateTime.Now;
                _productCodeRepo.Update(productcodeEntity);

                foreach (var oldTag in Context.Set<Product2IMSTagEntity>()
                                            .Where(p => p.ProductId == request.Id))
                {
                    _productTagRepo.Delete(oldTag);
                }
                if (request.Tag_Ids != null &&
                    request.Tag_Ids.Length > 0)
                {
                    foreach (var tag in request.Tag_Ids)
                    {
                        _productTagRepo.Insert(new Product2IMSTagEntity()
                        {
                            IMSTagId = tag,
                            ProductId = productEntity.Id
                        });
                    }
                }
                //step2: update product color property
                if (!string.IsNullOrEmpty(request.Color_Str))
                {
                    var colorPropertyEntity = Context.Set<ProductPropertyEntity>().Where(pp => pp.ProductId == productEntity.Id && pp.IsColor == true).First();
                    var colorValue = Context.Set<ProductPropertyValueEntity>().Where(ppv => ppv.PropertyId == colorPropertyEntity.Id && ppv.Status == (int)DataStatus.Normal).First();
                    colorValue.ValueDesc = request.Color_Str;
                    colorValue.UpdateDate = DateTime.Now;
                    _productPropertyValueRepo.Update(colorValue);
                }
                //2.0 update size property
                var sizePropertyEntity = Context.Set<ProductPropertyEntity>().Where(pp => pp.ProductId == productEntity.Id && pp.IsSize == true).First();

                //2.1 soft delete all propertyvalues
                var propertValues = Context.Set<ProductPropertyValueEntity>().Where(ppv => ppv.PropertyId == sizePropertyEntity.Id);
                foreach (var propertyValue in propertValues)
                {
                    propertyValue.Status = (int)DataStatus.Deleted;
                    _productPropertyValueRepo.Update(propertyValue);
                }
                var inventories = Context.Set<InventoryEntity>().Where(i => i.ProductId == productEntity.Id);
                foreach (var inventory in inventories)
                {
                    inventory.Amount = 0;
                    _inventoryRepo.Update(inventory);
                }
                //2.2 update property thereus
                var colorProperty = Context.Set<ProductPropertyEntity>().Where(pp => pp.ProductId == productEntity.Id && pp.IsColor == true)
                                      .Join(Context.Set<ProductPropertyValueEntity>(), o => o.Id, i => i.PropertyId, (o, i) => i).First();

                foreach (var size in request.Sizes)
                {
                    var propertyValue = propertValues.Where(ppv => ppv.ValueDesc == size.Name).FirstOrDefault();
                    if (propertyValue == null)
                    {
                        var sizevalueEntity = _productPropertyValueRepo.Insert(new ProductPropertyValueEntity()
                        {
                            CreateDate = DateTime.Now,
                            PropertyId = sizePropertyEntity.Id,
                            UpdateDate = DateTime.Now,
                            ValueDesc = size.Name,
                            Status = (int)DataStatus.Normal
                        });
                        _inventoryRepo.Insert(new InventoryEntity()
                        {
                            Amount = size.Inventory,
                            ProductId = productEntity.Id,
                            PColorId = colorProperty.Id,
                            PSizeId = sizevalueEntity.Id,
                            UpdateDate = DateTime.Now,
                            UpdateUser = 0

                        });
                    }
                    else
                    {
                        var inventory = Context.Set<InventoryEntity>().Where(i => i.ProductId == productEntity.Id &&
                                                            i.PColorId == colorProperty.Id &&
                                                            i.PSizeId == propertyValue.Id).FirstOrDefault();
                        if (inventory != null)
                        {
                            inventory.Amount = size.Inventory;
                            inventory.UpdateDate = DateTime.Now;
                            _inventoryRepo.Update(inventory);
                        }
                       
                        propertyValue.Status = (int)DataStatus.Normal;
                        _productPropertyValueRepo.Update(propertyValue);
                    }


                }
               
                bool canCommit = true;
                
                //step4: create image
               
                ResourceEntity resourceEntity = null;
                if (needUpdateImage)
                {
                    var currentResources = Context.Set<ResourceEntity>().
                                       Where(r => r.SourceType == (int)SourceType.Product &&
                                                   r.SourceId == (int)productEntity.Id &&
                                                   r.Status == (int)DataStatus.Normal);
                    int colorPropertyId =0;
                    foreach (var oldResource in currentResources)
                    {
                        if (colorPropertyId == 0)
                            colorPropertyId = oldResource.ColorId??0;
                        oldResource.Status = (int)DataStatus.Deleted;
                        oldResource.UpdatedDate = DateTime.Now;
                        _resourceRepo.Update(oldResource);
                    }

                    foreach (var newResource in request.Image_Ids)
                    {
                      
                        var newResourceEntity = Context.Set<ResourceEntity>().Find(newResource);

                        newResourceEntity.SourceType = (int)SourceType.Product;
                        newResourceEntity.SourceId = productEntity.Id;
                        newResourceEntity.ColorId = colorPropertyId;
                        newResourceEntity.Status = (int)DataStatus.Normal;
                        _resourceRepo.Update(newResourceEntity);
                        if (resourceEntity == null)
                            resourceEntity = newResourceEntity;
                    }
                    
                }
                if (resourceEntity == null)
                    canCommit = false;

                //step5: if create combo auto
                int? comboId = null;
                if (request.CreateCombo)
                {
                    var assocateEntity = Context.Set<IMS_AssociateEntity>().Where(ia => ia.UserId == authuid).First();
                    IMS_ComboEntity comboEntity = _comboService.CreateComboFromProduct(productEntity, assocateEntity);
                    comboId = comboEntity.Id;
                }
                if (canCommit)
                {
                    
                    ts.Complete();


                    return this.RenderSuccess<dynamic>(c => c.Data = new
                    {
                        id = productEntity.Id,
                        image = resourceEntity.Name.Image320Url(),
                        price = productEntity.Price,
                        combo_id = comboId

                    });
                }
                else
                {
                    return this.RenderError(r => r.Message = "图片上传出错");
                }

            }
        }

      
        [RestfulRoleAuthorize(UserLevel.DaoGou)]
        public ActionResult Detail(int id, int authuid)
        {
            var productEntity = Context.Set<ProductEntity>().Where(p => p.Id == id && p.Status != (int)DataStatus.Deleted)
                                .Join(Context.Set<TagEntity>(), o => o.Tag_Id, i => i.Id, (o, i) => new { P = o, C = i })
                                .Join(Context.Set<BrandEntity>(), o => o.P.Brand_Id, i => i.Id, (o, i) => new { P = o.P, C = o.C, B = i })
                                .Join(Context.Set<ProductCode2StoreCodeEntity>(), o => o.P.Id, i => i.ProductId, (o, i) => new { P = o.P, C = o.C, B = o.B, PC = i })
                                .GroupJoin(Context.Set<ResourceEntity>().Where(r => r.SourceType == (int)SourceType.Product && r.Status == (int)DataStatus.Normal),
                                            o => o.P.Id,
                                            i => i.SourceId,
                                            (o, i) => new { P = o.P, C = o.C, B = o.B, PC = o.PC, PR = i })
                                .FirstOrDefault();
            if (productEntity == null)
                return this.RenderError(r => r.Message = "商品不存在");
            if (productEntity.P.CreatedUser != authuid)
                return this.RenderError(r => r.Message = "无权操作该商品");
            var colorValue = Context.Set<ProductPropertyEntity>().Where(pp => pp.ProductId == id && pp.IsColor == true && pp.Status == (int)DataStatus.Normal)
                             .Join(Context.Set<ProductPropertyValueEntity>().Where(ppv => ppv.Status == (int)DataStatus.Normal)
                                        , o => o.Id
                                        , i => i.PropertyId
                                        , (o, i) => i)
                             .First();
            var sizeValues = Context.Set<ProductPropertyEntity>().Where(pp => pp.ProductId == id && pp.IsSize == true && pp.Status == (int)DataStatus.Normal)
                             .GroupJoin(Context.Set<ProductPropertyValueEntity>().Where(ppv => ppv.Status == (int)DataStatus.Normal)
                                            .Join(Context.Set<InventoryEntity>().Where(ie => ie.PColorId == colorValue.Id), o => o.Id, i => i.PSizeId, (o, i) => new { PPV=o,I=i})
                                        , o => o.Id
                                        , i => i.PPV.PropertyId
                                        , (o, i) => i)
                             .FirstOrDefault().ToList();

            
            return this.RenderSuccess<Yintai.Hangzhou.Contract.DTO.Response.IMSProductSelfDetailResponse>(r =>
                            r.Data = new IMSProductSelfDetailResponse().FromEntity<IMSProductSelfDetailResponse>(productEntity.P, p =>
                            {
                                p.Brand_Name = productEntity.B.Name;
                                p.Category_Name = productEntity.C.Name;
                                p.SalesCode = productEntity.PC.StoreProductCode;
                                p.ColorStr = colorValue.ValueDesc;
                                p.Sizes = sizeValues.Select(csv => new IMSProductSizeResponse()
                                {
                                    SizeName = csv.PPV.ValueDesc,
                                    SizeValueId = csv.PPV.Id,
                                    Inventory = csv.I.Amount
                                });
                                p.IMS_Tags = Context.Set<Product2IMSTagEntity>()
                                             .Where(pi => pi.ProductId == id)
                                            .Join(Context.Set<IMS_TagEntity>().Where(it => it.Status == (int)DataStatus.Normal), o => o.IMSTagId, i => i.Id, (o, i) => i)
                                            .Select(it => it.Id);
                                if (productEntity.PR != null)
                                {
                                    p.Images = productEntity.PR.Select(pr => new IMSSelfImageResponse() { 
                                         Id = pr.Id,
                                         Name = pr.Name
                                    }).ToArray();
                                }
                            }));
        }


    }
}
