using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Common.Web.Mvc.ActionResults;
using Yintai.Architecture.Framework.ServiceLocation;

namespace com.intime.fashion.common
{
   public static class CommonUtil
    {
        public static RestfulResult RenderError(Action<ExecuteResult> callback)
        {
            var result = new RestfulResult
            {
                Data = new ExecuteResult { StatusCode = StatusCode.InternalServerError, Message = "操作失败！" }

            };
            if (callback != null)
                callback(result.Data as ExecuteResult);
            return result;
        }
        public static RestfulResult RenderSuccess<T>(Action<ExecuteResult<T>> callback)
        {
            var result = new RestfulResult
            {
                Data = new ExecuteResult<T>
                {
                    StatusCode = StatusCode.Success,
                    IsSuccess = true,
                    Message = "操作成功！"
                }

            };
            if (callback != null)
                callback(result.Data as ExecuteResult<T>);
            return result;
        }

        public static ILog Log { get {
            return ServiceLocator.Current.Resolve<ILog>();
        } }
       
        public static string MD5_Encode(string value,Encoding encode)
        {

            byte[] hashData = System.Security.Cryptography.MD5.Create().ComputeHash((encode.GetBytes(value)));
            var hashText = BitConverter.ToString(hashData).Replace("-", "").ToLower();
            return hashText;

        }

        public static int Yuan2Fen(decimal input)
        {
            return (int)(input * 100);
        }
    }
}
