using System;
using System.Net;
using System.Text;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace NetFx40ConsoleTest
{
    /// <summary>
    /// https://github.com/jilonglv/MQTTnet
    ///
    /// <code>Install-Package MQTTnet40 -Version 1.0.0</code>
    /// </summary>
    public class MQTTnet40Test
    {
        public static void Main1(string[] args)
        {
            // CreateServer("127.0.0.1", 1883);
            CreateClient("10.0.11.80", 1883, "csharp",
                "csharp", "csharp123");

            // Wait for 15 seconds
            Thread.Sleep(TimeSpan.FromSeconds(15));
        }

        private static async void CreateClient(string serverIp, int serverPort, string clientId, string authAccount,
            string authPassword)
        {
            MqttClientOptions options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(serverIp, serverPort)
                .WithCredentials(authAccount, authPassword)
                .WithWillDelayInterval(60)
                .WithWillMessage(new MqttApplicationMessage()
                {
                    Topic = string.Format("/test/will/{0}", clientId),
                    Payload = Encoding.UTF8.GetBytes(string.Format("goodbye by {0}", clientId)),
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                })
                .WithCleanSession()
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .Build() as MqttClientOptions;

            MqttClient client = new MqttFactory().CreateMqttClient() as MqttClient;

            // https://www.cnblogs.com/yakniu/p/16423899.html
            // https://github.com/dotnet/MQTTnet/wiki/Client
            if (null != client)
            {
                client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e =>
                {
                    // TODO 绑定连接事件
                    Console.WriteLine("Connected");
                });

                client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e =>
                {
                    // TODO 绑定断开事件
                    Console.WriteLine("Disconnected");
                });

                client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    // 绑定消息接收事件
                    Console.WriteLine();
                });

                // await client.DisconnectAsync();
                await client.ConnectAsync(options);

                Console.WriteLine("OK");

                // 发布消息
                await client.PublishAsync(new MqttApplicationMessage()
                {
                    Topic = string.Format("/test/pub/{0}", clientId),
                    Payload = Encoding.UTF8.GetBytes("hello world"),
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce,
                    Retain = true
                });

                // 订阅主题
                // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
                //await client.UnsubscribeAsync("/test/sub/#");
                await client.SubscribeAsync("/test/sub/#");
            }
        }

        private static async void CreateServer(string serverIp, int serverPort)
        {
            MqttServerOptions options = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(serverIp))
                .WithDefaultEndpointPort(serverPort)
                .WithDefaultCommunicationTimeout(TimeSpan.FromSeconds(30))
                .WithConnectionValidator(context =>
                {
                    Console.WriteLine(string.Format("{0}", context.RawPassword));
                    Console.WriteLine(string.Format("{0} - {1}", context.Username, context.Password));
                    if (!"csharp".Equals(context.Username) || !"csharp123".Equals(context.Password))
                    {
                        context.ReasonCode = MqttConnectReasonCode.NotAuthorized;
                    }
                    else
                    {
                        context.ReasonCode = MqttConnectReasonCode.Success;
                    }
                }).Build() as MqttServerOptions;

            MqttServer server = new MqttFactory().CreateMqttServer() as MqttServer;

            // https://github.com/dotnet/MQTTnet/wiki/Upgrading-guide
            if (null != server)
            {
                server.StartedHandler = new MqttServerStartedHandlerDelegate(e =>
                {
                    // TODO 绑定服务端启动事件
                    Console.WriteLine("Started");
                });

                server.StoppedHandler = new MqttServerStoppedHandlerDelegate(e =>
                {
                    // TODO 绑定服务端停止事件
                    Console.WriteLine("Stopped");
                });

                server.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(e =>
                {
                    // TODO 绑定客户端连接事件
                    Console.WriteLine("Connected");
                });

                server.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(e =>
                {
                    // TODO 绑定客户端断开事件
                    Console.WriteLine("Disconnected");
                });

                server.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(e =>
                {
                    // TODO 绑定客户端订阅主题事件
                    Console.WriteLine("SubscribedTopic");
                });

                server.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(e =>
                {
                    // 绑定客户端退订主题事件
                    Console.WriteLine("UnsubscribedTopic");
                });

                server.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
                {
                    // TODO 绑定消息接收事件
                    Console.WriteLine("MessageReceived");
                });

                // await server.StopAsync();
                await server.StartAsync(options);

                Console.WriteLine("OK");
            }
        }
    }
}