using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync.Wgw.Request.Builder
{
    public abstract class RequestParamsBuilder
    {
        protected ISyncRequest Request;
        protected RequestParamsBuilder(ISyncRequest request)
        {
            this.Request = request;
        }

        /// <summary>
        /// 构建应用级别参数实体
        /// </summary>
        /// <returns></returns>
        public abstract ISyncRequest BuildParameters(object entity);

        protected YintaiHangzhouContext GetDbContext()
        {
            return DbContextHelper.GetDbContext();
        }
    }
}
