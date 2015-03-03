using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Card;
using Yintai.Hangzhou.Contract.DTO.Response.Card;

namespace Yintai.Hangzhou.Contract.Card
{
    public interface ICardDataService
    {
        /// <summary>
        /// bind
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CardInfoResponse> Binding(BindingRequest request);

        /// <summary>
        /// bind
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CardInfoResponse> UnBinding(BindingRequest request);

        /// <summary>
        /// get info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CardInfoResponse> GetInfo(GetCardInfoRequest request);
    }
}
