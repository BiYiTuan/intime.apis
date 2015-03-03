using System.Web;

namespace Yintai.Hangzhou.WebSupport.Ioc
{
    internal class RequestLifetimeHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, e) => PerRequestUnityServiceLocator.DisposeOfChildContainer();
        }

        public void Dispose()
        {
            // nothing to do here
        }        
    }
}