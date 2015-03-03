using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ICardRepository : IRepository<CardEntity, int>
    {
        CardEntity GetItemByCard(string cardno, CardType cardType, DataStatus? dataStatus);

        IEnumerable<CardEntity> GetListForUserId(int userId, CardType cardType, DataStatus? dataStatus);
    }
}
