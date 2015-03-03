using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Response.HotKey;

namespace Yintai.Hangzhou.Contract.HotWord
{
    public interface IHotwordDataService
    {
        ExecuteResult<HotWordCollectionResponse> GetCollection();
    }
}
