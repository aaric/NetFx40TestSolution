using System.Windows;
using AutoUpdaterDotNET;

namespace NetFx40WpfTest.View
{
    /// <summary>
    /// AutoUpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AutoUpdateWindow : Window
    {
        public AutoUpdateWindow()
        {
            InitializeComponent();
        }

        private void Update_Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.OK == MessageBox.Show("当前版本：1.0.0.1，将升级到1.0.0.2，是否升级？", "升级提示",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning))
            {
                AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdate.xml");
            }
        }
    }
}