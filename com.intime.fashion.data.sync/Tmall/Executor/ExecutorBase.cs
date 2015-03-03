using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Logging;
using System;

namespace com.intime.fashion.data.sync.Tmall.Executor
{
    
    public abstract class ExecutorBase
    {
        protected ILog Logger = LogManager.GetCurrentClassLogger();

        protected DateTime _benchDateTime;
        protected int _pageSize;

        protected ExecutorBase(DateTime benchTime, int pageSize)
        {
            _benchDateTime = benchTime;
            _pageSize = pageSize;
        }

        public abstract void Execute();
    }
}
