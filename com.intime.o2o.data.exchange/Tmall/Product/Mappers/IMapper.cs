
namespace com.intime.o2o.data.exchange.Tmall.Product.Mappers
{
    public interface IMapper<TInner, TChannel>
    {
        /// <summary>
        /// 本地到外部品牌的映射
        /// </summary>
        /// <param name="innerId">本地Id</param>
        /// <returns></returns>
        TChannel ToChannel(TInner innerId);

        /// <summary>
        /// 外部渠道到本地的映射
        /// </summary>
        /// <param name="outerId">外部Id</param>
        /// <returns>内部Id</returns>
        TInner FromChannel(TChannel outerId);

        /// <summary>
        /// 保存映射关系
        /// </summary>
        /// <param name="innerId">内部Id</param>
        /// <param name="outerId">外部Id</param>
        void Save(TInner innerId, TChannel outerId);
    }
}
