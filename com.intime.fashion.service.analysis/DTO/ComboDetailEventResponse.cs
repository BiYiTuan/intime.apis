using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.fashion.service.analysis.DTO
{
    class ComboDetailEventResponse:EventResponse
    {
        protected override string ParaName
        {
            get { return "id"; }
        }

       
    }
}
