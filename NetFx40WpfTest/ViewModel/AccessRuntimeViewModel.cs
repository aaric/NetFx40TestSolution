using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using NLog;

namespace NetFx40WpfTest.ViewModel
{
    public class AccessRuntimeViewModel : ViewModelBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand<string> DefaultCommand { get; set; }

        public AccessRuntimeViewModel()
        {
            DefaultCommand = new RelayCommand<string>(DefaultAction);
        }

        private string _accessFilePath = "No File!";

        public string AccessFilePath
        {
            get { return _accessFilePath; }
            set
            {
                _accessFilePath = value;
                RaisePropertyChanged(() => AccessFilePath);
            }
        }

        private void DefaultAction(string cmd)
        {
            switch (cmd)
            {
                case "select":
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        Filter = "全部文件 (*.*)|*.*"
                    };
                    if (true == openFileDialog.ShowDialog())
                    {
                        AccessFilePath = openFileDialog.FileName;
                    }

                    break;
                case "test":
                    if (!"No File!".Equals(AccessFilePath))
                    {
                        // connect
                        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AccessFilePath;
                        OleDbConnection connection = new OleDbConnection(connectionString);
                        connection.Open();

                        // query
                        string content = "";
                        if (ConnectionState.Open == connection.State)
                        {
                            // select
                            string sql = @"SELECT xh, symc FROM syk";
                            DataSet dataSet = new DataSet();
                            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                            dataAdapter.Fill(dataSet);

                            Log.Info("-- Data --");
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                Log.Info("{} - {}", row["xh"], row["symc"]);
                                content += string.Format("{0} - {1}\n", row["xh"], row["symc"]);
                            }

                            // close
                            connection.Close();
                        }

                        MessageBox.Show("-- Data --\n" + content);
                    }

                    break;
            }
        }
    }
}