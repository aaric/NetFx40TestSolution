using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Receiving;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using NLog;

namespace NetFx40WpfTest.Toolkit
{
    /// <summary>
    /// MQTT 客户端工具类<br>
    ///
    /// https://github.com/jilonglv/MQTTnet
    /// </summary>
    public class MqttClientHelper
    {
        // 日志
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private MqttClientHelper()
        {
        }

        private static MqttClientHelper _instance;

        /// <summary>
        /// 单例工厂
        /// </summary>
        public static MqttClientHelper Instance
        {
            get { return _instance ?? (_instance = new MqttClientHelper()); }
        }

        // 自动维护客户端连接
        private static readonly IDictionary<string, MqttClient> MqttClientDict = new Dictionary<string, MqttClient>();

        /// <summary>
        /// 创建MQTT连接
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="context">消息处理器</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(string clientId, string username, string password,
            Action<MqttApplicationMessageReceivedEventArgs> context)
        {
            // 初始化设置
            MqttClientOptions options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                //.WithTcpServer(App.MqttServer, App.MqttPort)
                .WithTcpServer("127.0.0.1", 1883)
                .WithCredentials(username, password)
                .WithWillDelayInterval(10)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                .WithWillMessage(new MqttApplicationMessage()
                {
                    Topic = string.Format("/client/will/{0}", clientId),
                    Payload = Encoding.UTF8.GetBytes(string.Format("goodbye by {0}", clientId)),
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                })
                .WithCleanSession()
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .Build() as MqttClientOptions;

            // 绑定事件
            MqttClient client = new MqttFactory().CreateMqttClient() as MqttClient;
            if (null != client)
            {
                // 绑定连接事件
                client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e =>
                {
                    // 记录日志
                    Log.Info("MqttClientConnectedHandlerDelegate -> clientId={}, state=ok", clientId);
                });

                // 绑定断开事件
                client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e =>
                {
                    // 记录日志
                    Log.Info("MqttClientDisconnectedHandlerDelegate -> clientId={}, state=ok", clientId);
                    
                    // 自动重连
                    client.ReconnectAsync();

                    // 记录日志
                    Log.Info("MqttClientDisconnectedHandlerDelegate -> clientId={}, reconnect=auto", clientId);
                });

                // 绑定消息接收事件
                client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(context);

                // 连接
                MqttClientAuthenticateResult result = await client.ConnectAsync(options);
                if (MqttClientConnectResultCode.Success == result.ResultCode)
                {
                    MqttClientDict.Add(clientId, client);

                    Log.Info("ConnectAsync -> clientId={}, state=ok", clientId);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="topic">消息主题</param>
        /// <param name="payload">消息内容</param>
        /// <param name="level">消息等级（默认QoS0）</param>
        /// <param name="retain">是否保留消息（默认保留）</param>
        /// <returns></returns>
        public async Task<bool> PublishAsync(string clientId, string topic, string payload,
            MqttQualityOfServiceLevel level = MqttQualityOfServiceLevel.AtMostOnce, bool retain = true)
        {
            MqttClient client = MqttClientDict[clientId];
            if (null != client && client.IsConnected)
            {
                MqttClientPublishResult result = await client.PublishAsync(new MqttApplicationMessage()
                {
                    Topic = string.Format(topic, clientId),
                    Payload = Encoding.UTF8.GetBytes(payload),
                    QualityOfServiceLevel = level,
                    Retain = retain
                });

                Log.Info("PublishAsync -> clientId={}, state={}", clientId,
                    MqttClientPublishReasonCode.Success == result.ReasonCode);
                return MqttClientPublishReasonCode.Success == result.ReasonCode;
            }

            return false;
        }

        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="topic">消息主题</param>
        /// <returns></returns>
        public async Task<bool> SubscribeAsync(string clientId, string topic)
        {
            MqttClient client = MqttClientDict[clientId];
            if (null != client && client.IsConnected)
            {
                await client.SubscribeAsync(topic);

                Log.Info("SubscribeAsync -> clientId={}, state=ok", clientId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取消订阅主题
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="topic">消息主题</param>
        /// <returns></returns>
        public async Task<bool> UnsubscribeAsync(string clientId, string topic)
        {
            MqttClient client = MqttClientDict[clientId];
            if (null != client && client.IsConnected)
            {
                await client.UnsubscribeAsync(topic);

                Log.Info("UnsubscribeAsync -> clientId={}, state=ok", clientId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        public async Task<bool> DisconnectAsync(string clientId = null)
        {
            if (null != clientId)
            {
                // 指定连接
                MqttClient client = MqttClientDict[clientId];
                if (null != client && client.IsConnected)
                {
                    await client.DisconnectAsync();

                    Log.Info("DisconnectAsync -> clientId={}, state=ok", clientId);
                    return true;
                }
            }
            else
            {
                // 全部连接
                foreach (KeyValuePair<string, MqttClient> kv in MqttClientDict)
                {
                    MqttClient client = kv.Value;
                    if (client.IsConnected)
                    {
                        await client.DisconnectAsync();

                        Log.Info("DisconnectAsync -> clientId={}, state=ok", clientId);
                    }
                }

                return true;
            }

            return false;
        }
    }
}