using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Util
{
    /// <summary>
    ///  per GBK standard, extract the input chinese's first Pinyin character
    ///  
    /// </summary>
    public static class ChineseUtil
    {
        //offset of code point
        private static readonly int GB_SP_DIFF = 160;
        //pinyin's code point start
        private static readonly int[] secPosValueList = { 1601, 1637, 1833, 2078, 2274, 2302, 2433, 2594, 2787, 3106, 3212,
        3472, 3635, 3722, 3730, 3858, 4027, 4086, 4390, 4558, 4684, 4925,
        5249, 5600 };
        //pinyin's characeter
        private static readonly char[] firstLetter = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'w', 'x', 'y', 'z' };

        public static char FirstPinYin(string chinese, char notMatchChar)
        {
            char result = notMatchChar;
            var firstStr = chinese.Substring(0,1).ToUpper().ToCharArray()[0];
            //encoding chinese by GBK
            var bytes = Encoding.GetEncoding("GBK").GetBytes(firstStr.ToString());
            if (bytes.Length < 2)
                return firstStr;
            int secPosValue = 0;
            int i;
            //each byte decrease offset 
            for (i = 0; i < bytes.Length; i++)
            {
                bytes[i] -= (byte)GB_SP_DIFF;
            }
            //convert to int as code point
            secPosValue = bytes[0] * 100 + bytes[1];
            for (i = 0; i < secPosValueList.Length - 1; i++)
            {
                if (secPosValue >= secPosValueList[i] && secPosValue < secPosValueList[i + 1])
                {
                    result = firstLetter[i];
                    break;
                }
            }
            return result.ToString().ToUpper().ToCharArray()[0];
        }

    }
}
