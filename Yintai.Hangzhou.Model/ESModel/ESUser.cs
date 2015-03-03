using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESUser
    {
        public int Id { get; set; }
        public ESResource Thumnail { get; set; }
        public int Status { get; set; }
        public string Nickie { get; set; }
        public int Level { get; set; }
    }
}
