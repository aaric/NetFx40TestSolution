using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NetFx40WpfTest.Toolkit;
using NLog;

namespace NetFx40WpfTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        // 日志
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // 启动应用程序互斥量
        private static Mutex _mutex;

        // 启动应用时检查网络，同时监听网络环境
        public static bool IsNetworkAvailable = NetworkHelper.IsNetworkAvailable();

        // 监听网络环境异常提示
        public static string IsNetworkAvailableErrorMsg = "网络不可用，请检查你的网络设置";

        protected override void OnStartup(StartupEventArgs e)
        {
            // 仅支持应用程序单列运行
            bool flag;
            _mutex = new Mutex(true, "NetFx40WpfTest", out flag);
            if (!flag)
            {
                MessageBox.Show("应用程序已运行！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                Environment.Exit(0);
            }

            // UI异常处理
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // 非UI异常处理
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // 线程异常处理
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // 网络环境异常处理
            // https://learn.microsoft.com/zh-cn/dotnet/api/system.net.networkinformation.networkchange?view=netframework-4.0
            NetworkChange.NetworkAddressChanged += NetworkChange_OnNetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_OnNetworkAvailabilityChanged;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = ExceptionHelper.Process(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                ExceptionHelper.Process(e.ExceptionObject as Exception);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ExceptionHelper.Process(e.Exception);
        }

        private void ShowNetworkStateLog(bool state)
        {
            bool isLog = false;
            if (state != IsNetworkAvailable)
            {
                IsNetworkAvailable = state;
                isLog = true;
            }

            if (isLog)
            {
                if (state)
                {
                    Log.Info("网络已连接！");
                }
                else
                {
                    Log.Error("网络已断开！");
                }
            }
        }

        private void NetworkChange_OnNetworkAddressChanged(object sender, EventArgs e)
        {
            /*NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                Log.Debug("NetworkChange_OnNetworkAddressChanged -> name={}, status={}", adapter.Name,
                    adapter.OperationalStatus);
            }*/
            ShowNetworkStateLog(NetworkHelper.IsNetworkAvailable() || NetworkHelper.IsPingOk("www.baidu.com"));
        }

        private void NetworkChange_OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            ShowNetworkStateLog(e.IsAvailable);
        }
    }
}