using com.intime.fashion.service.contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Common.Logger;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace com.intime.fashion.service
{
    public partial class ComboService:BusinessServiceBase, IComboService
    {
        private IEFRepository<IMS_AssociateItemsEntity> _associateItemRepo;
        private IEFRepository<IMS_ComboEntity> _comboRepo;
        private IEFRepository<IMS_Combo2ProductEntity> _combo2productRepo;
        private IResourceRepository _resourceRepo;

        public ComboService(IEFRepository<IMS_AssociateItemsEntity> associateItemRepo,
            IEFRepository<IMS_ComboEntity> comboRepo,
            IEFRepository<IMS_Combo2ProductEntity> combo2productRepo,
            IResourceRepository resourceRepo):base()
        {
            _associateItemRepo = associateItemRepo;
            _comboRepo = comboRepo;
            _combo2productRepo = combo2productRepo;
            _resourceRepo = resourceRepo;
        }
        public void RefreshPrice(IMS_ComboEntity combo)
        {
            if (combo == null)
                return;
            var products = _db.Set<IMS_Combo2ProductEntity>()
                .Where(icp => icp.ComboId == combo.Id)
                .Join(_db.Set<ProductEntity>(), o => o.ProductId, i => i.Id, (o, i) => i);
            var totalPrice = products.Sum(p => (decimal?)p.Price);
            combo.Price = totalPrice??0m;
            combo.UpdateDate = DateTime.Now;
            _comboRepo.Update(combo);
        }
        public void RefreshPrice(int productId)
        {
            foreach (var combo in _db.Set<IMS_Combo2ProductEntity>().Where(icp => icp.ProductId == productId)
                                    .Join(_db.Set<IMS_ComboEntity>(),o=>o.ComboId,i=>i.Id,(o,i)=>i)
                                    .ToList())
            {
                RefreshPrice(combo);
            }
        }
    }
}
