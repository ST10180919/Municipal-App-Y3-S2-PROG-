﻿using Municipal_App.Commands;
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

        public ObservableQueue<AnnouncementViewModel> AnnouncementsQueue { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate back to the MainViewModel
        /// </summary>
        public ICommand MainViewNavCommand { get; }

        public RelayCommand ApplySearchFilterCommand { get; set; }

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
        /// Calles the corresponding services to populate Events and or Announcements
        /// shown on the view Bound to this ViewModel
        /// </summary>
        private async void InitializeEvents()
        {
            // Initializing events
            var webservice = new EventsWebService();
            await webservice.LoadEventsAsync(this.EventsQueue);
        }

        private async void InitializeAnnouncements()
        {
            var webservice = new AnnouncementsWebService();
            await webservice.LoadAnnouncementsAsync(this.AnnouncementsQueue);
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