using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Repository.Contract
{
    public interface ISeedRepository
    {
        /// <summary>
        ///  返回 -2 为超出限制
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxSeed"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        int Generate(string name, int maxSeed, int k);
    }
}
