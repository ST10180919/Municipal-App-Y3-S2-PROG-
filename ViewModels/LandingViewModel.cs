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
    internal class LandingViewModel : ViewModelBase
    {
        private string _feedbackLabel;
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
        private NavigationStore NavigationStore => AppStore.Instance.NavigationStore;
        public ICommand ReportIssuesNavCommand { get; }
        public ICommand EventsAndAnnouncementsNavCommand { get; }
        public ICommand RequestStatusNavCommand { get; }
        public LandingViewModel()
        {
            this.FeedbackLabel = "Suggest services to be added:";
            // Initialising the navigation commands
            this.ReportIssuesNavCommand = new NavCommand(new Services.NavigationService(this.NavigationStore, this.CreateIssuesViewModel));
            this.EventsAndAnnouncementsNavCommand = new NavCommand(new Services.NavigationService(this.NavigationStore, this.CreateEventsViewModel));
            this.RequestStatusNavCommand = new NavCommand(new Services.NavigationService(this.NavigationStore, this.CreateRequestStatusViewModel));
        }

        private ReportIssuesViewModel CreateIssuesViewModel()
        {
            return new ReportIssuesViewModel();
        }
        private EventsViewModel CreateEventsViewModel()
        {
            return new EventsViewModel();
        }

        private RequestStatusViewModel CreateRequestStatusViewModel()
        {
            return new RequestStatusViewModel();
        }
    }
}
