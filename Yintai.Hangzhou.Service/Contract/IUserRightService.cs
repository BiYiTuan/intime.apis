using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IUserRightService
    {

        IEnumerable<RoleEntity> LoadAllRolesRight();

        IEnumerable<AdminAccessRightEntity> LoaddAllRights();
    }
}
