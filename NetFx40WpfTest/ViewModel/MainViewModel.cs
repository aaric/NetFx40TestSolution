using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace NetFx40WpfTest.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<string> DefaultCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            DefaultCommand = new RelayCommand<string>(DefaultAction);

            FrameSource = new Uri("View/Sub/Page01.xaml", UriKind.Relative);
        }

        private Uri _frameSource;

        public Uri FrameSource
        {
            get { return _frameSource; }
            set
            {
                _frameSource = value;
                RaisePropertyChanged(() => FrameSource);
            }
        }

        private void DefaultAction(string cmd)
        {
            switch (cmd)
            {
                case "prev":
                    FrameSource = new Uri("View/Sub/Page01.xaml", UriKind.Relative);
                    break;
                case "next":
                    FrameSource = new Uri("View/Sub/Page02.xaml", UriKind.Relative);
                    break;
            }
        }
    }
}