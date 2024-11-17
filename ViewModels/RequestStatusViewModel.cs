using Municipal_App.Commands;
using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Municipal_App.ViewModels
{
    // To be implemented in future POE parts
    internal class RequestStatusViewModel : ViewModelBase
    {
        public NavigationStore ServiceRequestNavigationStore { get; }

        public ViewModelBase CurrentViewModel => ServiceRequestNavigationStore.CurrentViewModel;

        public RequestStatusViewModel()
        {
            // Setting up navigation between the search subpage and the details subpage
            this.ServiceRequestNavigationStore = new NavigationStore();
            this.ServiceRequestNavigationStore.CurrentViewModel = new SearchRequestViewModel();

            // Subscribing to the CurrentViewModelChanged event (ensures that the UI updates when view model changes)
            this.ServiceRequestNavigationStore.CurrentViewModelChanged += this.OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
