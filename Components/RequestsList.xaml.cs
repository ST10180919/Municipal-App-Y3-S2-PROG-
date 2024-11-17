using Municipal_App.Services;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Municipal_App.Components
{
    /// <summary>
    /// Interaction logic for RequestsList.xaml
    /// </summary>
    public partial class RequestsList : UserControl
    {
        public ObservableCollection<ReportViewModel> RequestList;

        public static readonly DependencyProperty NavigationStoreProperty =
        DependencyProperty.Register(
            nameof(NavigationStore), // Property name
            typeof(NavigationStore), // Property type
            typeof(RequestsList), // Owner class
            new PropertyMetadata(null) // Default value
        );

        public NavigationStore NavigationStore
        {
            get => (NavigationStore)GetValue(NavigationStoreProperty);
            set => SetValue(NavigationStoreProperty, value);
        }

        public RequestsList()
        {
            InitializeComponent();

            // Adding reports to the observable list
            RequestList = new ObservableCollection<ReportViewModel>();
            foreach (var Report in AppStore.Instance.IssueReportStore.IssueReportList)
            {
                RequestList.Add(new ReportViewModel(Report));
            }

            MyList.ItemsSource = RequestList;
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Navigating to the RequestDetailsView (passing the ServiceRequest's details)
            var border = sender as Border;
            var reportViewModel = border?.DataContext as ReportViewModel;
            if (reportViewModel != null) 
            {
                var navigationService = new NavigationService(this.NavigationStore, () => { return new RequestDetailsViewModel(reportViewModel); });
                navigationService.Navigate();
            }
        }
    }
}
