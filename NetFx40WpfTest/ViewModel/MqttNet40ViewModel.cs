using System;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using NLog;

namespace NetFx40WpfTest.ViewModel
{
    public class MqttNet40ViewModel : ViewModelBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand<string> DefaultCommand { get; set; }

        public MqttNet40ViewModel()
        {
            DefaultCommand = new RelayCommand<string>(DefaultAction);
        }

        private async void DefaultAction(string cmd)
        {
            try
            {
                switch (cmd)
                {
                    case "connect":
                        // 初始化设置
                        string clientId = "csharp-client";
                        MqttClientOptions options = new MqttClientOptionsBuilder()
                            .WithClientId(clientId)
                            .WithTcpServer("10.0.11.80", 1883)
                            .WithCredentials("test", "test")
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
                                Log.Info("MqttClientConnectedHandlerDelegate -> clientId={}, state=ok",
                                    clientId);
                            });

                            // 绑定断开事件
                            client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e =>
                            {
                                // 记录日志
                                Log.Info("MqttClientDisconnectedHandlerDelegate -> clientId={}, state=ok",
                                    clientId);

                                // 自动重连
                                client.ReconnectAsync();

                                // 记录日志
                                Log.Info(
                                    "MqttClientDisconnectedHandlerDelegate -> clientId={}, reconnect=auto",
                                    clientId);
                            });

                            // 绑定消息接收事件
                            /*client.ApplicationMessageReceivedHandler =
                                new MqttApplicationMessageReceivedHandlerDelegate(context);*/

                            // 连接
                            MqttClientAuthenticateResult result = await client.ConnectAsync(options);
                            if (MqttClientConnectResultCode.Success == result.ResultCode)
                            {
                                Log.Info("ConnectAsync -> clientId={}, state=ok", clientId);

                                MessageBox.Show("ok");
                            }
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                Log.Error("{}", e.ToString());
                MessageBox.Show(e.ToString());
            }
        }
    }
}