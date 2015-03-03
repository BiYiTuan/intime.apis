using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class UserRightService:IUserRightService
    {
        private IRoleRepository _roleDataService;
        public UserRightService(IRoleRepository roleDataService)
        {
            _roleDataService = roleDataService;
        }
        public IEnumerable<Data.Models.RoleEntity> LoadAllRolesRight()
        {
            return _roleDataService.LoadAllEagerly();
        }



        public IEnumerable<Data.Models.AdminAccessRightEntity> LoaddAllRights()
        {
            return _roleDataService.LoadAllRights();
        }
    }
}
