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
    internal class RequestDetailsViewModel : ViewModelBase
    {
        public ReportViewModel ServiceRequest { get; }

        public NavCommand SearchRequestNavCommand { get; }
        public RequestDetailsViewModel(ReportViewModel serviceRequest, NavigationStore navigationStore)
        {
            this.ServiceRequest = serviceRequest;
            this.SearchRequestNavCommand = new NavCommand(new Services.NavigationService(navigationStore, () => { return new SearchRequestViewModel(); }));
        }
    }
}
