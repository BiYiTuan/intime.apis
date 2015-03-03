using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace Yintai.Architecture.Common.Data.EF
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        DbContext Context { get; set; }
       // DbTransaction Transaction { get; set; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; set; }
       // public DbTransaction Transaction { get; set; }

        public void Commit()
        {
            if (Context != null)
            {
                Context.SaveChanges();
                //if (Transaction != null)
                //{
                //    if (Transaction.Connection != null)
                //    {
                //        Transaction.Commit();
                //    }
                //}
            }
        }

        public void Dispose()
        {
            if (Context != null)
            {
                //if (Transaction != null)
                //{
                //    Transaction.Dispose();
                //}

                //Transaction = null;
                Context.Dispose();
                Context = null;
            }
        }
    }
}