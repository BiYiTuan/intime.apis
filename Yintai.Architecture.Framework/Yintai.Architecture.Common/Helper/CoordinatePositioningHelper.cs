using System;
using Yintai.Architecture.Common.Models;

namespace Yintai.Architecture.Common.Helper
{
    /// <summary>
    /// CLR Version: 4.0.30319.261
    /// NameSpace: Yintai.Architecture.Common.Helper
    /// FileName: LocaltionHelper
    ///
    /// Created at 4/6/2012 2:55:07 PM
    /// Description: 
    /// </summary>
    public class CoordinatePositioningHelper
    {
        #region fields
        /// <summary>
        /// 地球半径
        /// </summary>
        private const double EarthRadius = 6378.137;

        #endregion

        #region .ctor

        #endregion

        #region properties

        #endregion

        #region methods

        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 获取两点间距离 KM
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="lng1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="lng2">经度2</param>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            var radLat1 = Rad(lat1);
            var radLat2 = Rad(lat2);
            var a = radLat1 - radLat2;
            var b = Rad(lng1) - Rad(lng2);
            var s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EarthRadius;
            s = Math.Round(s * 10000) / 10000;

            return s;
        }

        /// <summary>
        ///  已知A点 和距离差，求 B点经度
        /// </summary>
        /// <param name="coordinate1">经纬度 A</param>
        /// <param name="km">距离</param>
        /// <returns></returns>
        public static CoordinateInfo GetLngDistance(CoordinateInfo coordinate1, double km)
        {
            if (coordinate1 == null)
            {
                return null;
            }

            //纬度90度
            if (coordinate1.Latitude > 90 || coordinate1.Latitude < 90)
            {
                var lng2 = coordinate1.Longitude * 100000 - km * 1000 / Math.Cos(coordinate1.Latitude);

                return new CoordinateInfo(lng2 / 100000, coordinate1.Latitude);
            }

            return new CoordinateInfo(0, coordinate1.Latitude);
        }

        /// <summary>
        /// 获取两点间距离 KM
        /// </summary>
        /// <param name="coordinate1"></param>
        /// <param name="coordinate2"></param>
        /// <returns></returns>
        public static double GetDistance(CoordinateInfo coordinate1, CoordinateInfo coordinate2)
        {
            if (coordinate1 == null || coordinate2 == null)
            {
                return 0;
            }

            return GetDistance(coordinate1.Longitude, coordinate1.Latitude, coordinate2.Longitude, coordinate2.Latitude);
        }

        /// <summary>
        /// 将 经纬度 转换为 度 分 秒
        /// </summary>
        /// <param name="coordinate">经纬度</param>
        /// <returns></returns>
        public static DegreeMinuteSecondInfo GetDegree(double coordinate)
        {
            //度
            var degree = Math.Floor(coordinate);
            var xiaoshu = coordinate - degree;

            //分
            var m = xiaoshu * 60;
            var minute = Math.Floor(m);
            //秒
            var s = m - minute;
            var second = s * 60;

            return new DegreeMinuteSecondInfo
            {
                Degree = Convert.ToInt64(degree),
                Minute = Convert.ToInt64(minute),
                Second = second
            };
        }

        /// <summary>
        /// 将 度 分 秒 转换为 经纬度
        /// </summary>
        /// <param name="degree">度分秒</param>
        /// <returns>因为计算时小数位保留的原因，导致正反计算存在一定误差，但误差影响不是很大。1秒的误差就是几米的样子</returns>
        public static double GetCoordinate(DegreeMinuteSecondInfo degree)
        {
            if (degree == null)
            {
                return 0d;
            }

            return degree.Degree + (degree.Minute / 60) + (degree.Second / 3600);
        }

        /// <summary>
        /// 经度 距离 按照地球是完美的球体计算
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <returns></returns>
        public static double GetLongitudeDistance(double longitude, double latitude)
        {
            return longitude * 111d * Math.Cos(latitude);
        }

        /// <summary>
        /// 纬度 距离 按照地球是完美的球体计算
        /// </summary>
        /// <returns></returns>
        public static double GetLatitudeDistance(DegreeMinuteSecondInfo degree)
        {
            if (degree == null)
            {
                return 0d;
            }

            return degree.Degree * 111d + degree.Minute * 1.85 + degree.Second * 0.0309;
        }

        /// <summary>
        /// 公里转换为 纬度变化：纬度变化一度，球面南北方向距离变化：πR/180 ........111km 
        /// </summary>
        /// <param name="km">公里数</param>
        /// <returns></returns>
        public static double KMConvertToLatitude(double km)
        {
            // return km * (1d / 111d);

            return km * (0.009009009009009d);
        }

        /// <summary>
        /// 公里转换为 经度变化 公式：经度变化一度，球面东西方向距离变化：πR/180*cosB ....111*cosB 
        /// </summary>
        /// <param name="km">公里数</param>
        /// <param name="latitude">纬度</param>
        /// <returns></returns>
        public static double KMConvertToLongitude(double km, double latitude)
        {
            return (0.009009009009009d * Math.Cos(latitude)) * km;
        }

        #endregion
    }
}
