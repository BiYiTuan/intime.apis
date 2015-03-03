using System.Runtime.Serialization;
using System.Web;
using Yintai.Architecture.Common;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Customer
{
    [DataContract]
    public class LoginRequest
    {
    }

    [DataContract]
    public class RegisterRequest
    {
    }

    [DataContract]
    public class GetUserInfoRequest : AuthRequest
    {

    }

    public class PortraitRequest : AuthRequest
    {
        public ThumnImageType? Type { get; set; }
    }

    public class UploadLogoRequest : PortraitRequest
    {
        public UploadLogoRequest(PortraitRequest b)
        {
            this.AuthUid = b.AuthUid;
            this.AuthUser = b.AuthUser;
            this.Method = DefineRestfulMethod.Create;
            this.Token = b.Token;
            this.Type = b.Type.HasValue?b.Type.Value:ThumnImageType.Logo;
        }

        public HttpFileCollectionBase Files { get; set; }
    }

    public class DestroyLogoRequest : PortraitRequest
    {
        public DestroyLogoRequest(PortraitRequest b)
        {
            this.AuthUid = b.AuthUid;
            this.AuthUser = b.AuthUser;
            this.Method = DefineRestfulMethod.Destroy;
            this.Token = b.Token;
            this.Type = b.Type.HasValue ? b.Type.Value : ThumnImageType.Logo;
        }
    }

    public class BindStoreCustomerRequest : AuthRequest
    {
        public int StoreId { get; set; }
    }

    public class UpdateCustomerRequest : AuthRequest
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 性别 1男 2女
        /// </summary>
        public byte? Gender { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 签名（说明）
        /// </summary>
        public string Desc { get; set; }
    }

    [DataContract]
    public class GetShowUserInfoRequest
    {
    }

    [DataContract]
    public class ShowCustomerRequest : BaseRequest
    {
        /// <summary>
        /// 达人Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 当前请求的用户，可以是匿名的
        /// </summary>
        public UserModel CurrentAuthUser { get; set; }
    }

    [DataContract]
    public class OutSiteLoginRequest : AuthRequest
    {
        public string OutsiteToken { get; set; }

        public string OutsiteUid { get; set; }

        public string OutsiteNickname { get; set; }

        public int OutsiteType { get; set; }

        public OutsiteType OsType
        {
            get { return (OutsiteType)OutsiteType; }
            set { OutsiteType = (int)value; }
        }

        /// <summary>
        /// 外展用户头像地址（有带商榷，是否符合我们系统的图片规格等因素）
        /// </summary>
        public string ThumnailUrl { get; set; }
    }

    [DataContract]
    public class VerifyBindMobileRequest : AuthRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember]
        public string Moblie { get; set; }

        public int Type
        {
            get { return 1; }
            set { }
        }

        public int Mode
        {
            get { return 1; }
            set { }
        }
    }

    [DataContract]
    public class BindMobileRequest : AuthRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember]
        public string Moblie { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        public int Type
        {
            get { return 1; }
            set { }
        }

        public int Mode
        {
            get { return 1; }
            set { }
        }
    }

}
