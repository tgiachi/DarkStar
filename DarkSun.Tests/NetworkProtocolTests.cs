using DarkSun.Network.Protocol.Builders;
using DarkSun.Network.Protocol.Messages;
using Microsoft.Extensions.Logging.Abstractions;

namespace DarkSun.Tests
{
    [TestClass]
    public class NetworkProtocolTests
    {

        [TestMethod]
        public void TestMessageBuilder()
        {
            var messageBuilder = new MessagePackMessageBuilder(new NullLogger<MessagePackMessageBuilder>());
            var buffer = messageBuilder.BuildMessage(new PingMessageResponse() { Timestamp = 1 });

            Assert.IsTrue(buffer.Length > 0);

        }

        [TestMethod]
        public void TestMessageParser()
        {
            var randomTimeStamp = new Random().Next();
            var messageBuilder = new MessagePackMessageBuilder(new NullLogger<MessagePackMessageBuilder>());
            var buffer = messageBuilder.BuildMessage(new PingMessageResponse() { Timestamp = randomTimeStamp });

            var message = messageBuilder.ParseMessage(buffer);
            var pingMessage = (PingMessageResponse)message.Message;

            Assert.IsTrue(randomTimeStamp == pingMessage.Timestamp);
        }
    }
}
