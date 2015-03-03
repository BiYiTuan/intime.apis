using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Yintai.Architecture.Common.Data.EF;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class CardRepository : RepositoryBase<CardEntity, int>, ICardRepository
    {
        private static Expression<Func<CardEntity, bool>> Filter(DataStatus? dataStatus, string cardno, CardType? cardType, int? userId)
        {
            var filter = PredicateBuilder.True<CardEntity>();

            if (dataStatus != null)
            {
                filter = filter.And(v => v.Status == (int)dataStatus);
            }

            if (!String.IsNullOrEmpty(cardno))
            {
                filter = filter.And(v => String.Compare(v.CardNo, cardno, StringComparison.OrdinalIgnoreCase) == 0);
            }

            if (cardType != null)
            {
                filter = filter.And(v => v.Type == (int)cardType);
            }

            if (userId != null)
            {
                filter = filter.And(v => v.User_Id == userId);
            }

            return filter;
        }

        public CardEntity GetItemByCard(string cardno, CardType cardType, DataStatus? dataStatus)
        {
            return base.Get(Filter(dataStatus, cardno, cardType, null)).SingleOrDefault();
        }

        public IEnumerable<CardEntity> GetListForUserId(int userId, CardType cardType, DataStatus? dataStatus)
        {
            return base.Get(Filter(dataStatus, null, cardType, userId));
        }
    }
}
