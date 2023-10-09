using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NLog;
using uPLibrary.Networking.M2Mqtt;

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
                        MqttClient client = new MqttClient("10.0.11.80", 1883,
                            false, null, null, null);
                        client.Connect("csharp-client");
                        if (client.IsConnected)
                        {
                            Log.Info("ok");
                            MessageBox.Show("ok");
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