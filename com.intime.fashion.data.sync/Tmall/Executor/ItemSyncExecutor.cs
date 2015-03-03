using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using com.intime.fashion.data.tmall.Models;
using com.intime.o2o.data.exchange.IT;
using com.intime.o2o.data.exchange.Ims.Request;
using Common.Logging;
using Newtonsoft.Json;
using Top.Api.Domain;

namespace com.intime.fashion.data.sync.Tmall.Executor
{
    public class ItemSyncExecutor : ExecutorBase
    {
        private IApiClient _client;
        public ItemSyncExecutor(DateTime benchTime, int pageSize, IApiClient client)
            : base(benchTime, pageSize)
        {
            _client = client;
        }

        public override void Execute()
        {
            Logger = LogManager.GetLogger(this.GetType());
            int totalCount = 0;
            int cursor = 0;
            Decimal lastCursor = 0;
            DoQuery(null, items => totalCount = items.Count());

            Logger.InfoFormat("获取到({0})商品", totalCount);

            while (cursor < totalCount)
            {
                List<JDP_TB_ITEM> oneTimeList = null;
                DoQuery(null,
                    orders =>
                        oneTimeList = orders.Where(o => o.num_iid > lastCursor).OrderBy(o => o.num_iid).Take(_pageSize).ToList());

                foreach (var item in oneTimeList)
                {
                    try
                    {
                        SyncOne(item);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
                cursor += _pageSize;
                lastCursor = oneTimeList.Max(o => o.num_iid);
            }
        }

        private void SyncOne(JDP_TB_ITEM item)
        {
            var req = new ItemMapRequest();

            var response = JsonConvert.DeserializeObject<dynamic>(item.jdp_response);
            var itemInfo = response.item_get_response.item;
            var skus = new List<dynamic>();
            if (itemInfo.skus == null)
            {
                skus.Add(new
                {
                    itemInfo.outer_id,
                    itemInfo.price,
                    quantity = itemInfo.num,
                    sku_id = 0,
                    properties = itemInfo.props
                });
            }
            else
            {
                foreach (var sku in itemInfo.skus.sku)
                {
                    skus.Add(new
                    {
                        sku.outer_id,
                        sku.price,
                        sku.quantity,
                        sku.sku_id,
                        sku.properties
                    });
                }
            }

            dynamic requestBody = new
            {
                itemInfo.num_iid,
                itemInfo.outer_id,
                skus
            };

            req.Data = new List<dynamic>() { requestBody };
            var rsp = _client.Post(req);

            Logger.Info(rsp);
        }

        protected void DoQuery(Expression<Func<JDP_TB_ITEM, bool>> whereCondition, Action<IQueryable<JDP_TB_ITEM>> callback)
        {
            using (var context = DbContextHelper.GetJushitaContext())
            {
                var linq =
                    context.JDP_TB_ITEM.Where(x => x.created >= _benchDateTime && x.jdp_delete != 1);
                if (whereCondition != null)
                    linq = linq.Where(whereCondition);
                if (callback != null)
                    callback(linq);
            }
        }
    }
}
