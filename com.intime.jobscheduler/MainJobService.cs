using com.intime.fashion.common.message;
using Common.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;


namespace com.intime.jobscheduler
{
    public partial class MainJobService : ServiceBase
    {
        private IScheduler _sche;
        private ILog _log;
        public MainJobService()
        {
            InitializeComponent();

            UnityBootStrapper.Init();

            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        }
        internal void ConsoleDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            _log = _log??LogManager.GetLogger(typeof(MainJobService));
            ISchedulerFactory scheduler = new StdSchedulerFactory();
            _sche =  scheduler.GetScheduler();

            _log.Info("starting scheduler...");
            _sche.Start();
            _log.Info("started scheduler...");

        }

        protected override void OnStop()
        {
           
            if (_sche != null &&
                !_sche.IsShutdown)
            {
                _sche.Shutdown();

                _log = _log ?? LogManager.GetLogger(typeof(MainJobService));
                _log.Info("shut down scheduler...");
            }
        }
    }
}
