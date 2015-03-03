using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using com.intime.fashion.test.fixture;

namespace com.intime.fashion.common.message.test
{
    [TestClass]
    public class BaseMessageTest
    {
        [TestMethod]
        public void TestBaseMessageSerialize()
        {
            var rawMessage = BaseMessageData.GetRawMessage();
            var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(rawMessage);

            Assert.IsNotNull(baseMessage);
            Assert.AreEqual(baseMessage.ActionType, (int)MessageAction.UpdateEntity);
        }
    }
}
