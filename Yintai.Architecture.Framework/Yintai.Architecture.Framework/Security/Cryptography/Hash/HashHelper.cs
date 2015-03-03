using System.Collections.Generic;
using System.Linq;

namespace Yintai.Architecture.Framework.Security.Cryptography.Hash
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Architecture.Framework.Security.Cryptography.Hash
    /// FileName: HashHelper
    ///
    /// Created at 11/9/2012 11:29:07 AM
    /// Description: 
    /// </summary>
    public static class HashHelper
    {
        public static MD5 MD5(string input)
        {
            return MD5(input, false);
        }

        public static MD5 MD5(string input, bool igonreCase)
        {
            return new MD5(input, igonreCase);
        }

        public static SHA SHA1(string input)
        {
            return SHA1(input, false);
        }

        public static SHA SHA1(string input, bool igonreCase)
        {
            return new SHA().SHA1(input, igonreCase);
        }

        public static SHA SHA256(string input)
        {
            return SHA256(input, false);
        }

        public static SHA SHA256(string input, bool igonreCase)
        {
            return new SHA().SHA256(input, igonreCase);
        }
    }
}
