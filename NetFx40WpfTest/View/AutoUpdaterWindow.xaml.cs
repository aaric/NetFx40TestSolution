using System.Reflection;
using System.Windows;
using AutoUpdaterDotNET;
using Newtonsoft.Json;

namespace NetFx40WpfTest.View
{
    /// <summary>
    /// AutoUpdateWindow.xaml 的交互逻辑
    ///
    /// <see cref="https://github.com/ravibpatel/AutoUpdater.NET/tree/v1.6.4"/>
    /// <code>Install-Package Autoupdater.NET.Official -Version 1.6.4</code>
    ///
    /// <seealso cref="https://www.nuget.org/packages/Autoupdater.NET.SelfDriven"/>
    /// <seealso cref="https://www.nuget.org/packages/Best.Client.AutoUpdate"/>
    /// <seealso cref="https://www.nuget.org/packages/Zl.AutoUpgrade.Core"/>
    /// </summary>
    public partial class AutoUpdaterWindow : Window
    {
        public AutoUpdaterWindow()
        {
            InitializeComponent();

            // 设置版本
            // VersionLabel.Content = "1.0.2.4";
            Assembly assembly = Assembly.GetExecutingAssembly();
            VersionLabel.Content = assembly.GetName().Version;
        }

        private void Update_XmlConfig_Button_OnClick(object sender, RoutedEventArgs e)
        {
            /*if (MessageBoxResult.OK == MessageBox.Show("当前版本：1.0.0.1，将升级到1.0.2.4，是否升级？", "升级提示",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning))
            {
                AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdate.xml", Assembly.GetEntryAssembly());
            }*/

            // AutoUpdater.AppTitle = Assembly.GetEntryAssembly().GetName().Name;

            AutoUpdater.ShowSkipButton = false;
            // AutoUpdater.LetUserSelectRemindLater = false;
            // AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
            // AutoUpdater.RemindLaterAt = 2;
            AutoUpdater.ShowRemindLaterButton = false;

            // AutoUpdater.Mandatory = true;
            // AutoUpdater.UpdateMode = Mode.Forced;
            // AutoUpdater.OpenDownloadPage = true;
            // AutoUpdater.DownloadPath = Environment.CurrentDirectory;

            // AutoUpdater.RunUpdateAsAdmin = false;

            AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdaterTest.xml");
        }

        private void Update_JsonConfig_Button_OnClick(object sender, RoutedEventArgs e)
        {
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;

            AutoUpdater.ParseUpdateInfoEvent += JsonConfigParseUpdateInfoEvent;
            AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdaterTest.json");
        }

        private void JsonConfigParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            string baseUri = "http://10.0.11.25:8021";
            dynamic configJson = JsonConvert.DeserializeObject(args.RemoteData);
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = configJson.version,
                ChangelogURL = configJson.changelog,
                DownloadURL = configJson.url,
                Mandatory = new Mandatory
                {
                    Value = configJson.mandatory.value,
                    UpdateMode = configJson.mandatory.mode,
                    MinimumVersion = configJson.mandatory.minVersion
                },
                CheckSum = new CheckSum
                {
                    Value = configJson.checksum.value,
                    HashingAlgorithm = configJson.checksum.hashingAlgorithm
                }
            };
        }
    }
}