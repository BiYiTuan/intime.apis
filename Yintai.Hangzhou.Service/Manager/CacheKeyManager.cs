using System;
using System.Globalization;
using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Service.Manager
{
    ///// <summary>
    ///// cache key
    ///// </summary>
    //public class CacheKeyManager
    //{
    //    //公司名_项目名_功能名_小功能_方法名_参数
    //    private const string ApiDataServiceKeyPre = "yt_hz_api_ds";
    //    private const string ProductKeyPre = ApiDataServiceKeyPre + "_prod";
    //    private const string TagKeyPre = ApiDataServiceKeyPre + "_tag";
    //    private const string StoreKeyPre = ApiDataServiceKeyPre + "_store";
    //    private const string BrandKeyPre = ApiDataServiceKeyPre + "_brand";
    //    private const string PromotionKeyPre = ApiDataServiceKeyPre + "_pro";
    //    private const string TopicKeyPre = ApiDataServiceKeyPre + "_topic";
    //    private const string ItemsKeyPre = ApiDataServiceKeyPre + "_items";
    //    private const string FavorKeyPre = ApiDataServiceKeyPre + "_favor";
    //    private const string HotWordKeyPre = ApiDataServiceKeyPre + "_hotword";

    //    private static readonly int CacheDateSecond;
    //    private static readonly int CacheSeed;

    //    static CacheKeyManager()
    //    {
    //        CacheSeed = ConfigManager.GetCacheSeed();

    //        if (CacheSeed < 0)
    //        {
    //            CacheSeed = 0;
    //        }

    //        CacheDateSecond = 60 * CacheSeed;
    //    }

    //    #region hotword

    //    /// <summary>
    //    /// 产品详情 key
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="mark"></param>
    //    /// <returns>cache second</returns>
    //    public static int HotWordCollectionKey(out string key, string mark)
    //    {
    //        const string keypre = HotWordKeyPre + "_collection";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    #endregion

    //    #region favor

    //    /// <summary>
    //    /// 产品详情 key
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="mark"></param>
    //    /// <returns>cache second</returns>
    //    public static int FavorListKey(out string key, string mark)
    //    {
    //        const string keypre = FavorKeyPre + "_ls";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    #endregion

    //    #region pro

    //    public static int PromotionInfoKey(out string key, string mark)
    //    {
    //        const string keypre = PromotionKeyPre + "_info";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    public static int PromotionBannerKey(out string key, string mark)
    //    {
    //        const string keypre = PromotionKeyPre + "_banner";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    public static int PromotionListKey(out string key, string mark)
    //    {
    //        const string keypre = PromotionKeyPre + "_ls";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    #endregion

    //    #region topic

    //    public static int TopicListKey(out string key, string mark)
    //    {
    //        const string keypre = TopicKeyPre + "_ls";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    public static int TopicInfoKey(out string key, string mark)
    //    {
    //        const string keypre = TopicKeyPre + "_info";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    #endregion

    //    /// <summary>
    //    /// 产品详情 key
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="key"></param>
    //    /// <returns>cache second</returns>
    //    public static int ProductInfoKey(int id, out string key)
    //    {
    //        const string keypre = ProductKeyPre + "_info";

    //        key = String.Concat(keypre, "_", id.ToString(CultureInfo.InvariantCulture));

    //        return CacheDateSecond;
    //    }

    //    public static int ProductSearchKey(out string key, string mark)
    //    {
    //        const string keypre = ProductKeyPre + "_sch";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    public static int ProductListKey(out string key, string mark)
    //    {
    //        const string keypre = ProductKeyPre + "_ls";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    public static int TagAllKey(out string key)
    //    {
    //        const string tagKeyPre4All = TagKeyPre + "_all";
    //        key = tagKeyPre4All;

    //        return CacheDateSecond;
    //    }

    //    #region store

    //    public static int StoreAllKey(CoordinateInfo info, out string key)
    //    {
    //        const string keyPre4All = StoreKeyPre + "_all";

    //        key = info == null ? keyPre4All : String.Format("{0}_{1}_{2}", keyPre4All, info.Longitude, info.Latitude);

    //        return CacheDateSecond;
    //    }

    //    public static int StoreInfoKey(out string key, string mark)
    //    {
    //        const string keypre = StoreKeyPre + "_info";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    #endregion

    //    #region brand

    //    public static int BrandAllKey(out string key)
    //    {
    //        const string keyPre4All = BrandKeyPre + "_all";

    //        key = keyPre4All;

    //        return CacheDateSecond;
    //    }

    //    public static int BrandAll4GroupKey(out string key)
    //    {
    //        const string keyPre4All = BrandKeyPre + "_all4group";

    //        key = keyPre4All;

    //        return CacheDateSecond;
    //    }

    //    #endregion

    //    #region items

    //    public static int ItemsListKey(out string key,string mark)
    //    {
    //        const string keypre = ItemsKeyPre + "_ls";

    //        key = String.Concat(keypre, "_", mark);

    //        return CacheDateSecond;
    //    }

    //    #endregion
    //}
}