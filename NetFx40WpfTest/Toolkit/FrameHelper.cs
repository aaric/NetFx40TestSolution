using System.Windows;
using System.Windows.Controls;

namespace NetFx40WpfTest.Toolkit
{
    /// <summary>
    /// Frame 绑定工具类
    /// </summary>
    public class FrameHelper
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached(
                "Source",
                typeof(object),
                typeof(FrameHelper),
                new FrameworkPropertyMetadata(null, OnSourceChanged)
            );

        public static object GetSource(DependencyObject obj)
        {
            return obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, object value)
        {
            obj.SetValue(SourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Frame frame)
            {
                frame.Navigate(e.NewValue);
            }
        }
    }
}