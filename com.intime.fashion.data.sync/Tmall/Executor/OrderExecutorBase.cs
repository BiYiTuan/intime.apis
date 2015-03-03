using com.intime.fashion.data.tmall.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using OrderStatus = com.intime.fashion.data.tmall.Order.OrderStatus;

namespace com.intime.fashion.data.sync.Tmall.Executor
{
    public abstract class OrderExecutorBase : ExecutorBase
    {
        protected OrderExecutorBase(DateTime benchTime, int pageSize) : base(benchTime, pageSize)
        {
        }

        public abstract OrderType OrderType { get; }

        /// <summary>
        /// 只获取已付款的订单
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <param name="callback"></param>
        protected void DoQuery(Expression<Func<JDP_TB_TRADE, bool>> whereCondition, Action<IQueryable<JDP_TB_TRADE>> callback)
        {
            using (var context = DbContextHelper.GetJushitaContext())
            {
                var linq =
                    context.JDP_TB_TRADE.Where(x => x.status == OrderStatus.WAIT_SELLER_SEND_GOODS && !context.Set<OrderSync>().Any(o => o.TmallOrderId == x.tid && o.Type == (int)OrderType));
                if (whereCondition != null)
                    linq = linq.Where(whereCondition);
                if (callback != null)
                    callback(linq);
            }
        }
    }
}