using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Apns.FSNotificationService
{
    public partial class NFService : ServiceBase
    {
        private FSNotificationManager _nsManager = FSNotificationManager.Instance;
        public NFService()
        {
            InitializeComponent();
        }
#if DEBUG
        public void Run()
        {
            OnStart(null);
        }
#endif
        protected override void OnStart(string[] args)
        {
            _nsManager.Start();
        }
        protected override void OnPause()
        {
            base.OnPause();
            _nsManager.Stop();
        }
        protected override void OnContinue()
        {
            base.OnContinue();
            _nsManager.Resume();
        }
        protected override void OnStop()
        {
            _nsManager.Close();
        }
    }
}
