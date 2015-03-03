using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceProcess;
using Yintai.Architecture.Common.Logger;

namespace Yintai.Architecture.FileUploadServer
{
    public partial class Service1 : ServiceBase
    {
        private ServiceHost _host;
        private readonly ILog _logger = LoggerManager.Current();


        public Service1()
        {
            InitializeComponent();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            _logger.Info("service start....");

            _host = new ServiceHostEx(typeof(ImageTool.Impl.ImageService), _logger);
            _host.Open();
        }

        protected override void OnStop()
        {
            if (_host != null)
            {
                _host.Close();
                _host = null;
            }

            _logger.Info("service stop ....");
        }
    }

    public class ServiceHostEx : ServiceHost
    {
        private readonly ILog _log;

        public ServiceHostEx(Type type, ILog log)
            : base(type)
        {
            this._log = log;
        }

        protected override void InitializeRuntime()
        {

            base.InitializeRuntime();

            foreach (ChannelDispatcher item in this.ChannelDispatchers)
            {
                item.ErrorHandlers.Add(new FaultHandler(_log));
            }
        }
    }

    public class FaultHandler : IErrorHandler
    {
        private readonly ILog _log;

        public FaultHandler(ILog log)
        {
            this._log = log;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (fault == null)
            {
                var fe = new FaultException<FaultException>(new FaultException(error.StackTrace));

                var mf = fe.CreateMessageFault();
                fault = Message.CreateMessage(version, mf, fe.Action);
            }
        }

        public bool HandleError(Exception error)
        {
            try
            {
                while (error != null)
                {
                    if (_log != null)
                    {
                        _log.Error(error);
                        error = error.InnerException;
                    }

                }
            }
            catch (Exception)
            {

            }

            return true;
        }
    }
}
