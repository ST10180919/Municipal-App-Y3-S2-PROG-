using Municipal_App.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Municipal_App.Views
{
    /// <summary>
    /// Interaction logic for BannerMessageView.xaml
    /// </summary>
    public partial class BannerMessageView : UserControl
    {
        public BannerMessageView()
        {
            InitializeComponent();
            this.Loaded += BannerMessageView_Loaded;
        }

        // Event handler for when the UserControl is loaded
        private void BannerMessageView_Loaded(object sender, RoutedEventArgs e)
        {
            // Assuming the ViewModel is the DataContext and it implements INotifyPropertyChanged
            if (this.DataContext is INotifyPropertyChanged viewModel)
            {
                // Subscribe to the PropertyChanged event of the ViewModel
                viewModel.PropertyChanged += OnHasMessageChanged;
            }
        }

        // This method gets called whenever a property in the ViewModel changes
        private void OnHasMessageChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the property that changed is "HasMessage"
            if (e.PropertyName == "HasMessage")
            {
                var viewModel = (BannerMessageViewModel)sender;

                // Use the Dispatcher to run the animation code on the UI thread
                Dispatcher.Invoke(() =>
                {
                    if (viewModel.HasMessage)
                    {
                        // Trigger expand animation when HasMessage is true
                        ShowBanner();
                    }
                    else
                    {
                        // Trigger collapse animation when HasMessage is false
                        HideBanner();
                    }
                });
            }
        }

        // Method to show the banner by triggering the expand animation
        private void ShowBanner()
        {
            BannerGrid.Visibility = Visibility.Visible;

            var expandStoryboard = (Storyboard)this.FindResource("ExpandBanner");
            expandStoryboard.Begin(); // Start the expand animation
        }

        // Method to hide the banner by triggering the collapse animation
        private void HideBanner()
        {
            var collapseStoryboard = (Storyboard)this.FindResource("CollapseBanner");
            collapseStoryboard.Begin();
        }
    }
}
