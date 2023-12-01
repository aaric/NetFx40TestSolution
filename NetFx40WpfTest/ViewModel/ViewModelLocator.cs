/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:NetFx40WpfTest"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using NetFx40WpfTest.ViewModel.Main;

namespace NetFx40WpfTest.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<HandyDemoViewModel>();
            SimpleIoc.Default.Register<FirstViewModel>();
            SimpleIoc.Default.Register<MqttNet40ViewModel>();
            SimpleIoc.Default.Register<M2MqttViewModel>();
            SimpleIoc.Default.Register<AccessRuntimeViewModel>();
            SimpleIoc.Default.Register<NetworkChangeViewModel>();
            SimpleIoc.Default.Register<ProcessViewModel>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public FirstViewModel MainFirst
        {
            get { return ServiceLocator.Current.GetInstance<FirstViewModel>(); }
        }

        public HandyDemoViewModel HandyDemo
        {
            get { return ServiceLocator.Current.GetInstance<HandyDemoViewModel>(); }
        }

        public MqttNet40ViewModel MqttNet40
        {
            get { return ServiceLocator.Current.GetInstance<MqttNet40ViewModel>(); }
        }

        public M2MqttViewModel M2Mqtt
        {
            get { return ServiceLocator.Current.GetInstance<M2MqttViewModel>(); }
        }

        public AccessRuntimeViewModel AccessRuntime
        {
            get { return ServiceLocator.Current.GetInstance<AccessRuntimeViewModel>(); }
        }

        public NetworkChangeViewModel NetworkChange
        {
            get { return ServiceLocator.Current.GetInstance<NetworkChangeViewModel>(); }
        }

        public ProcessViewModel Process
        {
            get { return ServiceLocator.Current.GetInstance<ProcessViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}