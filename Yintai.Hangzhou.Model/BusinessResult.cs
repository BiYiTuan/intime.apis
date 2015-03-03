using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model
{
    public class BusinessResult<T> where T:class
    {
        public bool IsSuccess { get; set; }
        public ErrorResult Error { get; set; }
        public T Result { get; set; }
    }
}
