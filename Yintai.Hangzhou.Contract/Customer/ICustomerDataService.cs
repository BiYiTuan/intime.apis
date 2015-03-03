using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Customer;
using Yintai.Hangzhou.Contract.DTO.Response.Customer;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Contract.Customer
{
    public interface ICustomerDataService
    {
        /// <summary>
        /// 请求达人信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<ShowCustomerInfoResponse> GetShowCustomer(ShowCustomerRequest request);

        /// <summary>
        /// 外站用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CustomerInfoResponse> OutSiteLogin(OutSiteLoginRequest request);

        /// <summary>
        /// 外站用户登录2
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UserEntity OutSiteLogin2(OutSiteLoginRequest request);

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CustomerInfoResponse> GetUserInfo(GetUserInfoRequest request);

        /// <summary>
        /// 绑定手机号，发送验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult VerifyBindMobile(VerifyBindMobileRequest request);

        /// <summary>
        /// 绑定手机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult BindMobile(BindMobileRequest request);

        /// <summary>
        /// 上传客户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CustomerInfoResponse> UploadLogo(UploadLogoRequest request);

        /// <summary>
        /// 删除客户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CustomerInfoResponse> DestroyLogo(DestroyLogoRequest request);

        /// <summary>
        /// 修改个人资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ExecuteResult<CustomerInfoResponse> Update(UpdateCustomerRequest request);
    }
}
