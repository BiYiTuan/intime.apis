using System.Web.Mvc;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Web.Mvc.Ioc;

namespace Yintai.Hangzhou.WebSupport.Ioc
{
    internal class ImportantSupportIocRegister : BaseIocRegister
    {
        #region Overrides of BaseIocRegister

        public override void Register()
        {
            Current.Register<IControllerActivator, CustomControllerActivator>();
            Current.Register<IDependencyResolver, IocDependencyResolver>();
            Current.RegisterSingleton<ILog, Log4NetLog>();
        }

        #endregion
    }
}