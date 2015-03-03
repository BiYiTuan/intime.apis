using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Sample.Web.Controllers
{
    class BinaryFormatter : MediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Byte[]);
        }

        
    }
}
