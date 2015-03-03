using System;

namespace com.intime.jobscheduler.Job.Wgw
{
    public class WgwSyncException:Exception
    {
        public WgwSyncException() { }

        public WgwSyncException(string msg) : base(msg)
        {
        }
    }

    public class StockInsufficientException : WgwSyncException
    {
        public StockInsufficientException(string msg) : base(msg)
        {
        }

        public StockInsufficientException(string dealCode, string dealStatus)
        {
            this.DealCode = dealCode;
            this.DealStatus = dealStatus;
        }

        public string DealCode { get; set; }

        public string DealStatus { get; set; }

    }
}
