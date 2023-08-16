using System.Reflection;
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

            // 设置版本
            // VersionLabel.Content = "1.0.2.4";
            VersionLabel.Content = Assembly.GetEntryAssembly().GetName().Version;
        }

        private void UpdateJson_Button_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Current Version: " + VersionLabel.Content);
        }

        private void UpdateXml_Button_OnClick(object sender, RoutedEventArgs e)
        {
            /*if (MessageBoxResult.OK == MessageBox.Show("当前版本：1.0.0.1，将升级到1.0.0.2，是否升级？", "升级提示",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning))
            {
                AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdate.xml");
            }*/

            // AutoUpdater.AppTitle = Assembly.GetEntryAssembly().GetName().Name;

            AutoUpdater.ShowSkipButton = false;
            // AutoUpdater.LetUserSelectRemindLater = false;
            // AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
            // AutoUpdater.RemindLaterAt = 2;
            AutoUpdater.ShowRemindLaterButton = false;

            // AutoUpdater.Mandatory = true;
            // AutoUpdater.UpdateMode = Mode.Forced;

            // AutoUpdater.RunUpdateAsAdmin = false;
            // AutoUpdater.OpenDownloadPage = true;
            // AutoUpdater.DownloadPath = Environment.CurrentDirectory;

            // AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdate.xml", Assembly.GetEntryAssembly());
            AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdate.xml");
        }
    }
}