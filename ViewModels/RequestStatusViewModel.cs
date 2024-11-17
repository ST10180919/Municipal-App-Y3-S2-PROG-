using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.ViewModels
{
    // To be implemented in future POE parts
    internal class RequestStatusViewModel : ViewModelBase
    {
        public NavigationStore ServiceRequestNavigationStore { get; set; }

        public ViewModelBase CurrentViewModel => ServiceRequestNavigationStore.CurrentViewModel;

        public RequestStatusViewModel()
        {
            // Setting up navigation between the search subpage and the details subpage
            this.ServiceRequestNavigationStore = new NavigationStore();
            this.ServiceRequestNavigationStore.CurrentViewModel = new SearchRequestViewModel();
        }
    }
}
