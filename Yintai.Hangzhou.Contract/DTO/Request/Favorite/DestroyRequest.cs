using Yintai.Hangzhou.Contract.DTO.Request;

namespace Yintai.Hangzhou.Contract.Request.Favorite
{
    public class FavoriteDestroyRequest: AuthRequest
    {
        public int FavoriteId { get; set; }
    }
}