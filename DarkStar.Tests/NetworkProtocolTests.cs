using System.Net;
using System.Net.Sockets;
using DarkStar.Network.Data;
using DarkStar.Network.Protocol.Builders;
using DarkStar.Network.Protocol.Live;
using DarkStar.Network.Protocol.Messages.Server;
using DarkStar.Network.Server;
using DarkStar.Network.Session;
using Microsoft.Extensions.Logging.Abstractions;

namespace DarkStar.Tests;

[TestClass]
public class NetworkProtocolTests
{
    [TestMethod]
    public void TestMessageBuilder()
    {
        var messageBuilder = new ProtoBufMessageBuilder(new NullLogger<ProtoBufMessageBuilder>());
        var buffer = messageBuilder.BuildMessage(new PingMessageResponse() { TimeStamp = 1 });

        Assert.IsTrue(buffer.Length > 0);
    }

    [TestMethod]
    public void TestMessageParser()
    {
        var randomTimeStamp = new Random().Next();
        var messageBuilder = new ProtoBufMessageBuilder(new NullLogger<ProtoBufMessageBuilder>());
        var buffer = messageBuilder.BuildMessage(new PingMessageResponse() { TimeStamp = randomTimeStamp });

        var message = messageBuilder.ParseMessage(buffer);
        var pingMessage = (PingMessageResponse)message.Message;

        Assert.IsTrue(randomTimeStamp == pingMessage.TimeStamp);
    }

    [TestMethod]
    public async Task TestTcpServerAsync()
    {
        var server = new TcpNetworkServer(new NullLogger<TcpNetworkServer>(),
            new InMemoryNetworkSessionManager(),
            new ProtoBufMessageBuilder(new NullLogger<ProtoBufMessageBuilder>()),
            new DarkStarNetworkServerConfig() { Address = IPAddress.Any.ToString(), Port = 9000 });

        await server.StartAsync();
        Assert.IsTrue(server.IsStarted);

        await server.StopAsync();

        Assert.IsFalse(server.IsStarted);
    }

    [TestMethod]
    public async Task TestTcpServerWithMessagesAsync()
    {
        var messageBuilder = new ProtoBufMessageBuilder(new NullLogger<ProtoBufMessageBuilder>());
        var messagesToSend = new List<byte[]>
        {
            messageBuilder.BuildMessage(new PingMessageResponse() { TimeStamp = 1 }),
            messageBuilder.BuildMessage(new PongMessageResponse() { TimeStamp = 1 }),
            messageBuilder.BuildMessage(new ServerVersionResponseMessage() {Minor = 0, Build = 0, Major = 1} ),
        };

        var server = new TcpNetworkServer(new NullLogger<TcpNetworkServer>(),
            new InMemoryNetworkSessionManager(),
            new ProtoBufMessageBuilder(new NullLogger<ProtoBufMessageBuilder>()),
            new DarkStarNetworkServerConfig() { Address = IPAddress.Any.ToString(), Port = 9000 });

        await server.StartAsync();
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
