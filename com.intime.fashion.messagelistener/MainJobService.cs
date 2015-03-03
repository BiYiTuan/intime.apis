using com.intime.fashion.common.message;
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
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Framework.ServiceLocation;


namespace com.intime.fashion.messagelistener
{
    public partial class MainJobService : ServiceBase
    {
        private MessageListener _messageListener;
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
            _log = ServiceLocator.Current.Resolve<ILog>();
            //register message listen
            _messageListener = MessageListener.Current;
            _messageListener.Start();
            _log.Info("started listener...");

        }

        protected override void OnStop()
        {

            if (_messageListener.IsStart())
            {
                _messageListener.Stop();
            
                _log.Info("shut down listener...");
            }
        }
    }
}
