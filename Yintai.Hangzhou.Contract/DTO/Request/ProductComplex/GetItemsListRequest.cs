using Yintai.Hangzhou.Model;

namespace Yintai.Hangzhou.Contract.DTO.Request.ProductComplex
{
    public class GetItemsListRequest : ListRequest
    {
        public UserModel UserModel { get; set; }
    }
}
