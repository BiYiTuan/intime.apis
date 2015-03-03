using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.console.onetime
{
    /// <summary>
    /// This class main purpose is to inject dbcontext class to fix the caching bug
    /// 
    /// </summary>
    public class ScopedLifetimeDbContextManager:IDisposable
    {
        DbContext _db = null;
        public ScopedLifetimeDbContextManager()
        {
            _db = ServiceLocator.Current.Resolve<DbContext>();
        }

        private void Refresh()
        {

            ObjectContext oc = ((IObjectContextAdapter)_db).ObjectContext; ;

            const BindingFlags flags = BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance;

            var c = oc.GetType().GetField("_cache", flags);


            c.SetValue(oc, null);
        }

        public void Dispose()
        {
            Refresh();
        }
    }
}
