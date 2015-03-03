using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Contract;

namespace Yintai.Hangzhou.Service.Impl
{
    public class LikeService : BaseService, ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public LikeEntity Get(int likeUserId, int likedUserId)
        {
            return _likeRepository.GetItem(likeUserId, likedUserId);
        }
    }
}
