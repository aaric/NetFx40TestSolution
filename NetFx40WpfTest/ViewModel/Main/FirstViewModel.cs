using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NetFx40WpfTest.View.Main;
using NLog;

namespace NetFx40WpfTest.ViewModel.Main
{
    public class FirstViewModel : ViewModelBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RelayCommand<string> DefaultCommand { get; set; }

        public FirstPage ThisPage { get; set; }

        public FirstViewModel()
        {
            DefaultCommand = new RelayCommand<string>(DefaultAction);

            DataList = new ObservableCollection<DataVo>();
            for (int i = 0; i < 20; i++)
            {
                int no = i + 1;
                DataList.Add(new DataVo()
                {
                    Id = no.ToString(),
                    Name = string.Format("name-{0}", no),
                    Title = string.Format("title-{0}", no)
                });
            }
        }

        private ObservableCollection<DataVo> _dataList;

        public ObservableCollection<DataVo> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                RaisePropertyChanged(() => DataList);
            }
        }

        [Obsolete]
        private Page FindParentPage(object context)
        {
            DependencyObject parent = context as DependencyObject;

            while (parent != null && !(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as Page;
        }

        private void DefaultAction(string cmd)
        {
            switch (cmd)
            {
                case "add":
                    int no = DataList.Count + 1;
                    DataVo dataVo = new DataVo()
                    {
                        Id = no.ToString(),
                        Name = string.Format("name-{0}", no),
                        Title = string.Format("title-{0}", no)
                    };
                    DataList.Add(dataVo);
                    ThisPage.MyDataGrid.ScrollIntoView(dataVo);
                    break;
                case "test":
                    Page thisPage = ThisPage;
                    if (null != thisPage)
                    {
                        MessageBox.Show(thisPage.Title);
                    }
                    else
                    {
                        MessageBox.Show("error");
                    }

                    break;
            }
        }

        public class DataVo : ObservableObject
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
        }
    }
}