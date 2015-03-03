using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Architecture.Framework.ServiceLocation
{
    /// <summary>
    /// 提供Service Locator事件参数数据
    /// </summary>
    public class ServiceEventArgs : EventArgs
    {
        private string _key;

        public ServiceEventArgs(string key)
        {
            this._key = key;
        }

        /// <summary>
        /// 向容器中注册或者获取服务时的键值
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
    }
}
