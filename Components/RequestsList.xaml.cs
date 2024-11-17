using Municipal_App.Services;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Municipal_App.Components
{
    /// <summary>
    /// Interaction logic for RequestsList.xaml
    /// </summary>
    public partial class RequestsList : UserControl
    {
        public ObservableCollection<ReportViewModel> RequestList;

        private IssueCategory SelectedCategory = IssueCategory.None;

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
            foreach (var Report in AppStore.Instance.IssueReportStore.GetAllIssueReports())
            {
                RequestList.Add(new ReportViewModel(Report));
            }

            MyList.ItemsSource = RequestList;

            // Subscribing to the SearchTermChangedEvent ensuring that the list is filtered when the user searches
            AppStore.Instance.IssueReportStore.SearchTermChanged += UpdateList;
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Navigating to the RequestDetailsView (passing the ServiceRequest's details)
            var border = sender as Border;
            var reportViewModel = border?.DataContext as ReportViewModel;
            if (reportViewModel != null) 
            {
                var navigationService = new NavigationService(this.NavigationStore, () => { return new RequestDetailsViewModel(reportViewModel, this.NavigationStore); });
                navigationService.Navigate();
            }
        }

        private void UpdateList()
        {
            // Clearing the list
            this.RequestList.Clear();

            var newList = new List<ISSUE_REPORT>();

            if (this.SelectedCategory == IssueCategory.None)
            {
                // Apply search
                newList = AppStore.Instance.IssueReportStore.Search();
            }
            else
            {
                // Apply search and filter
                var filterTerm = SelectedCategory.ToString();
                newList = AppStore.Instance.IssueReportStore.FilterAndSearch(filterTerm);
            }

            foreach (var report in newList)
            {
                this.RequestList.Add(new ReportViewModel(report));
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Clear borders on all radio buttons
            RoadsRadioButton.IsChecked = false;
            SanitationRadioButton.IsChecked = false;
            UtilitiesRadioButton.IsChecked = false;

            // Set gray border on the selected radio button
            RadioButton selectedRadioButton = sender as RadioButton;
            if (selectedRadioButton != null)
            {
                selectedRadioButton.IsChecked = true;

                // Update selected category
                 Enum.TryParse(selectedRadioButton.Content.ToString(), out SelectedCategory);

                // UpdateList
                this.UpdateList();
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            // Clear borders on all radio buttons
            RoadsRadioButton.IsChecked = false;
            SanitationRadioButton.IsChecked = false;
            UtilitiesRadioButton.IsChecked = false;

            this.SelectedCategory = IssueCategory.None;
            this.UpdateList();
        }
    }
}
