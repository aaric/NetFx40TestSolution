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
            Password = "123456";
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

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private void DefaultAction(string cmd)
        {
            switch (cmd)
            {
                case "login":
                    // Application.Current.Dispatcher.Invoke(() => {});
                    MessageBox.Show(string.Format("{0} {1}", Account, Password));
                    break;
            }
        }
    }
}