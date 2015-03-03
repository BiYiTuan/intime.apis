using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.service.analysis.DTO
{
    class StoreDetailEventResponse:EventResponse
    {
        protected override string ParaName
        {
            get { return "id"; }
        }
    }
}
