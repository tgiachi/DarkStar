using System.Net;
using System.Net.Sockets;
using DarkSun.Network.Data;
using DarkSun.Network.Protocol.Builders;
using DarkSun.Network.Protocol.Live;
using DarkSun.Network.Protocol.Messages;
using DarkSun.Network.Server;
using DarkSun.Network.Session;
using Microsoft.Extensions.Logging.Abstractions;

namespace DarkSun.Tests;

[TestClass]
public class NetworkProtocolTests
{
    [TestMethod]
    public void TestMessageBuilder()
    {
        var messageBuilder = new MessagePackMessageBuilder(new NullLogger<MessagePackMessageBuilder>());
        var buffer = messageBuilder.BuildMessage(new PingMessageResponse() { TimeStamp = 1 });

        Assert.IsTrue(buffer.Length > 0);
    }

    [TestMethod]
    public void TestMessageParser()
    {
        var randomTimeStamp = new Random().Next();
        var messageBuilder = new MessagePackMessageBuilder(new NullLogger<MessagePackMessageBuilder>());
        var buffer = messageBuilder.BuildMessage(new PingMessageResponse() { TimeStamp = randomTimeStamp });

        var message = messageBuilder.ParseMessage(buffer);
        var pingMessage = (PingMessageResponse)message.Message;

        Assert.IsTrue(randomTimeStamp == pingMessage.TimeStamp);
    }

    [TestMethod]
    public async Task TestTcpServerAsync()
    {
        var server = new MessagePackNetworkServer(new NullLogger<MessagePackNetworkServer>(),
            new InMemoryNetworkSessionManager(),
            new MessagePackMessageBuilder(new NullLogger<MessagePackMessageBuilder>()),
            new DarkSunNetworkServerConfig() { Address = IPAddress.Any.ToString(), Port = 9000 });

        await server.StartAsync();
        Assert.IsTrue(server.IsStarted);

        await server.StopAsync();

        Assert.IsFalse(server.IsStarted);
    }

    [TestMethod]
    public async Task TestTcpServerWithMessagesAsync()
    {
        var messageBuilder = new MessagePackMessageBuilder(new NullLogger<MessagePackMessageBuilder>());
        var messagesToSend = new List<byte[]>
        {
            messageBuilder.BuildMessage(new PingMessageResponse() { TimeStamp = 1 }),
            messageBuilder.BuildMessage(new PongMessageResponse() { TimeStamp = 1 })
        };

        var server = new MessagePackNetworkServer(new NullLogger<MessagePackNetworkServer>(),
            new InMemoryNetworkSessionManager(),
            new MessagePackMessageBuilder(new NullLogger<MessagePackMessageBuilder>()),
            new DarkSunNetworkServerConfig() { Address = IPAddress.Any.ToString(), Port = 9000 });

        server.StartAsync().GetAwaiter().GetResult();
        Assert.IsTrue(server.IsStarted);
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync("127.0.0.1", 9000);

        foreach (var message in messagesToSend)
        {
            tcpClient.Client.Send(message);
        }

        await Task.Delay(3000);
    }
}
