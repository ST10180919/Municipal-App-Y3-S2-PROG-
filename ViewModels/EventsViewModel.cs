using Municipal_App.Commands;
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
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Backing field for the FeedbackLabel
        /// </summary>
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

        private string _testText;
        public string TestText
        {
            get
            {
                return _testText;
            }
            set
            {
                _testText = value;
                OnPropertyChanged(nameof(TestText));
            }
        }

        private BitmapImage _eventImage;
        public BitmapImage EventImage
        {
            get { return _eventImage; }
            set
            {
                _eventImage = value;
                OnPropertyChanged(nameof(EventImage)); 
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate back to the MainViewModel
        /// </summary>
        public ICommand MainViewNavCommand { get; }

        public EventsViewModel()
        {
            this.FeedbackLabel = "How can we improve our events and annoucements?";

            // Setting up back naviagtion
            var navigationStore = AppStore.Instance.NavigationStore;
            this.MainViewNavCommand = new NavCommand(new Services.NavigationService(navigationStore, CreateLandingViewModel));

            // Testing web service
            Initialize();
        }

        // Asynchronous initialization method
        private async void Initialize()
        {
            // Initializing events
            var webservice = new EventsWebService();
            await webservice.InitializeEvents();

            // Start both tasks in parallel, after initialization is done
            var loadTextTask = webservice.GetEventsTestString();
            var loadImageTask = webservice.GetImageTest();

            // Wait for the text and image to load independently
            this.EventImage = await loadImageTask;
            this.TestText = await loadTextTask;
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