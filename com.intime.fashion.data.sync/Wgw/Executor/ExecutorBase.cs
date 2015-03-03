using Common.Logging;
using System;
using System.Collections.Generic;

namespace com.intime.fashion.data.sync.Wgw.Executor
{
    public abstract class ExecutorBase

    {
        /// <summary>
        /// Bench time
        /// </summary>
        protected DateTime BenchTime { get; set; }

        protected SyncClient Client { get; set; }

        protected abstract int SucceedCount { get; }

        protected abstract int FailedCount { get; }

        protected virtual int TotalCount
        {
            get { return SucceedCount + FailedCount; }
        }

        protected ILog Logger { get; set; }
        protected ExecutorBase(DateTime benchTime, ILog logger):this(new SyncClient(), benchTime,logger )
        {
         
        }

        protected ExecutorBase(SyncClient client, DateTime benchTime, ILog logger)
        {
            this.BenchTime = benchTime;
            this.Logger = logger;
            this.MessageList = new List<string>();
            this.Client = client;
        }

        /// <summary>
        /// Message for execute infos
        /// </summary>
        public List<string> MessageList { get; set; }

        /// <summary>
        /// execute a sync task
        /// </summary>
        /// <param name="extraParameter"></param>
        protected abstract void ExecuteCore(dynamic extraParameter = null);

        public ExecuteResult Execute(dynamic extraParameter = null)
        {
            try
            {
                ExecuteCore(extraParameter);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                var result = new ExecuteResult()
                {
                    Status = ExecuteStatus.ExceptionThrow,
                };
                result.MessageList.Add(ex.Message);
                return result;
            }
            return new ExecuteResult()
            {
                TotalCount = TotalCount,
                FailedCount = FailedCount,
                ResultFlag = GetResult(),
                Status = ExecuteStatus.Succeed
            };
        }

        protected ResultFlag GetResult()
        {
            if (TotalCount == 0)
            {
                return ResultFlag.None;
            }
            if (SucceedCount == TotalCount)
            {
                return ResultFlag.Succeed;
            }
            if (FailedCount == TotalCount)
            {
                return ResultFlag.Failed;
            }
            return ResultFlag.PartialSucceed;
        }

    }

    public class ExecuteResult
    {
        public ExecuteResult()
        {
            this.MessageList = new List<string>();
        }

        public ExecuteStatus Status { get; set; }

        /// <summary>
        /// 成功同步数量
        /// </summary>
        public int SucceedCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 执行信息
        /// </summary>
        public List<string> MessageList { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public ResultFlag ResultFlag { get; set; }
    }

    public enum ExecuteStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 200,

        /// <summary>
        /// 抛出异常
        /// </summary>
        ExceptionThrow = 500,
    }

    public enum ResultFlag
    {
        /// <summary>
        /// 无更新
        /// </summary>
        None = -1,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 0,

        /// <summary>
        /// 部分成功
        /// </summary>

        PartialSucceed = 1,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 2,
    }
}
