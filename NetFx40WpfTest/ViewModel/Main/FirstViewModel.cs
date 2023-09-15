using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace NetFx40WpfTest.ViewModel.Main
{
    public class FirstViewModel : ViewModelBase
    {
        public FirstViewModel()
        {
            DataList = new ObservableCollection<DataVo>();
            for (int i = 0; i < 100; i++)
            {
                DataList.Add(new DataVo()
                {
                    Id = (i + 1).ToString(),
                    Name = string.Format("name-{0}", i),
                    Title = string.Format("title-{0}", i)
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


        public class DataVo : ObservableObject
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
        }
    }
}