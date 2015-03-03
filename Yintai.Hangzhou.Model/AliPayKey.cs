using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model
{
   public  class AliPayKey:BaseModel
    {
        public string ParterId { get; set; }
        public string Md5Key { get; set; }
        public int GroupId { get; set; }
        public string SellerAccount { get; set; }
    }
}
