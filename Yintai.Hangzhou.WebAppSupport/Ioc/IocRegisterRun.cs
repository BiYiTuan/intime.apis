using System.Collections.Generic;

namespace Yintai.Hangzhou.WebSupport.Ioc
{
    internal sealed class IocRegisterRun
    {
        private static readonly List<IIocRegister> _iocRegisters;
        private static readonly IocRegisterRun _registerRun;

        static IocRegisterRun()
        {
            _iocRegisters = new List<IIocRegister>();
            _registerRun = new IocRegisterRun();
        }

        private IocRegisterRun()
        {
            Add(new ImportantSupportIocRegister());
            Add(new ServiceIocRegister());
            Add(new RepositoryIocRegister());
            Add(new ConfigIocRegister());
            Add(new UnfinishedIocRegister());
        }

        public static IocRegisterRun Current
        {
            get { return _registerRun; }
        }

        public static void Add(IIocRegister register)
        {
            _iocRegisters.Add(register);
        }

        public void Register()
        {
            foreach (var iocRegister in _iocRegisters)
            {
                iocRegister.Register();
            }
        }
    }
}