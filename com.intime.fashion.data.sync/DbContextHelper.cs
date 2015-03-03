using com.intime.fashion.data.tmall.Models;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.data.sync
{

    //test git push
    public class DbContextHelper
    {
        public static YintaiHangzhouContext GetDbContext()
        {
            return new YintaiHangzhouContext("YintaiHangzhouContext");
        }

        public static SYS_INFOContext GetJushitaContext(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new SYS_INFOContext();
            }
            return new SYS_INFOContext();
        }
    }
}
