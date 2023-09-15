using System.Windows.Controls;
using NetFx40WpfTest.ViewModel.Main;

namespace NetFx40WpfTest.View.Main
{
    /// <summary>
    /// Page01.xaml 的交互逻辑
    /// </summary>
    public partial class FirstPage : Page
    {
        public FirstPage()
        {
            InitializeComponent();

            if (DataContext is FirstViewModel viewModel)
            {
                viewModel.ThisPage = this;
            }
        }
    }
}