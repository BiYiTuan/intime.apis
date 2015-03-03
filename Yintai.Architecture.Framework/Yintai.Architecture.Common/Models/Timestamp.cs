using System;
using System.Globalization;

namespace Yintai.Architecture.Common.Models
{
    [Serializable]
    public class Timestamp
    {
        public Timestamp(TimestampType tsType, DateTime dateTime)
        {
            TsType = tsType;
            Ts = dateTime;
        }

        public Timestamp()
            : this(TimestampType.Default, DateTime.Now)
        {
        }

        public DateTime Ts { get; set; }

        /// <summary>
        ///  1 获取新于Ts 2 获取老于Ts的
        /// </summary>
        public TimestampType TsType { get; set; }

        /// <summary>
        /// 重写了TO STRING
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var t = Ts.ToString("yyyy-MM-dd_HH:mm:ss");
            var s = (int)TsType;

            return t + s.ToString(CultureInfo.InvariantCulture);
        }
    }

    [Serializable]
    public enum TimestampType
    {
        /// <summary>
        ///  == old
        /// </summary>
        Default = 0,
        /// <summary>
        /// 取最新的
        /// </summary>
        New = 1,

        /// <summary>
        /// 取当前点前的
        /// </summary>
        Old = 2
    }
}
