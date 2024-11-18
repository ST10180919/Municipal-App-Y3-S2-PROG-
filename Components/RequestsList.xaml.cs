using Municipal_App.Models;
using Municipal_App.Services;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Municipal_App.Components
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents the RequestsList component that displays and manages a list of issue reports.
    /// </summary>
    public partial class RequestsList : UserControl
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Collection of reports to display in the list.
        /// </summary>
        public ObservableCollection<ReportViewModel> RequestList;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The currently selected issue category for filtering.
        /// </summary>
        private IssueCategory SelectedCategory = IssueCategory.None;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Dependency property for binding a NavigationStore instance to the control.
        /// </summary>
        public static readonly DependencyProperty NavigationStoreProperty =
        DependencyProperty.Register(
            nameof(NavigationStore), // Property name
            typeof(NavigationStore), // Property type
            typeof(RequestsList), // Owner class
            new PropertyMetadata(null) // Default value
        );

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the navigation store used for managing navigation within the application.
        /// </summary>
        public NavigationStore NavigationStore
        {
            get => (NavigationStore)GetValue(NavigationStoreProperty);
            set => SetValue(NavigationStoreProperty, value);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestsList"/> class and sets up the initial state.
        /// </summary>
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

            // Enabling Lazy loading of Report Issues
            AppStore.Instance.IssueReportStore.ReportsLoaded += UpdateList;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the mouse down event on a list view item to navigate to the RequestDetailsView.
        /// </summary>
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

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Updates the list of displayed reports based on the current search term and selected category.
        /// </summary>
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

            // Force a layout update to ensure UI refresh
            MyList.UpdateLayout();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the Checked event for radio buttons, updating the selected category and refreshing the list.
        /// </summary>
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

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the ClearFilter button click event, resetting the selected category and refreshing the list.
        /// </summary>
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
//---------------------------------------EOF-------------------------------------------