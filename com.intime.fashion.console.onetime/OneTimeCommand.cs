using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;

namespace com.intime.fashion.console.onetime
{
    partial class OneTimeCommand
    {
        private static ILog _log = ServiceLocator.Current.Resolve<ILog>(); 
    }
}
