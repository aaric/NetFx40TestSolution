using System;
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
        protected override void OnStartup(StartupEventArgs e)
        {
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