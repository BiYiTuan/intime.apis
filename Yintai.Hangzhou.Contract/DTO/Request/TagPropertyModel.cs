using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
   public class TagPropertyModel
    {
            public string PropertyName { get; set; }
            public IEnumerable<string> Values { get; set; }
    }

}
