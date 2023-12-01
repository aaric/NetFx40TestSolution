﻿using System.Diagnostics;
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
            switch (cmd)
            {
                case "kill":
                    Process cp = Process.GetCurrentProcess();

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
            }
        }
    }
}