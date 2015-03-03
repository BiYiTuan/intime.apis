using Yintai.Hangzhou.WebSupport.Binder;

namespace Yintai.Hangzhou.WebSupport.Ioc
{
    internal class UnfinishedIocRegister : BaseIocRegister
    {
        #region Overrides of BaseIocRegister

        public override void Register()
        {
            Current.RegisterSingleton<PagerRequestBinder, PagerRequestBinder>();
        }

        #endregion
    }
}
