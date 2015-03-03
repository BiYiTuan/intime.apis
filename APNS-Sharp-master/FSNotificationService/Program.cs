using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            new NFService().Run();
#else
           ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new NFService()
            };
            ServiceBase.Run(ServicesToRun);
#endif


        }
    }
}
