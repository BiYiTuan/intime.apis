using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Yintai.Architecture.Common.Configuraton;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using System.Linq;

namespace Yintai.Architecture.Common.Web.Mvc
{
    public class ParameterManager
    {
        #region fields

        private static readonly ParameterManager current;
        private readonly List<IRequestParameterHandler> _requestHandlers;

        #endregion

        #region .ctor

        static ParameterManager()
        {
            current = new DefaultParameterManager();
        }

        protected ParameterManager()
        {
            _requestHandlers = new List<IRequestParameterHandler>();
        }

        #endregion

        #region Properties

        public static ParameterManager Current
        {
            get { return current; }
        }

        public List<IRequestParameterHandler> RequestParameterHandlers
        {
            get { return this._requestHandlers; }
        }

        #endregion

        #region Methods

        public bool Validate(ActionExecutingContext context)
        {
            foreach (var requestHandler in this.RequestParameterHandlers)
            {
                if (requestHandler.Validate(context)) continue;
                context.Result = requestHandler.Result;

                return false;
            }

            return true;
        }

        #endregion

        #region private class

        private class DefaultParameterManager : ParameterManager
        {
            public DefaultParameterManager()
            {
                this.RequestParameterHandlers.Add(new CloseServiceHandler());
                this.RequestParameterHandlers.Add(new FormatRequestHandler());
                this.RequestParameterHandlers.Add(new SignatureVerificationHandler());
            }
        }

        #endregion
    }

    /// <summary>
    /// 关闭服务
    /// </summary>
    public class CloseServiceHandler : RequestParameterHandlerBase
    {
        public override bool Validate(ActionExecutingContext context)
        {
            var isEnable = ConfigManager.IsCloseService;

            if (isEnable)
            {
                SetActionResult(new RestfulResult()
                {
                    Data = new ExecuteResult()
                    {
                        Message = "服务站暂不可用",
                        StatusCode = StatusCode.InternalServerError
                    }
                });

                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 返回值格式验证
    /// </summary>
    public class FormatRequestHandler : RequestParameterHandlerBase
    {
        public override bool Validate(ActionExecutingContext context)
        {
            var format = context.HttpContext.Request[Define.Format];

            if (String.IsNullOrEmpty(format))
            {
                return true; // 如果为空，将会使用默认值
            }

            // 判断Format是否为Xml或者Json
            if (!((format.ToLower() != Define.Xml) || (format.ToLower() != Define.Json)))
            {
                SetActionResult(new RestfulResult
                {
                    Data = "format 格式不正确"
                });
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 签名验证
    /// </summary>
    public class SignatureVerificationHandler : RequestParameterHandlerBase
    {
        private static void CommonParameterValidate(ActionExecutingContext context)
        {
            var request = context.RequestContext.HttpContext.Request;

            var uid = request[Define.Uid];
            if (String.IsNullOrEmpty(uid))
            {
                throw new System.ArgumentNullException(Define.Uid);
            }

            //获取客户端操作系统
            //var os = nameValueCollection[Define.OS];

            //if (string.IsNullOrEmpty(os))
            //{
            //    throw new System.ArgumentNullException(Define.OS);
            //}

            //获取时间戳
            //var timeReq = nameValueCollection[Define.TimeReq];

            //if (string.IsNullOrEmpty(timeReq))
            //{
            //    throw new System.ArgumentNullException(Define.TimeReq);
            //}

            //获取签名串
            var sign = request[Define.Sign];

            if (String.IsNullOrEmpty(sign))
            {
                throw new System.ArgumentNullException(Define.Sign);
            }

            //客户端版本
            var clientVersion = request[Define.ClientVersion];

            if (String.IsNullOrEmpty(clientVersion))
            {
                throw new ArgumentNullException(Define.ClientVersion);
            }

            #region 进行签名验证

            const string os = "iphone";
            var ver = GetFloatVersion(clientVersion);

            string appkey = ConfigManager.GetAppkey(ver > 2.09 ? (os + (2.10).ToString("F2")) : os);

            var vList = new Dictionary<string, string> { { Define.ClientVersion, HttpUtility.UrlDecode(clientVersion, Encoding.UTF8) }, { Define.Uid, HttpUtility.UrlDecode(uid, Encoding.UTF8) } };

            var builder = new StringBuilder();
            builder.Append(appkey);

            var kOrder = vList.Keys.OrderBy(v => v);
            foreach (var k in kOrder)
            {
                builder.AppendFormat("{0}{1}", k, vList[k]);
            }

            builder.Append(appkey);

            var str = GetMd5Hash(builder.ToString());

            var isOk = System.String.Compare(sign, str, StringComparison.OrdinalIgnoreCase) == 0;

            if (!isOk)
            {
                throw new System.Security.SecurityException("签名验证失败");
            }

            #endregion
        }

        private static float GetFloatVersion(string Client_Version)
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

        private static bool ValidateDispatch(ActionExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            CommonParameterValidate(context);

            return true;
        }

        public override bool Validate(ActionExecutingContext context)
        {
            var isEnable = ConfigManager.IsEnableSign;

            //如果没有开启
            if (!isEnable)
            {
                return true;
            }

            if (String.Compare(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return ValidateDispatch(context);
            }

            if (String.Compare(context.HttpContext.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return ValidateDispatch(context);
            }

            return ExceptionResult(String.Format("不支持当前的方法{0}", context.HttpContext.Request.HttpMethod));
        }

        #region ExceptionResult

        private bool ExceptionResult(string description)
        {
            var result = new RestfulResult
            {
                Data = new ExecuteResult
                {
                    StatusCode = StatusCode.ClientError,
                    Message = description
                }
            };

            SetActionResult(result);

            return false;
        }

        #endregion

        #region Hepler

        private static string GetMd5Hash(string input)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        #endregion
    }

    public abstract class RequestParameterHandlerBase : IRequestParameterHandler
    {
        #region fields

        private ActionResult _result = new EmptyResult();

        #endregion

        #region IRequestParameterHandler Members

        public abstract bool Validate(ActionExecutingContext context);

        public ActionResult Result
        {
            get { return _result; }
        }

        #endregion

        #region Methods

        protected void SetActionResult(ActionResult result)
        {
            _result = result;
        }

        #endregion
    }

    public interface IRequestParameterHandler
    {
        bool Validate(ActionExecutingContext context);

        ActionResult Result { get; }
    }
}
