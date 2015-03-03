using System;
using System.Collections.Generic;
using com.intime.fashion.data.tmall.Models;

namespace com.intime.fashion.data.sync.Tmall.Executor
{
    public class SyncResult
    {
        public static SyncResult ExceptionResult(Exception ex, JDP_TB_TRADE trade)
        {
            return new SyncResult
            {
                FailedReason = string.Format("Order tid is({0})", trade.tid),
                Exception = ex,
                Succeed = false
            };
        }

        public static SyncResult FailedResult(string message)
        {
            return new SyncResult()
            {
                FailedReason = message,
                Exception = null,
                Succeed = false
            };
        }

        public static SyncResult SucceedResult(string imsOrderNo, long tmallOrderId, dynamic tmallOrder)
        {
            return new SyncResult
            {
                Succeed = true,
                TargetOrderNo = imsOrderNo,
                TmalOrderId = tmallOrderId,
                TmallOrder = tmallOrder
            };
        }

        public long TmalOrderId { get; set; }

        public string TargetOrderNo { get; set; }

        public bool Succeed { get; set; }

        public string FailedReason { get; set; }

        public Exception Exception { get; set; }

        public dynamic TmallOrder { get; private set; }
    }
}