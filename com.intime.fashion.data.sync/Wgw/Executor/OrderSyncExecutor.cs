using System.Linq.Expressions;
using com.intime.fashion.data.sync.Wgw.Request.Builder;
using com.intime.fashion.data.sync.Wgw.Request.Order;
using com.intime.fashion.data.sync.Wgw.Response.Processor;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.data.sync.Wgw.Executor
{
    public class OrderSyncExecutor:ExecutorBase
    {
        private int _succeedCount = 0;
        private int _failedCount = 0;
        private readonly string _beginTime;
        private readonly string _endTime;

        public OrderSyncExecutor(DateTime benchTime, ILog logger) : this(new SyncClient(), benchTime, logger)
        {
        }

        public OrderSyncExecutor(SyncClient client, DateTime benchTime, ILog logger) : base(client, benchTime, logger)
        {
            _beginTime = BeginTime(benchTime);
            _endTime = EndTime();
        }

        protected override int SucceedCount
        {
            get { return _succeedCount; }
        }

        protected override int FailedCount
        {
            get { return _failedCount; }
        }

        protected override void ExecuteCore(dynamic extraParameter = null)
        {
            int pageSize = extraParameter??20;
            var sizeStr = pageSize.ToString();
            //this.SyncCreatedOrders(sizeStr);
            this.SyncPaidOrders(sizeStr);
            //this.SyncCancelledOrder(sizeStr);
            this.SyncShippedOrder(pageSize);
        }

        private void DoQuery(Expression<Func<Map4OrderEntity, bool>> whereCondition, Action<IQueryable<Map4OrderEntity>> callback)
        {
            using (var context = DbContextHelper.GetDbContext())
            {
                const int shipping = (int) OrderOpera.Shipping;
                const int fromCustomer = (int) OrderOpera.FromCustomer;
                var linq =
                    context.OrderLogs.Where(l => l.Type == shipping && l.CreateDate >= BenchTime)
                        .Join(context.Map4Order.Where(m => m.SyncStatus == fromCustomer), l => l.OrderNo,
                            m => m.OrderNo, (l, m) => m);
                if (whereCondition != null)
                    linq = linq.Where(whereCondition);
                if (callback != null)
                    callback(linq);
            }
        }

        /// <summary>
        /// 同步已发货状态至微购物
        /// </summary>
        /// <param name="pageSize"></param>
        private void SyncShippedOrder(int pageSize)
        {
            int totalCount = 0;
            int cursor = 0;
            int lastCursor = 0;
            DoQuery(null,orders=>totalCount = orders.Count());

            while (cursor < totalCount)
            {
                List<Map4OrderEntity> oneTimeList = null;
                DoQuery(null,orders=>oneTimeList = orders.Where(o=>o.Id > lastCursor).OrderBy(o=>o.Id).Take(pageSize).ToList());
                foreach (var map4Order in oneTimeList)
                {
                    try
                    {
                        var builder = RequestParamsBuilderFactory.CreateBuilder(new MarkShippingRequest());
                        var rsp = Client.Execute<dynamic>(builder.BuildParameters(map4Order.ChannelOrderCode));
                        if (rsp.errorCode != 0)
                        {
                            _failedCount += 1;
                            Logger.Error(string.Format("Failed to mark order to shipping status ==>Error Message:{0}", rsp.errorMessage));
                        }
                        else
                        {
                            using (var db = DbContextHelper.GetDbContext())
                            {
                                var mapping = db.Map4Order.FirstOrDefault(m => m.Id == map4Order.Id);
                                if(mapping == null) continue;
                                mapping.SyncStatus = (int)OrderOpera.Shipping;
                                mapping.UpdateDate = DateTime.Now;
                                db.SaveChanges();
                                _succeedCount += 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message);
                        _failedCount += 1;
                    }
                }
                cursor += pageSize;
                lastCursor = oneTimeList.Max(o => o.Id);
            }
        }

        /// <summary>
        /// 微购物上取消的订单状态同步
        /// </summary>
        /// <param name="pageSize"></param>
        private void SyncCancelledOrder(string pageSize)
        {
            var dict = BuildQueryDict(pageSize);
            var request = new QueryOrderListRequest(dict);
            request.Put("dealState", OrderStatusConst.STATE_WG_CANCLE);
            SyncCancelledOrdersFromWgw(request);
        }

        private Dictionary<string, string> BuildQueryDict(string pageSize)
        {
            return new Dictionary<string, string>
            {
                {"pageSize", pageSize},
                {"pageindex", "1"},
                {"timeBegin",_beginTime},
                {"timeEnd", _endTime},
                {"historyDeal", "0"},
                {"listItem", "0"},
                {"orderDesc", "0"},
            };
        }


        private void SyncCreatedOrders(string sizeStr)
        {
            var dict = BuildQueryDict(sizeStr);
            var request = new QueryOrderListRequest(dict);
            request.Put("dealState", OrderStatusConst.STATE_WG_WAIT_PAY);
            request.Put("timeType", "CREATE");
            SyncOrdersFromWgw(request);
        }

        private void SyncPaidOrders(string pageSize)
        {
            var dict = BuildQueryDict(pageSize);
            var request = new QueryOrderListRequest(dict);
            request.Put("dealState", OrderStatusConst.STATE_WG_PAY_OK);
            request.Put("timeType", "PAY");
            SyncOrdersFromWgw(request);
        }

        /// <summary>
        /// 微购物上取消的订单状态同步下来
        /// </summary>
        /// <param name="request"></param>
        private void SyncCancelledOrdersFromWgw(QueryOrderListRequest request)
        {
            var result = Client.Execute<dynamic>(request);

            if (result.errorCode == 0)
            {
                foreach (var order in result.dealList.dealListVo)
                {
                    var processor = OrderPrcossorFactory.CreateProcessor(request.RequestParams["dealState"]);
                    if (!processor.Process(order, null))
                    {
                        _failedCount += 1;
                        Logger.Error(processor.ErrorMessage);
                    }
                    else
                    {
                        _succeedCount += 1;
                    }
                }
                var maxPage = result.dealList.maxPageNo;
                var currentPage = result.dealList.currPage;
                while (currentPage < maxPage)
                {
                    request.Put("pageindex", currentPage += 1);
                    request.Remove("sign");
                    SyncCancelledOrdersFromWgw(request);
                }
            }
            Logger.Error(result.errorMessage);
            

        }

        /// <summary>
        /// 同步订单
        /// </summary>
        /// <param name="request"></param>
        private void SyncOrdersFromWgw(ISyncRequest request)
        {
            var result = Client.Execute<dynamic>(request);

            if (result.errorCode == 0)
            {
                foreach (var order in result.dealList.dealListVo)
                {
                    var detailRequest = new QueryOrderDetailRequest(order.dealCode.ToString());
                    var detail = Client.Execute<dynamic>(detailRequest);
                    var processor = OrderPrcossorFactory.CreateProcessor(request.RequestParams["dealState"]);
                    if (!processor.Process(order, detail))
                    {
                        _failedCount += 1;
                        Logger.Error(string.Format("Failed to load order detail {0},Error Message{1}", order.dealCode, processor.ErrorMessage));
                    }
                    else
                    {
                        _succeedCount += 1;
                    }
                }
                var maxPage = result.dealList.maxPageNo;
                var currentPage = result.dealList.currPage;
                while (currentPage < maxPage)
                {
                    request.Put("pageindex", currentPage += 1);
                    request.Remove("sign");
                    SyncOrdersFromWgw(request);
                }
            }
            else
            {
                Logger.Error(string.Format("Failed to load order list:{0}", result.errorMessage));
            }
        }

        private string BeginTime(DateTime dateTime)
        {
            return TimeStampStr(dateTime);
        }

        private string EndTime()
        {
            return TimeStampStr(DateTime.Now);
        }

        private string TimeStampStr(DateTime dateTime)
        {
            TimeSpan t = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1);
            return ((Int64)t.TotalMilliseconds).ToString();
        }
    }
}
