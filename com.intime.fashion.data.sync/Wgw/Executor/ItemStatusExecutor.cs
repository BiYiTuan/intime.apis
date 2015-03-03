using com.intime.fashion.data.sync.Wgw.Request.Item;
using com.intime.jobscheduler.Job.Wgw;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace com.intime.fashion.data.sync.Wgw.Executor
{
    public class ItemStatusExecutor : ExecutorBase
    {
        private int _succeedCount;
        private int _failedCount;
        private int _totalCount;

        private readonly ISyncRequest _request;

        public ItemStatusExecutor(DateTime benchTime, ILog logger, UpOrDownItemRequest request)
            : base(benchTime, logger)
        {
            this._request = request;
        }
        protected override int SucceedCount
        {
            get { return _succeedCount; }
        }

        protected override int FailedCount
        {
            get { return _failedCount; }
        }

        protected override int TotalCount
        {
            get { return _totalCount; }
        }

        protected override void ExecuteCore(dynamic extraParameter = null)
        {
            var itemIds = extraParameter as IEnumerable<string>;
            if (itemIds == null)
            {
                throw new WgwSyncException(string.Format("extraParameter must be IEnumerable<string>"));
            }

            int cursor = 0;
            const int pageSize = 200;
            _totalCount = itemIds.Count();

            while (cursor < _totalCount)
            {
                List<string> itemIdList = itemIds.Skip(cursor).Take(pageSize).ToList();
                _request.Put(ParamName.Param_ItemList, string.Join("|", itemIdList));
                _request.Remove("sign");
                var rsp = Client.Execute<dynamic>(_request);
                if (rsp.errorCode == 0)
                {
                    _succeedCount += itemIdList.Count;
                }
                else if (rsp.errorCode == 40005) //部分失败
                {
                    string failList = rsp.failList;
                    var count = failList.Split(':').Count();
                    _failedCount += count;
                    _succeedCount += itemIdList.Count - count;
                    Logger.Error(failList);
                }
                else
                {
                    _failedCount += itemIdList.Count;
                    Logger.Error(rsp.errorMessage);
                }
                cursor += pageSize;
            }
        }
    }
}
