using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.messagelistener
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                var job = new MainJobService();
                job.ConsoleDebug();
            }
            else
            {

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MainJobService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
