using System.Data.Entity;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service
{
    public abstract class BaseService
    {
        public static ILog Logger;

        public MappingManagerV2 MappingManager { get; set; }

	//BaseService
        protected BaseService()
        {
            MappingManager = ServiceLocator.Current.Resolve<MappingManagerV2>();
        }

        protected DbContext Context
        {
            get
            {
                return ServiceLocator.Current.Resolve<DbContext>();
            }
        }
        static BaseService()
        {
            Logger = LoggerManager.Current();
        }

        
    }
}