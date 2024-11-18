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
    // To be implemented in future POE parts
    internal class RequestStatusViewModel : ViewModelBase
    {
        public NavigationStore ServiceRequestNavigationStore { get; }

        public ViewModelBase CurrentViewModel => ServiceRequestNavigationStore.CurrentViewModel;

        private Visibility _recentRequestVisibility = Visibility.Hidden;
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

        public ReportViewModel RecentRequest { get; private set; }

        public NavCommand RecentRequestNavCommand { get; private set; }

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

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
