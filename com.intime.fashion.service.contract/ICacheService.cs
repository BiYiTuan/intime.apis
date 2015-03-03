using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.contract
{
    public interface ICacheService
    {
        IEnumerable<T> GetList<T>(string name,Func<IEnumerable<T>> refreshResult);

        T Get<T>(string name, Func<T> refreshResult) where T: class;
    }
}
