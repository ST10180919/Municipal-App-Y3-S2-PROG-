using Municipal_App.Commands;
using Municipal_App.Models;
using Municipal_App.Services;
using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// ViewModel for managing the status and navigation of service requests.
    /// </summary>
    internal class RequestStatusViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The navigation store used for managing the current subpage view model.
        /// </summary>
        public NavigationStore ServiceRequestNavigationStore { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The current view model displayed in the service request navigation store.
        /// </summary>
        public ViewModelBase CurrentViewModel => ServiceRequestNavigationStore.CurrentViewModel;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The visibility status of the most recent service request section.
        /// </summary>
        private Visibility _recentRequestVisibility = Visibility.Hidden;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the visibility of the most recent service request section.
        /// </summary>
        public Visibility RecentRequestVisibility
        {
            get
            {
                return _recentRequestVisibility;
            }
            set
            {
                _recentRequestVisibility = value;
                OnPropertyChanged(nameof(RecentRequestVisibility));
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The most recent service request.
        /// </summary>
        public ReportViewModel RecentRequest { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command for navigating to the details page of the most recent service request.
        /// </summary>
        public NavCommand RecentRequestNavCommand { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestStatusViewModel"/> class.
        /// </summary>
        public RequestStatusViewModel()
        {
            // Setting up navigation between the search subpage and the details subpage
            this.ServiceRequestNavigationStore = new NavigationStore();
            this.ServiceRequestNavigationStore.CurrentViewModel = new SearchRequestViewModel();

            // Subscribing to the CurrentViewModelChanged event (ensures that the UI updates when view model changes)
            this.ServiceRequestNavigationStore.CurrentViewModelChanged += this.OnCurrentViewModelChanged;

            // Recent Request Setup
            this.SetupRecentRequest();
            AppStore.Instance.IssueReportStore.ReportsLoaded += this.SetupRecentRequest;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Sets up the most recent service request by retrieving it from the issue report store.
        /// </summary>
        private void SetupRecentRequest()
        {
            // Recent Request Setup
            var heapItem = AppStore.Instance.IssueReportStore.GetMostRecentReport();

            if (heapItem != null && heapItem.Data != null)
            {
                this.RecentRequest = new ReportViewModel(heapItem.Data);
                OnPropertyChanged(nameof(RecentRequest));

                this.RecentRequestNavCommand = new NavCommand(
                    new NavigationService(this.ServiceRequestNavigationStore,
                    () => { return new RequestDetailsViewModel(RecentRequest, this.ServiceRequestNavigationStore); }));
                OnPropertyChanged(nameof(RecentRequestNavCommand));

                this.RecentRequestVisibility = Visibility.Visible;
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the <see cref="NavigationStore.CurrentViewModelChanged"/> event to notify the UI of changes.
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
//---------------------------------------EOF-------------------------------------------