using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Model;

namespace com.intime.fashion.service
{
    public abstract class BusinessServiceBase:IDbAware,IDebugAare
    {
        protected ILog _log ;
        protected DbContext _db ;

        public BusinessServiceBase()
            : this(ServiceLocator.Current.Resolve<ILog>(),
            ServiceLocator.Current.Resolve<DbContext>())
        { }
        public BusinessServiceBase(ILog log,
            DbContext db)
        {
            _db = db;
            _log = log;
        }

        protected BusinessResult<T> Error<T>(string message) 
            where T:class
        {
            return new BusinessResult<T>()
            {
                IsSuccess = false,
                Error = new ErrorResult(message) 
            };
        }
        protected BusinessResult<T> Success<T>(T result)
            where T:class
        {
            return new BusinessResult<T>() { 
                IsSuccess = true,
                Result = result
            };
        }
        public ILog GetLog()
        {
            return _log;
        }

        public DbContext GetContext()
        {
            return _db;
        }
    }
}
