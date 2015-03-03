using com.intime.fashion.common.message;
using com.intime.fashion.common.message.rabbit;
using com.intime.fashion.service;
using com.intime.fashion.service.analysis;
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

namespace com.intime.jobscheduler
{
    class UnityBootStrapper
    {
        public static void Init()
        {
            ServiceLocator.Current.RegisterSingleton<Yintai.Architecture.Common.Logger.ILog, Yintai.Architecture.Common.Logger.Log4NetLog>();
            ServiceLocator.Current.Register<IOrderRepository, OrderRepository>();
            ServiceLocator.Current.Register<IEFRepository<IMS_AssociateIncomeHistoryEntity>, EFRepository<IMS_AssociateIncomeHistoryEntity>>();
            ServiceLocator.Current.Register<IEFRepository<IMS_AssociateIncomeEntity>, EFRepository<IMS_AssociateIncomeEntity>>();
            ServiceLocator.Current.Register<IEFRepository<IMS_ComboEntity>, EFRepository<IMS_ComboEntity>>();
            ServiceLocator.Current.Register<IEFRepository<IMS_AssociateItemsEntity>, EFRepository<IMS_AssociateItemsEntity>>();

            ServiceLocator.Current.Register<IAssociateIncomeService, AssociateIncomeService>();

            ServiceLocator.Current.Register<AnalysisService, AnalysisService>();
            ServiceLocator.Current.Register<IMessageCenterProvider, MessageProvider>();
        }
    }
}
