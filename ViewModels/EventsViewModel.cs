using Municipal_App.Commands;
using Municipal_App.Models;
using Municipal_App.Services;
using Municipal_App.Stores;
using Municipal_App.Stores.Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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

        private string _searchText = string.Empty;
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The search text entered by the user. Updates the filter stores for events 
        /// and announcements when modified.
        /// </summary>
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                AppStore.Instance.EventsStore.FilterStore.SearchText = value;
                AppStore.Instance.AnnouncementsStore.FilterStore.SearchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Queue containing web scraped events for the UI to bind to
        /// </summary>
        public ObservableQueue<MunicipalEventViewModel> EventsQueue { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Queue containing web scraped announcements for the UI to bind to.
        /// </summary>
        public ObservableQueue<AnnouncementViewModel> AnnouncementsQueue { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate back to the MainViewModel
        /// </summary>
        public ICommand MainViewNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to apply search filters to events and announcements.
        /// </summary>
        public RelayCommand ApplySearchFilterCommand { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of the EventsViewModel, sets up the navigation command,
        /// initializes the events and announcements queues, and prepares the search filter command.
        /// </summary>
        public EventsViewModel()
        {
            this.FeedbackLabel = "How can we improve our events and annoucements?";

            // Setting up back naviagtion
            var navigationStore = AppStore.Instance.NavigationStore;
            this.MainViewNavCommand = new NavCommand(new Services.NavigationService(navigationStore, CreateLandingViewModel));

            // Setting up events
            var eventsStore = AppStore.Instance.EventsStore;
            this.EventsQueue = eventsStore.EventsQueue;

            if (!eventsStore.IsQueueInitialized)
            {
                InitializeEvents();
            }

            // Setting up Announcements
            var announcementsStore = AppStore.Instance.AnnouncementsStore;
            this.AnnouncementsQueue = announcementsStore.AnnouncementsQueue;

            if (!announcementsStore.IsQueueInitialized)
            {
                InitializeAnnouncements();
            }

            // Initializing relaycommands
            ApplySearchFilterCommand = new RelayCommand(o => this.ApplyFilters());
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Applies the search filters to events and announcements by adding the search
        /// text to the recommendation service and invoking the filter actions.
        /// </summary>
        private void ApplyFilters()
        {
            // Add search term to recommendations
            if (SearchText != string.Empty)
            {
                AppStore.Instance.EventsStore.RecommendationService.AddTerm(RecommendationTermType.Search, SearchText);
                AppStore.Instance.AnnouncementsStore.RecommendationService.AddTerm(RecommendationTermType.Search, SearchText);
            }

            // Invoke Filter Actions
            AppStore.Instance.EventsStore.FilterStore.OnFilterEvents?.Invoke();
            AppStore.Instance.AnnouncementsStore.FilterStore.OnFilterAnnouncements?.Invoke();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Calls the corresponding service to populate the EventsQueue with web-scraped events.
        /// </summary>
        private async void InitializeEvents()
        {
            // Initializing events
            var webservice = new EventsWebService();
            await webservice.LoadEventsAsync(this.EventsQueue);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Calls the corresponding service to populate the AnnouncementsQueue with web-scraped announcements.
        /// </summary>
        private async void InitializeAnnouncements()
        {
            var webservice = new AnnouncementsWebService();
            await webservice.LoadAnnouncementsAsync(this.AnnouncementsQueue);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of the LandingViewModel to navigate to the landing page.
        /// </summary>
        /// <returns>LandingViewModel instance.</returns>
        public LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel();
        }
    }
}
//---------------------------------------EOF-------------------------------------------