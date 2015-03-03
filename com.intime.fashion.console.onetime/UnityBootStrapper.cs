using com.intime.fashion.common.message;
using com.intime.fashion.common.message.rabbit;
using com.intime.fashion.service;
using com.intime.fashion.service.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Repository.Impl;

namespace com.intime.fashion.console.onetime
{
    class UnityBootStrapper
    {
        public static void Init()
        {
            ConfigureCommon();
            ConfigureData();
            ConfigureService();
            
       
           
        }

        private static void ConfigureService()
        {
            ServiceLocator.Current.Register<IMessageCenterProvider, MessageProvider>();
            ServiceLocator.Current.Register<IComboService, ComboService>();
        }

        private static void ConfigureData()
        {
            ServiceLocator.Current.Register<IEFRepository<IMS_ComboEntity>, ComboRepository>();
            ServiceLocator.Current.Register<IEFRepository<IMS_AssociateItemsEntity>, EFRepository<IMS_AssociateItemsEntity>>();
            ServiceLocator.Current.Register<IEFRepository<IMS_Combo2ProductEntity>, EFRepository<IMS_Combo2ProductEntity>>();
            ServiceLocator.Current.Register<IAssociateIncomeService,AssociateIncomeService>();
            ServiceLocator.Current.Register<IEFRepository<IMS_AssociateIncomeHistoryEntity>,EFRepository<IMS_AssociateIncomeHistoryEntity>>();
            ServiceLocator.Current.Register<IResourceRepository, ResourceRepository>();
            ServiceLocator.Current.Register<IEFRepository<OrderTransactionEntity>,EFRepository<OrderTransactionEntity>>();
            ServiceLocator.Current.Register<IEFRepository<PaymentNotifyLogEntity>,EFRepository<PaymentNotifyLogEntity>>();
            ServiceLocator.Current.Register<IOrderRepository,OrderRepository>();
            ServiceLocator.Current.Register<IOrderLogRepository,OrderLogRepository>();
            ServiceLocator.Current.Register<IAuthKeysService,AuthKeysService>();
            ServiceLocator.Current.Register<IEFRepository<IMS_AssociateIncomeEntity>,EFRepository<IMS_AssociateIncomeEntity>>();

        }

        private static void ConfigureCommon()
        {
            ServiceLocator.Current.RegisterSingleton<Yintai.Architecture.Common.Logger.ILog, Yintai.Architecture.Common.Logger.Log4NetLog>();
        }
    }
}
