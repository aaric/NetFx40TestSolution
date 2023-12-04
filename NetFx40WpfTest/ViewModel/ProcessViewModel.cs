using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NLog;

namespace NetFx40WpfTest.ViewModel
{
    public class ProcessViewModel : ViewModelBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand<string> DefaultCommand { get; set; }

        public ProcessViewModel()
        {
            DefaultCommand = new RelayCommand<string>(DefaultAction);
        }

        private void DefaultAction(string cmd)
        {
            Process cp = Process.GetCurrentProcess();

            switch (cmd)
            {
                case "kill":
                    foreach (Process p in Process.GetProcesses())
                    {
                        if (p.ProcessName.Equals("NetFx40WpfTest") && p.Id != cp.Id)
                        {
                            Log.Info("[{}] taskkill /f /im /{}.exe", p.Id, p.ProcessName);
                            p.Kill();
                            p.WaitForExit();
                        }
                    }

                    Log.Info("[{}] taskkill /f /im /{}.exe", cp.Id, cp.ProcessName);
                    cp.Kill();
                    cp.WaitForExit();

                    break;
                case "restart":
                    foreach (Process p in Process.GetProcesses())
                    {
                        if (p.ProcessName.Equals("NetFx40WpfTest") && p.Id != cp.Id)
                        {
                            Log.Info("[{}] taskkill /f /im /{}.exe", p.Id, p.ProcessName);
                            p.Kill();
                            p.WaitForExit();
                        }
                    }

                    string appFileName = Application.ResourceAssembly.Location;
                    Log.Info("[{}] start {}", cp.Id, appFileName);
                    Process.Start(appFileName);

                    Log.Info("[{}] taskkill /f /im /{}.exe", cp.Id, cp.ProcessName);
                    cp.Kill();
                    cp.WaitForExit();

                    break;
            }
        }
    }
}