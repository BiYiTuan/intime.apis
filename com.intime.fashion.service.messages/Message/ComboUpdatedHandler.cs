using System.Security.Cryptography;
using com.intime.fashion.common.message;
using System;
using System.Data.Entity;
using System.Linq;
using com.intime.o2o.data.exchange.Tmall.Core;
using com.intime.o2o.data.exchange.Tmall.Core.Support;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.service.messages.Message
{
    public class ComboUpdatedHandler : MessageHandler
    {
        private DbContext _db;
        private ITopClient _topClient;
        private string _sessionKey;
        private readonly ITopClientFactory _topClientFactory = new DefaultTopClientFactory();
        public ComboUpdatedHandler()
        {
            _db = ServiceLocator.Current.Resolve<DbContext>();
            _topClient = _topClientFactory.Get(InventoryUpdatedHandler.TOP_CONSUMER_KEY);
            _sessionKey = _topClientFactory.GetSessionKey(InventoryUpdatedHandler.TOP_CONSUMER_KEY);
        }

        public override int SourceType
        {
            get { return (int)MessageSourceType.Combo; }
        }

        public override int ActionType
        {
            get
            {
                return (int)(MessageAction.UpdateEntity | MessageAction.DeleteEntity);
            }
        }

        public override bool Work(BaseMessage message)
        {
            int comboId = message.EntityId;

            var combo = _db.Set<IMS_ComboEntity>().FirstOrDefault(x => x.Id == comboId);
            if (combo == null)
            {
                return false;
            }
            var mergedProducts =
                _db.Set<IMS_Combo2ProductEntity>()
                    .Where(c => c.ComboId == message.EntityId)
                    .Join(_db.Set<ProductPoolEntity>(), c => c.ProductId, p => p.ProductId, (c, p) => p);


            var mappedInventories =
                mergedProducts.Join(_db.Set<ProductPoolEntity>(), p1 => p1.MergedProductCode, p2 => p2.MergedProductCode,
                    (p1, p2) => p2)
                    .Join(_db.Set<InventoryEntity>(), p => p.ProductId, m => m.ProductId, (p, i) => i)
                    .Join(_db.Set<Map4InventoryEntity>().Where(m => m.Channel == "tmall" && m.status == (int)DataStatus.Normal), i => i.Id, m => m.InventoryId,
                        (i, m) => new { Inventory = i, Map = m });

            bool allSucceed = true;
            bool isDown = combo.Status == (int)DataStatus.Default || combo.Status == (int)DataStatus.Deleted;
            foreach (var entity in mappedInventories)
            {
                string outerId = entity.Inventory.Id.ToString();
                long num_iid = Convert.ToInt64(entity.Map.itemId);
                long skuId = entity.Map.skuId;
                int amount = isDown ? 0 : entity.Inventory.Amount;

                allSucceed = SyncInventory(outerId, num_iid, skuId, amount);
            }

            return allSucceed;
        }

        private bool SyncInventory(string inventoryId, long channelItemId, long channelSkuId, int amount)
        {
            var req = new ItemQuantityUpdateRequest
            {
                OuterId = inventoryId,
                Quantity = amount,
                NumIid = channelItemId,
                SkuId = channelSkuId,
            };
            return (_topClient.Execute(req, _sessionKey).IsError);
        }
    }
}
