using System;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;

namespace NetFx40ConsoleTest
{
    /// <summary>
    /// https://github.com/eclipse/paho.mqtt.m2mqtt
    ///
    /// <code>Install-Package M2Mqtt -Version 4.1.0</code>
    /// </summary>
    public class M2MqttTests
    {
        public static void Main1(string[] args)
        {
            CreateClient("127.0.0.1", 1883, "csharp",
                "csharp", "csharp123");

            // Wait for 15 seconds
            Thread.Sleep(TimeSpan.FromSeconds(15));
        }

        private static void CreateClient(string serverIp, int serverPort, string clientId, string authAccount,
            string authPassword)
        {
            MqttClient client = new MqttClient(serverIp, serverPort,
                false, null, null, null);

            //client.Disconnect();
            client.Connect("csharp");

            if (client.IsConnected)
            {
                // 发布消息
                client.Publish("/test/pub/{0}", Encoding.UTF8.GetBytes("hello world"));

                // 订阅主题
                client.Subscribe(new string[] { "/test/sub/{0}" }, null);
            }
        }
    }
}