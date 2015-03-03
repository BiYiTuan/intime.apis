using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model
{
   public class WeixinPayKey:BaseModel
    {
        public int GroupId { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string PaySignKey { get; set; }
        public string ParterId { get; set; }
        public string ParterKey { get; set; }

    }
}
