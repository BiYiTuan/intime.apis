using Yintai.Architecture.Framework.ServiceLocation.Adapter;

namespace Yintai.Architecture.Framework.ServiceLocation
{
    /// <summary>
    /// 针对不同的DI容器提供外观类
    /// </summary>
    public sealed class ServiceLocator
    {
        private static IServiceLocator _serviceLocator;

        private static readonly object _lockObj = new object();

        /// <summary>
        /// 当前容器
        /// </summary>
        public static IServiceLocator Current
        {
            get
            {
                if (_serviceLocator == null)
                {
                    lock (_lockObj)
                    {
                        if (_serviceLocator == null)
                        {
                            _serviceLocator = new UnityServiceLocator();
                        }
                    }
                }

                return _serviceLocator;
            }
        }

        /// <summary>
        /// 设置当前使用的容器
        /// </summary>
        /// <param name="newLocator"></param>
        public static void SetLocatorProvider(IServiceLocator newLocator)
        {
            _serviceLocator = newLocator;
        }
    }
}
