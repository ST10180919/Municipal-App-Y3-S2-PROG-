using Municipal_App.Commands;
using Municipal_App.Models;
using Municipal_App.Services;
using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Class containing the business logic for the Events View
    /// </summary>
    internal class EventsViewModel : ViewModelBase
    {
        private string _feedbackLabel;
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Feedback Label for this ViewModel, exposed for the UI to bind to
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

        private ObservableQueue<MunicipalEventViewModel> _events;
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Queue containing web scraped events for the UI to bind to
        /// </summary>
        public ObservableQueue<MunicipalEventViewModel> EventsQueue
        {
            get
            {
                return _events;
            }
            set
            {
                _events = value;
                OnPropertyChanged(nameof(EventsQueue));
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate back to the MainViewModel
        /// </summary>
        public ICommand MainViewNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of the EventsViewModel
        /// </summary>
        public EventsViewModel()
        {
            this.FeedbackLabel = "How can we improve our events and annoucements?";

            // Setting up back naviagtion
            var navigationStore = AppStore.Instance.NavigationStore;
            this.MainViewNavCommand = new NavCommand(new Services.NavigationService(navigationStore, CreateLandingViewModel));

            // Testing web service
            var eventsStore = AppStore.Instance.EventsStore;
            this.EventsQueue = eventsStore.EventsQueue;

            if (!eventsStore.IsQueueInitialized)
            {
                Initialize();
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Calles the corresponding services to populate Events and or Announcements
        /// shown on the view Bound to this ViewModel
        /// </summary>
        private async void Initialize()
        {
            // Initializing events
            var webservice = new EventsWebService();
            await webservice.LoadEventsAsync(this.EventsQueue);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new LandingViewModel
        /// </summary>
        /// <returns></returns>
        public LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel();
        }
    }
}
//---------------------------------------EOF-------------------------------------------