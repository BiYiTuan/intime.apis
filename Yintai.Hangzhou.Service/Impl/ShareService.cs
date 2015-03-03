using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class ShareService : BaseService, IShareService
    {
        private readonly IShareRepository _shareRepository;
        private readonly IUserService _userService;

        public ShareService(IShareRepository shareRepository, IUserService userService)
        {
            this._shareRepository = shareRepository;
            this._userService = userService;
        }

        public ShareHistoryEntity Create(ShareHistoryEntity shareHistoryEntity)
        {
            if (shareHistoryEntity == null)
            {
                return null;
            }

            var entity = this._shareRepository.Insert(shareHistoryEntity);

            if (entity != null)
            {
                //TODO:用户账户操作
                this._userService.AddShare(shareHistoryEntity.User_Id, 1, shareHistoryEntity.CreatedUser);
            }

            return entity;
        }
    }
}
