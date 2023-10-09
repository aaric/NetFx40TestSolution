using System;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NLog;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace NetFx40WpfTest.ViewModel
{
    public class M2MqttViewModel : ViewModelBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand<string> DefaultCommand { get; set; }

        public M2MqttViewModel()
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
                        string clientId = "csharp-client";
                        MqttClient client = new MqttClient("10.0.11.80", 1883, false, null);
                        client.Connect(clientId, "test", "test");
                        if (client.IsConnected)
                        {
                            client.Publish(string.Format("/test/topic/{0}", clientId),
                                Encoding.UTF8.GetBytes("hello word"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                            Log.Info("send ok");
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