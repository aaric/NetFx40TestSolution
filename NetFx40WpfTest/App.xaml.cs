using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NetFx40WpfTest.Toolkit;

namespace NetFx40WpfTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        // 设置应用程序单列运行
        private static Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            // 单列运行逻辑
            _mutex = new Mutex(true, "NetFx40WpfTest", out bool flag);
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
    }
}