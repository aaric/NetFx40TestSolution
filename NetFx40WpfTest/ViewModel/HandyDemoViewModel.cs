using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;

namespace NetFx40WpfTest.ViewModel
{
    public class HandyDemoViewModel : ViewModelBase
    {
        public RelayCommand<string> DefaultCommand { get; set; }

        public HandyDemoViewModel()
        {
            DefaultCommand = new RelayCommand<string>(DefaultAction);

            Account = "admin";
        }

        private string _account;

        public string Account
        {
            get { return _account; }
            set
            {
                _account = value;
                RaisePropertyChanged(() => Account);
            }
        }

        private async void DefaultAction(string cmd)
        {
            switch (cmd)
            {
                case "login":
                    MessageBox.Show("" + Account);
                    break;
            }
        }
    }
}