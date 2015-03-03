using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yintai.Architecture.Common.Data.EF;

namespace Yintai.Hangzhou.Repository.Impl
{
    public static class EFRepositoryExt
    {
        public static void NotifyMessage<T>(this EFRepository<T> rep, Action messageAction)
            where T : Yintai.Architecture.Common.Models.BaseEntity
        {
            if (Transaction.Current == null)
            {
                //if no transaction enlist here, means implicit transaction for single action
                messageAction();
            }
            else
            {
                Transaction.Current.TransactionCompleted += new TransactionCompletedEventHandler((o, e) =>
                {
                    if (e.Transaction.TransactionInformation.Status == TransactionStatus.Committed)
                    {
                        messageAction();
                    }

                });
            }
        }
    }
}
