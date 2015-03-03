using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.intime.fashion.test.fixture;
using com.intime.fashion.common.Util;

namespace com.intime.fashion.common.test
{
    [TestClass]
    public class ChineseUtilTest
    {
        [TestMethod]
        public void ChineseFirstPinyinTest()
        {
            var input = ChineseUtilData.GetInput();
            var firstLetter = ChineseUtil.FirstPinYin(input, '0');
            Assert.AreEqual(firstLetter, 'Y');
        }
    }
}
