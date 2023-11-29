using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NetFx40WpfTest.Toolkit;
using NLog;

namespace NetFx40WpfTest.ViewModel
{
    public class NetworkChangeViewModel : ViewModelBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand<string> DefaultCommand { get; set; }

        public NetworkChangeViewModel()
        {
            DefaultCommand = new RelayCommand<string>(DefaultAction);
        }

        private void DefaultAction(string cmd)
        {
            switch (cmd)
            {
                case "test":
                    Log.Info("IsNetworkAvailable={}", NetworkHelper.IsNetworkAvailable());
                    Log.Info("IsPingOk={}", NetworkHelper.IsPingOk("10.0.11.50"));
                    Log.Info("IsPingOk={}", NetworkHelper.IsPingOk("10.0.11.55"));

                    break;
            }
        }
    }
}