using Municipal_App.Commands;
using Municipal_App.Stores;
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
            this.EventsAndAnnouncementsNavCommand = new NavCommand(new Services.NavigationService(navigationStore, this.CreateEventsViewModel));

            // To be implemented in future POE parts
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

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new CreateEventsViewModel
        /// </summary>
        /// <returns></returns>
        private EventsAnnouncementsViewModel CreateEventsViewModel()
        {
            return new EventsAnnouncementsViewModel();
        }

        //private RequestStatusViewModel CreateRequestStatusViewModel()
        //{
        //    return new RequestStatusViewModel();
        //}
    }
}
//---------------------------------------EOF-------------------------------------------