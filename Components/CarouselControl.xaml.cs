using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Municipal_App.Components
{
    /// <summary>
    /// Interaction logic for CarouselControl.xaml
    /// </summary>
    public partial class CarouselControl : UserControl
    {
        private int CarouselContentWidth => CarouselGrid != null ? Convert.ToInt32(CarouselGrid.ActualWidth - 80) : 0;

        public CarouselControl()
        {
            InitializeComponent();
        }

        private void LeftArrow_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation horizontalAnimation = new DoubleAnimation
            {
                To = scrollViewer.HorizontalOffset - this.CarouselContentWidth, // Scroll left by 120px (adjust as necessary)
                Duration = new Duration(TimeSpan.FromMilliseconds(600)) // Smooth transition of 300ms
            };
            scrollViewer.BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, horizontalAnimation);
        }

        private void RightArrow_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation horizontalAnimation = new DoubleAnimation
            {
                To = scrollViewer.HorizontalOffset + this.CarouselContentWidth, // Scroll right by 120px
                Duration = new Duration(TimeSpan.FromMilliseconds(600))
            };
            scrollViewer.BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, horizontalAnimation);
        }
    }

    // Behavior to enable animated scrolling
    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerBehavior),
                new PropertyMetadata(OnHorizontalOffsetChanged));

        public static double GetHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalOffsetProperty, value);
        }

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer viewer)
            {
                viewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }
    }

    public class Event
    {
        public string Title { get; set; }
        public string ImageSource { get; set; }
    }
}
