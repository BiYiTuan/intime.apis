using System;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class FavoriteService : BaseService, IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUserService _userService;

        public FavoriteService(IFavoriteRepository favoriteRepository, IUserService userService)
        {
            this._favoriteRepository = favoriteRepository;
            this._userService = userService;
        }

        public FavoriteEntity Create(FavoriteEntity entity)
        {
            var r = this._favoriteRepository.Insert(entity);
            if (r != null)
            {
                //TODO:对用户账户操作
                this._userService.AddFover(entity.User_Id, 1, entity.CreatedUser);
            }

            return r;
        }

        public FavoriteEntity Get(int userId, int soureceId, SourceType sourceType)
        {
            return this._favoriteRepository.GetItem(userId, sourceType, soureceId);
        }

        public void Del(FavoriteEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entity.Status = (int)DataStatus.Deleted;
            this._favoriteRepository.Delete(entity);

            //TODO: 账户减少一个fover
            this._userService.SubFover(entity.User_Id, 1, entity.User_Id);
        }
    }
}
