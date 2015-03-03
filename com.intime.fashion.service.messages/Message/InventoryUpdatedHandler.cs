using com.intime.fashion.common.message;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using com.intime.o2o.data.exchange.Tmall.Core;
using com.intime.o2o.data.exchange.Tmall.Core.Support;
using Top.Api;
using Top.Api.Request;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;


namespace com.intime.fashion.service.messages.Message
{
    public class InventoryUpdatedHandler : MessageHandler
    {
        private DbContext _context;
        private ITopClient _topClient;
        private string _sessionKey;
        internal static string TOP_CONSUMER_KEY = ConfigurationManager.AppSettings["TOP_CONSUMER_KEY"] ?? "intime";
        private readonly ITopClientFactory _topClientFactory = new DefaultTopClientFactory();
        public InventoryUpdatedHandler()
        {
            _context = ServiceLocator.Current.Resolve<DbContext>();
            _topClient = _topClientFactory.Get(TOP_CONSUMER_KEY);
            _sessionKey = _topClientFactory.GetSessionKey(TOP_CONSUMER_KEY);
        }
        public override int SourceType
        {
            get { return (int)MessageSourceType.Inventory; }
        }

        public override int ActionType
        {
            get { return (int)(MessageAction.CreateEntity | MessageAction.UpdateEntity); }
        }

        public override bool Work(BaseMessage message)
        {
            return SyncInventory(message.EntityId, _context, _topClient);
        }

        /// <summary>
        /// 增加OrderByDescending，防止天猫上删除的sku在map表中重复导致的库存更新问题
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="db"></param>
        /// <param name="topClient"></param>
        /// <returns></returns>
        private bool SyncInventory(int inventoryId, DbContext db, ITopClient topClient)
        {
            var stockInfo =
                 db.Set<InventoryEntity>()
                     .Where(x => x.Id == inventoryId)
                     .Join(
                         db.Set<Map4InventoryEntity>()
                             .Where(m => m.Channel == "tmall" && m.status == (int)DataStatus.Normal && m.InventoryId == inventoryId), i => i.Id,
                         m => m.InventoryId, (i, m) => new { Inventory = i, Map4Inventory = m }).OrderByDescending(x => x.Map4Inventory.Id)
                     .FirstOrDefault();
            if (stockInfo == null)
            {
                return false;
            }
            var req = new ItemQuantityUpdateRequest
            {
                OuterId = stockInfo.Inventory.Id.ToString(),
                Quantity = stockInfo.Inventory.Amount,
                NumIid = Convert.ToInt64(stockInfo.Map4Inventory.itemId),
                SkuId = stockInfo.Map4Inventory.skuId
            };

            return topClient.Execute(req, _sessionKey).IsError;
        }
    }
}
