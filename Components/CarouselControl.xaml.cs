using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
namespace Municipal_App.Components
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// A user control representing a carousel for displaying items with horizontal scrolling.
    /// It provides functionality for filtering and recommending items.
    /// </summary>
    public partial class CarouselControl : UserControl
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Dependency property for binding the items source of the carousel.
        /// This allows data binding events or announcements to the carousel's ItemsControl.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CarouselControl), new PropertyMetadata(null));

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the items source for the carousel.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Dependency property for specifying a template key to be used for rendering 
        /// the items.
        /// 
        /// NOTE: I pass the template key  here because passing the template itself 
        /// created crazy slowdowns with loading events and announcements, not sure why.
        /// </summary>
        public static readonly DependencyProperty SelectedItemTemplateKeyProperty =
            DependencyProperty.Register("SelectedItemTemplateKey", typeof(string), typeof(CarouselControl), new PropertyMetadata(null));

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the selected data template based on the provided template key.
        /// The template is used to render each item in the carousel.
        /// </summary>
        public DataTemplate SelectedItemTemplate
        {
            get
            {
                return (DataTemplate)this.Resources[SelectedItemTemplateKey];
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the key for the selected item template, allowing different 
        /// templates to be applied.
        /// </summary>
        public string SelectedItemTemplateKey
        {
            get { return (string)GetValue(SelectedItemTemplateKeyProperty); }
            set { SetValue(SelectedItemTemplateKeyProperty, value); }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Calculates the width of the carousel content minus padding (80px).
        /// This value is used for adjusting how far the carousel scrolls based on
        /// the the width of the items shown
        /// </summary>
        private int CarouselContentWidth => CarouselGrid != null ? Convert.ToInt32(CarouselGrid.ActualWidth - 80) : 0;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Property to access the EventsStore instance from the AppStore, which manages
        /// the collection and filtering of events.
        /// </summary>
        private EventsStore EventsStore => AppStore.Instance.EventsStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Property to access the AnnouncementsStore instance from the AppStore, which manages
        /// the collection and filtering of announcements.
        /// </summary>
        private AnnouncementsStore AnnouncementsStore => AppStore.Instance.AnnouncementsStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes the carousel control and registers event handlers for filtering 
        /// and recommendations.
        /// </summary>
        public CarouselControl()
        {
            InitializeComponent();
            EventsStore.Filter.OnFilterEvents += this.ApplySearchFilter;
            AnnouncementsStore.Filter.OnFilterAnnouncements += this.ApplySearchFilter;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Filters the items in the carousel based on events or announcements.
        /// Sets recommendations and determines whether the item should be accepted.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The filter event arguments containing the item to filter.
        /// </param>
        public void OnFilterEvents(object sender, FilterEventArgs e)
        {
            try
            {
                if (e.Item != null && e.Item is EventViewModel)
                {
                    var viewModel = e.Item as EventViewModel;
                    // Setting recommendation
                    var isRecommended = EventsStore.RecommendationService.GetRecommendation(viewModel.Title, viewModel.Category, viewModel.Date);
                    viewModel.IsRecommended = isRecommended;
                    // Accepting or denying item
                    e.Accepted = EventsStore.Filter.IsAccepted(viewModel);

                }
                else if (e.Item != null && e.Item is AnnouncementViewModel)
                {
                    var viewModel = e.Item as AnnouncementViewModel;
                    // Setting recommendation
                    var isRecommended = AnnouncementsStore.RecommendationService.GetRecommendation(viewModel.Title, "", viewModel.Date);
                    viewModel.IsRecommended = isRecommended;
                    // Accepting or denying item
                    e.Accepted = AnnouncementsStore.Filter.IsAccepted(viewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Applies the search filter and refreshes the view by updating the filtered 
        /// items source.This method is invoked whenever the filter criteria change.
        /// </summary>
        public void ApplySearchFilter()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("Failed to apply search filter", ex);
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the left arrow click to animate horizontal scrolling to the left.
        /// </summary>
        private void LeftArrow_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation horizontalAnimation = new DoubleAnimation
            {
                To = scrollViewer.HorizontalOffset - this.CarouselContentWidth, // Scroll left by 120px (adjust as necessary)
                Duration = new Duration(TimeSpan.FromMilliseconds(600)) // Smooth transition of 300ms
            };
            scrollViewer.BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, horizontalAnimation);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the right arrow click to animate horizontal scrolling to the right.
        /// </summary>
        private void RightArrow_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation horizontalAnimation = new DoubleAnimation
            {
                To = scrollViewer.HorizontalOffset + this.CarouselContentWidth,
                Duration = new Duration(TimeSpan.FromMilliseconds(600))
            };
            scrollViewer.BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, horizontalAnimation);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles mouse wheel scrolling within the carousel. Propagates vertical 
        /// scroll events to a parent scroll viewer to allow the page to scroll.
        /// </summary>
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            // Check if scrolling Horizontally
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

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Finds the parent scroll viewer of the current control in the visual tree.
        /// This method is used to propagate vertical scrolling to the parent scroll viewer.
        /// </summary>
        /// <param name="child">The child element whose parent scroll viewer is to be 
        /// found.</param>
        /// <returns>The parent ScrollViewer, or null if not found.</returns>
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
    }

    //---------------------------------------------------------------------------------
    /// <summary>
    /// A behavior class that provides attached properties for controlling the 
    /// horizontal scroll offset of a ScrollViewer.
    /// This allows for smooth scrolling animations and programmatic control of 
    /// ScrollViewer offsets.
    /// </summary>
    public static class ScrollViewerBehavior
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// An attached dependency property for controlling the horizontal offset of a 
        /// ScrollViewer. Changes to this property trigger the ScrollToHorizontalOffset 
        /// behavior.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerBehavior),
                new PropertyMetadata(OnHorizontalOffsetChanged));

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the current horizontal offset value from the attached property on the 
        /// specified object.
        /// </summary>
        /// <param name="obj">The object (typically a ScrollViewer) from which to 
        /// retrieve the horizontal offset value.</param>
        /// <returns>The horizontal offset value as a double.</returns>
        public static double GetHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalOffsetProperty);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Sets the horizontal offset value on the attached property for the specified 
        /// object.
        /// This triggers the scrolling behavior when the property value is changed.
        /// </summary>
        /// <param name="obj">The object on which to set the 
        /// horizontal offset value.</param>
        /// <param name="value">The new horizontal offset value as a double.</param>
        public static void SetHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalOffsetProperty, value);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles changes to the HorizontalOffset property and scrolls the 
        /// ScrollViewer to the new offset.
        /// This method is called whenever the attached HorizontalOffsetProperty changes.
        /// </summary>
        /// <param name="d">The dependency object whose property changed</param>
        /// <param name="e">The event arguments containing the old and new values of 
        /// the property.</param>
        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer viewer)
            {
                viewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }
    }
}
//---------------------------------------EOF-------------------------------------------
