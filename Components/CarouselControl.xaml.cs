using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.Collections;
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

        // DependencyProperty for ItemsSource
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CarouselControl), new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // DependencyProperty to accept the template key from the ViewModel or View
        public static readonly DependencyProperty SelectedItemTemplateKeyProperty =
            DependencyProperty.Register("SelectedItemTemplateKey", typeof(string), typeof(CarouselControl), new PropertyMetadata(null, OnSelectedItemTemplateKeyChanged));

        // Property to get the selected DataTemplate based on the key
        public DataTemplate SelectedItemTemplate
        {
            get
            {
                return (DataTemplate)this.Resources[SelectedItemTemplateKey];
            }
        }

        // This callback gets triggered when the SelectedItemTemplateKey changes
        private static void OnSelectedItemTemplateKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CarouselControl;
            if (control != null)
            {
                // Refresh or update the ItemTemplate for the ItemsControl
                control.OnSelectedItemTemplateChanged();
            }
        }

        private void OnSelectedItemTemplateChanged()
        {
            // This method can be used to force UI updates if necessary
        }

        public string SelectedItemTemplateKey
        {
            get { return (string)GetValue(SelectedItemTemplateKeyProperty); }
            set { SetValue(SelectedItemTemplateKeyProperty, value); }
        }

        public CarouselControl()
        {
            InitializeComponent();
            AppStore.Instance.EventsStore.FilterStore.OnFilterEvents += this.ApplyFilter;
        }

        public void OnFilterEvents(object sender, FilterEventArgs e)
        {
            if (e.Item != null && e.Item is MunicipalEventViewModel)
            {
                var eventItem = e.Item as MunicipalEventViewModel;
                var searchText = AppStore.Instance.EventsStore.FilterStore.SearchText;

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Only include items that contain the search text in their title
                    e.Accepted = eventItem.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                else
                {
                    e.Accepted = true; // Include all items if there's no search text
                }
            } else if (e.Item != null && e.Item is AnnouncementViewModel)
            {
                var eventItem = e.Item as AnnouncementViewModel;
                var searchText = AppStore.Instance.AnnouncementsStore.FilterStore.SearchText;

                if (!string.IsNullOrEmpty(searchText))
                {
                    // Only include items that contain the search text in their title
                    e.Accepted = eventItem.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                else
                {
                    e.Accepted = true; // Include all items if there's no search text
                }
            }
        }

        // Method to update the search string and refresh the view
        //public void UpdateFilter(string searchText)
        //{
        //    _searchText = searchText;  // Update the search string

        //    // Refresh the filter to apply the new search string
        //    var collectionViewSource = (CollectionViewSource)FindResource("FilteredEvents");
        //    collectionViewSource.View.Refresh();
        //}

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

        private void AdjustLatoLetterSpacing(string text, double spacing, TextBlock textBlock)
        {
            textBlock.Inlines.Clear();  // Clear any existing text in the TextBlock

            // Loop through each character in the string
            for (int i = 0; i < text.Length; i++)
            {
                // Create a new Run for each character
                Run run = new Run(text[i].ToString());

                // Add the character to the TextBlock
                textBlock.Inlines.Add(run);

                // If it's not the last character, add a space Run to simulate spacing
                if (i < text.Length - 1)
                {
                    // Add spacing by adding a space character with adjusted FontSize or Width
                    Run space = new Run(" ");
                    space.FontSize = textBlock.FontSize + spacing;  // Increase space size
                    textBlock.Inlines.Add(space);
                }
            }
        }

        private void LatoText_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            AdjustLatoLetterSpacing(textBlock.Text, 7, textBlock);
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            // Check if we are scrolling horizontally
            if (scrollViewer != null && scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
            {
                if (e.Delta < 0)
                {
                    scrollViewer.LineRight();
                }
                else
                {
                    scrollViewer.LineLeft();
                }

                // If you want to scroll horizontally within the carousel and NOT scroll the page, set e.Handled to true.
                // But in this case, we're allowing vertical scrolling, so we don't handle the event here.
                e.Handled = false;
            }

            // Allow vertical scrolling to propagate to parent ScrollViewer
            if (e.Delta != 0 && !e.Handled)
            {
                // Get the parent scroll viewer and pass the scroll event to it
                var parentScrollViewer = FindParentScrollViewer(scrollViewer);
                if (parentScrollViewer != null)
                {
                    parentScrollViewer.ScrollToVerticalOffset(parentScrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;  // Ensure the event is handled so it doesn't bubble further
                }
            }
        }

        // Helper method to find the parent ScrollViewer
        private ScrollViewer FindParentScrollViewer(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is ScrollViewer parentScrollViewer)
                {
                    return parentScrollViewer;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public void ApplyFilter()
        {
            var collectionViewSource = (CollectionViewSource)this.FindResource("SortedItems");
            if (collectionViewSource != null)
            {
                // Refresh the view to reapply the filter
                collectionViewSource.View.Refresh();

                // Set the filtered view as the ItemsSource for the ItemsControl
                carouselItems.ItemsSource = collectionViewSource.View;

                // Force a layout update to ensure UI refresh
                carouselItems.UpdateLayout();
            }
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

}
