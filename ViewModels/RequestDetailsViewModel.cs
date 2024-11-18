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
    //---------------------------------------------------------------------------------
    /// <summary>
    /// ViewModel for displaying the details of a specific service request.
    /// </summary>
    internal class RequestDetailsViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The service request being displayed in this ViewModel.
        /// </summary>
        public ReportViewModel ServiceRequest { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command for navigating to the SearchRequest view.
        /// </summary>
        public NavCommand SearchRequestNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestDetailsViewModel"/> class.
        /// </summary>
        /// <param name="serviceRequest">The service request to display.</param>
        /// <param name="navigationStore">The navigation store to manage view navigation.</param>
        public RequestDetailsViewModel(ReportViewModel serviceRequest, NavigationStore navigationStore)
        {
            this.ServiceRequest = serviceRequest;
            this.SearchRequestNavCommand = new NavCommand(new Services.NavigationService(navigationStore, () => { return new SearchRequestViewModel(); }));
        }
    }
}
//---------------------------------------EOF-------------------------------------------