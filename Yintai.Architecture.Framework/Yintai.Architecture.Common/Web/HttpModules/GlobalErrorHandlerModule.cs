using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.Extension;
using Yintai.Architecture.Framework.Utility;

namespace Yintai.Architecture.Common.Web.HttpModules
{
    public class GlobalErrorHandlerModule : IHttpModule
    {
        #region fields

        private static bool hasInitilized;
        private static readonly object syncRoot = new object();
        private static string eventSourceName;
        private static int unhandledExceptionCount;
        private static readonly ILog _log = LoggerManager.Current();

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            //注册对于全局错误的记录
            context.Error += OnError;

            #region 记录UnhandledException

            //使用Double-Check机制保证在多线程并发下只注册一次UnhandledException处理事件
            if (!hasInitilized)
            {
                lock (syncRoot)
                {
                    if (!hasInitilized)
                    {
                        //1. 按照.net的习惯，依然首先将该内容写入到系统的EventLog中
                        string webenginePath = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "webengine.dll");
                        //通过webengine.dll来查找asp.net的版本，eventlog的名称由asp.net+版本构成
                        if (!File.Exists(webenginePath))
                        {
                            throw new Exception(String.Format(CultureInfo.InvariantCulture, "Failed to locate webengine.dll at '{0}'.  This module requires .NET Framework 2.0.", webenginePath));
                        }

                        FileVersionInfo ver = FileVersionInfo.GetVersionInfo(webenginePath);
                        eventSourceName = string.Format(CultureInfo.InvariantCulture, "ASP.NET {0}.{1}.{2}.0", ver.FileMajorPart, ver.FileMinorPart, ver.FileBuildPart);

                        if (!EventLog.SourceExists(eventSourceName))
                        {
                            throw new Exception(String.Format(CultureInfo.InvariantCulture, "There is no EventLog source named '{0}'. This module requires .NET Framework 2.0.", eventSourceName));
                        }

                        //在出现问题后将内容记录下来
                        AppDomain.CurrentDomain.UnhandledException += (o, e) =>
                                                                          {
                                                                              if (Interlocked.Exchange(ref unhandledExceptionCount, 1) != 0)
                                                                                  return;

                                                                              string appId = (string)AppDomain.CurrentDomain.GetData(".appId");
                                                                              appId = appId ?? "No-appId";

                                                                              Exception currException;
                                                                              StringBuilder sb = new StringBuilder();
                                                                              sb.AppendLine(appId);
                                                                              for (currException = (Exception)e.ExceptionObject; currException != null; currException = currException.InnerException)
                                                                              {
                                                                                  sb.AppendFormat("{0}\n\r", currException.ToString());
                                                                                  _log.Error(currException);
                                                                              }

                                                                              EventLog eventLog = new EventLog { Source = eventSourceName };
                                                                              eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Error);
                                                                          };

                        //初始化后设置该值为true保证不再继续注册事件
                        hasInitilized = true;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// 记录asp.net未捕获的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnError(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context == null) return;

            var exception = context.Server.GetLastError();
            if (exception == null) return;


            string message = exception.Message;

            var httpException = exception as HttpException;

            int statusCode = 404;
            if (httpException != null)
            {
                statusCode = httpException.GetHttpCode();
            }

            //包括记录异常的内部包含异常
            while (exception != null)
            {
                _log.Error("Global:");
                _log.Error(exception);
                exception = exception.InnerException;
            }

            context.Server.ClearError();
            context.Response.TrySkipIisCustomErrors = true;

            //输出错误信息

            var format = context.Request[Define.Format];

            if (String.IsNullOrEmpty(format))
            {
                format = String.Empty; // 如果为空，将会使用默认值
            }
            var response = String.Empty;
            var result = new ExecuteResult()
                             {
                                 Message = "服务器正在维护请稍后重试！",
                                 StatusCode = StatusCode.InternalServerError
                             };
            switch (format.ToLower())
            {
                case Define.Json:
                    response = Utils.DataContractToJson(result);
                    context.Response.ContentType = "application/json; charset=utf-8";
                    break;
                case Define.Xml:
                    response = result.ToXml();
                    context.Response.ContentType = "text/xml; charset=utf-8";
                    break;
                default:
                    response = Utils.DataContractToJson(result);
                    context.Response.ContentType = "text/html; charset=utf-8";
                    break;
            }

            context.Response.ClearHeaders();
            context.Response.Clear();
            context.Response.StatusCode = 200;

            context.Response.Write(response);
        }

        #endregion
    }
}