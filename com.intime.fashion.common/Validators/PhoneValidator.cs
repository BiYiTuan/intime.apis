using System;
using System.Text.RegularExpressions;

namespace com.intime.fashion.common.Validators
{
    /// <summary>
    /// 电话号码校验
    /// </summary>
    public class PhoneValidator
    {
        static readonly Regex Regex = new Regex(@"^1\d{2}-?\d{4}-?\d{4}",RegexOptions.Compiled);
        public static bool ValidateMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return false;
            }
            return Regex.IsMatch(mobile);
        }

        /// <summary>
        /// 格式化手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string RegularMobileNo(string mobile)
        {
            if (!ValidateMobile(mobile))
            {
                throw new ArgumentException("Invalid format mobile phone number!");
            }

            return mobile.Replace("-",string.Empty);
        }
    }
}
