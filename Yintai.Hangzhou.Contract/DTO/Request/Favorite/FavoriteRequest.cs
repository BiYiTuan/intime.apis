using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Favorite
{
    public class FavoriteCreateRequest : AuthRequest
    {
        public int SourceId { get; set; }

        public int SourceType { get; set; }
    }

    public class FavoriteListRequest : BaseRequest
    {
        public SourceType SType { get { return (SourceType)SourceType; } set { } }

        public int SourceType { get; set; }

        public int Sort { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public double Lng { get; set; }

        public double Lat { get; set; }

        public FavoriteSortOrder SortOrder
        {
            get { return (FavoriteSortOrder)Sort; }
        }

        public UserModel UserModel { get; set; }
    }

    public class GetFavoriteListRequest : FavoriteListRequest
    {
    }

    public class DarenFavoriteListRequest
        : FavoriteListRequest
    {
    }
}
