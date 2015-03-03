using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yintai.Hangzhou.WebApiCore.Areas.Gg.Controllers;

namespace Yintai.Hangzhou.WebApiCore.Test
{
    [TestClass]
    public class OrderControllerTest
    {
        [TestMethod]
        public void TestVoid()
        {

            OrderController controller = new OrderController();
            var rsp =controller.Void("{orderno:'11111'}", "yintai");
            Assert.IsNotNull(rsp);
        }
    }
}
