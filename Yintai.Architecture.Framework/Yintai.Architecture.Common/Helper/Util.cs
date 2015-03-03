using System;
using System.Globalization;

namespace Yintai.Architecture.Common.Helper
{
    public class UtilHelper
    {
        /// <summary>
        /// 补齐操作,会抛异常
        /// </summary>
        /// <param name="num">需要补齐的基数</param>
        /// <param name="maxLength">长度</param>
        /// <param name="filledChar">需要补的字符</param>
        /// <returns></returns>
        public static string PreFilled(int num, int maxLength, char filledChar)
        {
            var t = num.ToString(CultureInfo.InvariantCulture);
            if (t.Length > maxLength)
            {
                throw new ArgumentException(String.Format("基数{0}大于指定长度{1}", num.ToString(CultureInfo.InvariantCulture), maxLength));
            }

            if (t.Length == maxLength)
            {
                return t;
            }
            //开始
            var tm = String.Empty;
            var n = String.Empty;

            for (var i = 0; i < (maxLength - t.Length); i++)
            {
                n += filledChar;
            }

            tm = n + t;

            if (tm.Length > maxLength)
            {
                throw new ArgumentException("计算错误");
            }

            return tm;
        }
    }
}
