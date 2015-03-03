using System;
using System.Runtime.Serialization;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Model;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public interface IAuth
    {
        /// <summary>
        /// 认证信息
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// 如果 为-1 标识未认证，登录用户的IDauthuid
        /// </summary>
        int AuthUid
        {
            get;
            set;
        }

        /// <summary>
        /// 已经认证的用户
        /// </summary>
        UserModel AuthUser { get; set; }
    }

    public interface IPagedRequest
    {
        int Page { get; set; }

        int Pagesize { get; set; }

        PagerRequest PagerRequest { get; set; }
    }

    public interface IRestfulRequest
    {
        string Method { get; set; }

        string Client_Version { get; set; }
    }

    [Serializable]
    public abstract class BaseRequest : IRestfulRequest
    {
        public string Method { get; set; }
        public string Client_Version { get; set; }
        public string Channel { get; set; }
        public float Version
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Client_Version))
                {
                    return 0;
                }

                var pos = Client_Version.IndexOf(".", StringComparison.Ordinal);

                if (pos <= 0)
                {
                    return Single.Parse(Client_Version);
                }

                var c = Client_Version.Replace(".", String.Empty);

                var t = c.Insert(pos, ".");

                return Single.Parse(t);
            }
            set { }
        }
    }

    [DataContract]
    public abstract class AuthRequest : BaseRequest, IAuth
    {
        ///// <summary>
        ///// 签名
        ///// </summary>
        //public string Sign { get; set; }

        ///// <summary>
        ///// 客户唯一标识，不做业务使用
        ///// </summary>
        //public string Uid { get; set; }

        ///// <summary>
        ///// 客户端版本
        ///// </summary>
        //public string Client_Version { get; set; }

        /// <summary>
        /// 认证信息
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }

        /// <summary>
        /// 如果 为-1 标识未认证，登录用户的IDauthuid
        /// </summary>
        [DataMember(Name = "authuid")]
        [System.Obsolete("建议使用AuthUser 对象")]
        public int AuthUid
        {
            get;
            set;
        }

        /// <summary>
        /// 已经认证的用户
        /// </summary>
        [DataMember(Name = "authuser")]
        public UserModel AuthUser { get; set; }
    }

    [DataContract]
    public abstract class ListRequest : RefreshRequest
    {
        /// <summary>
        /// 默认最大 pageSize 40
        /// </summary>
        protected ListRequest()
            : this(40)
        {
        }

        protected ListRequest(int maxPageSize)
            : base(maxPageSize, TimestampType.Old)
        {
        }
    }

    [DataContract]
    public abstract class RefreshRequest : PagerInfoRequest
    {
        private readonly TimestampType _type;

        /// <summary>
        /// 默认最大 pageSize 40
        /// </summary>
        protected RefreshRequest()
            : this(40)
        {
        }

        protected RefreshRequest(int maxPageSize)
            : this(maxPageSize, TimestampType.New)
        {
        }

        protected RefreshRequest(TimestampType type)
            : this(40, type)
        {
        }

        protected RefreshRequest(int maxPageSize, TimestampType type)
            : base(maxPageSize)
        {
            _type = type;
        }

        private string _ts;

        /// <summary>
        /// 刷新时间戳
        /// </summary>
        public string RefreshTs
        {
            get { return String.IsNullOrEmpty(_ts) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : _ts; }
            set { this._ts = value; }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        public Timestamp Timestamp
        {
            get
            {
                return new Timestamp { TsType = _type, Ts = DateTime.Parse(RefreshTs) };
            }
            set { }
        }
    }

    [DataContract]
    public abstract class AuthPagerInfoRequest : PagerInfoRequest, IAuth
    {
        public string Token { get; set; }
        public int AuthUid { get; set; }
        public UserModel AuthUser { get; set; }
    }

    [DataContract]
    public class PagerInfoRequest : BaseRequest, IPagedRequest
    {
        private int _maxPageSize;

        public PagerInfoRequest()
            : this(40)
        {
        }

        public PagerInfoRequest(int maxPageSize)
        {
            if (maxPageSize < 0)
            {
                throw new ArgumentOutOfRangeException("maxPageSize", "最大页Size不能小于0");
            }

            this._maxPageSize = maxPageSize;
        }

        public virtual int Page { get; set; }

        public virtual int Pagesize { get; set; }

        public PagerRequest PagerRequest
        {
            get { return new PagerRequest(Page, Pagesize, _maxPageSize); }
            set { }
        }
    }

    [DataContract]
    public abstract class CoordinateRequest : AuthRequest
    {
        [DataMember(Name = "lng")]
        public double Lng { get; set; }

        [DataMember(Name = "lat")]
        public double Lat { get; set; }

        [IgnoreDataMember]
        public CoordinateInfo CoordinateInfo
        {
            get
            {
                if (Lng > 0 || Lng < 0)
                {
                    if (Lat > 0 || Lat < 0)
                    {
                        return null;
                    }
                }

                return new CoordinateInfo(Lng, Lat);
            }
        }
    }
}
