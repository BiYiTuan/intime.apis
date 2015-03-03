using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.intime.o2o.data.exchange.Ims.Domain;
using com.intime.o2o.data.exchange.IT;

namespace com.intime.o2o.data.exchange.Ims.Response
{
    public class QueryOrderStatusResponse:Response<IEnumerable<LogisticStatus>>
    {
    }
}
