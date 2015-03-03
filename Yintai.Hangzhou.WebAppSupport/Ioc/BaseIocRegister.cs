using Yintai.Architecture.Framework.ServiceLocation;

namespace Yintai.Hangzhou.WebSupport.Ioc
{
    internal abstract class BaseIocRegister : IIocRegister
    {
        protected static readonly IServiceLocator Current;

        static BaseIocRegister()
        {
            ServiceLocator.SetLocatorProvider(new PerRequestUnityServiceLocator());
            Current = ServiceLocator.Current;
        }

        #region Implementation of IIocRegister

        public abstract void Register();

        public void Add(IIocRegister register)
        {

        }

        #endregion
    }
}