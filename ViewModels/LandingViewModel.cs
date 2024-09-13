using Municipal_App.Commands;
using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// ViewModel for the LandingView
    /// </summary>
    internal class LandingViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Backing class for the FeedbackLabel
        /// </summary>
        private string _feedbackLabel;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Label shown above the feedback submission form. Exposed for the UI to bind 
        /// to.
        /// </summary>
        public string FeedbackLabel
        {
            get
            {
                return _feedbackLabel;
            }
            set
            {
                _feedbackLabel = value;
                OnPropertyChanged(nameof(FeedbackLabel));
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate to the IssuesView
        /// </summary>
        public ICommand ReportIssuesNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate to the EventsView
        /// </summary>
        public ICommand EventsAndAnnouncementsNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate to the RequestStatusView
        /// </summary>
        public ICommand RequestStatusNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates the LandingViewModel
        /// </summary>
        public LandingViewModel()
        {
            this.FeedbackLabel = "Suggest services to be added:";
            // Initialising the navigation commands
            var navigationStore = AppStore.Instance.NavigationStore;
            this.ReportIssuesNavCommand = new NavCommand(new Services.NavigationService(navigationStore, this.CreateIssuesViewModel));

            // To be implemented in future POE parts
            //this.EventsAndAnnouncementsNavCommand = new NavCommand(new Services.NavigationService(navigationStore, this.CreateEventsViewModel));
            //this.RequestStatusNavCommand = new NavCommand(new Services.NavigationService(navigationStore, this.CreateRequestStatusViewModel));
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new CreateIssuesViewModel
        /// </summary>
        /// <returns></returns>
        private ReportIssuesViewModel CreateIssuesViewModel()
        {
            return new ReportIssuesViewModel();
        }

        // To be implemented in future POE parts
        //private EventsViewModel CreateEventsViewModel()
        //{
        //    return new EventsViewModel();
        //}

        //private RequestStatusViewModel CreateRequestStatusViewModel()
        //{
        //    return new RequestStatusViewModel();
        //}
    }
}
//---------------------------------------EOF-------------------------------------------