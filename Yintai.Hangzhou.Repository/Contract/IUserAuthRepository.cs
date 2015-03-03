using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface IUserAuthRepository : IRepository<UserAuthEntity, int>
    {
        IQueryable<dynamic> AuthFilter(IQueryable<dynamic> query, int userId, int userRole);

    }
}
