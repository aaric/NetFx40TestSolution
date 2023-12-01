using System.Reflection;
using System.Windows;
using AutoUpdaterDotNET;
using Newtonsoft.Json;
using NLog;

namespace NetFx40WpfTest.View
{
    /// <summary>
    /// AutoUpdateWindow.xaml 的交互逻辑
    ///
    /// <see cref="https://github.com/ravibpatel/AutoUpdater.NET/tree/v1.6.4"/>
    /// <code>Install-Package Autoupdater.NET.Official -Version 1.6.4</code>
    /// </summary>
    public partial class AutoUpdaterWindow : Window
    {
        // 日志
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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

            if (App.IsNetworkAvailable)
            {
                AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdaterTest.xml");
            }
            else
            {
                Log.Error(App.IsNetworkAvailableErrorMsg);
            }
        }

        private void Update_JsonConfig_Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (App.IsNetworkAvailable)
            {
                AutoUpdater.ParseUpdateInfoEvent += JsonConfigParseUpdateInfoEvent;
                AutoUpdater.Start("http://10.0.11.25:8021/vs2013/test/AutoUpdaterTest.json");
            }
            else
            {
                Log.Error(App.IsNetworkAvailableErrorMsg);
            }
        }

        private void JsonConfigParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            string baseUri = "http://10.0.11.25:8021";
            dynamic jsonConfig = JsonConvert.DeserializeObject(args.RemoteData);

            if (null != jsonConfig && bool.Parse(jsonConfig.mandatory.value.ToString()))
            {
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.ShowRemindLaterButton = false;
            }

            if (null != jsonConfig)
            {
                args.UpdateInfo = new UpdateInfoEventArgs
                {
                    CurrentVersion = jsonConfig.version,
                    ChangelogURL = baseUri + jsonConfig.changelog,
                    DownloadURL = baseUri + jsonConfig.url,
                    Mandatory = new Mandatory
                    {
                        Value = jsonConfig.mandatory.value,
                        UpdateMode = jsonConfig.mandatory.mode,
                        MinimumVersion = jsonConfig.mandatory.minVersion
                    },
                    CheckSum = new CheckSum
                    {
                        Value = jsonConfig.checksum.value,
                        HashingAlgorithm = jsonConfig.checksum.hashingAlgorithm
                    }
                };
            }
        }
    }
}