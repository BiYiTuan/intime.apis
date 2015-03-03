using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.fashion.service.search
{
    class ESInventoryService : ESServiceSingle<ESStock>
    {
        protected override ESStock entity2Model(int id)
        {
            var db = Context;
            //var inventoryEntity = db.Set<InventoryEntity>().Where(i => i.Id == id).Join(db.Set<ProductPropertyValueEntity>(),i=>i.PColorId,ppvColor=>ppv.Id,(i,ppv)=>new {i,ppv})
            //                    .Join(db.Set<ProductEntity>(), o => o.ProductId, i => i.Id, (o, i) => new { I = o, P = i }).FirstOrDefault();
            //if (inventoryEntity == null)
            //    return null;
            //return new ESStock()
            //{
            //    ProductId = inventoryEntity.I.ProductId,
            //    Amount = inventoryEntity.I.Amount,
            //    ColorValueId = inventoryEntity.I.PColorId,
            //    SizeValueId = inventoryEntity.I.PSizeId,
            //    Id = inventoryEntity.I.Id,
            //    LabelPrice = inventoryEntity.P.UnitPrice.HasValue ? inventoryEntity.P.UnitPrice.Value : 999999,
            //    Price = inventoryEntity.P.Price,
            //    UpdateDate = DateTime.Now,
            //};

            var stocks = from inventory in db.Set<InventoryEntity>()
                         from product in db.Set<ProductEntity>()
                         from cppv in db.Set<ProductPropertyValueEntity>()
                         from sppv in db.Set<ProductPropertyValueEntity>()
                         where
                             inventory.Id == id && inventory.ProductId == product.Id && inventory.PColorId == cppv.Id &&
                             inventory.PSizeId == sppv.Id && cppv.Status == (int)DataStatus.Normal && sppv.Status == (int)DataStatus.Normal
                         select new ESStock()
                         {
                             ProductId = inventory.ProductId,
                             Amount = inventory.Amount,
                             ColorValueId = cppv.Id,
                             ColorDesc = cppv.ValueDesc,
                             SizeValueId = sppv.Id,
                             SizeDesc = sppv.ValueDesc,
                             Id = inventory.Id,
                             LabelPrice = product.UnitPrice.HasValue ? product.UnitPrice.Value : 999999,
                             Price = product.Price,
                             UpdateDate = DateTime.Now
                         };
            return stocks.FirstOrDefault();
        }

    }
}
