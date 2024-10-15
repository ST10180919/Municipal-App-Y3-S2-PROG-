using Municipal_App.Commands;
using Municipal_App.Stores;

namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// ViewModel for the MainWindow
    /// </summary>
    internal class MainViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Exposed NavigationStore from the AppStore Singleton
        /// </summary>
        private NavigationStore NavigationStore => AppStore.Instance.NavigationStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// This field, bound to by the UI, determines which view is shown to the user
        /// at any given time, by relation to the view depending on the ViewModel type.
        /// When the value of this field changes, the view being shown to the user 
        /// changes to the view paired with that new ViewModel.
        /// </summary>
        public ViewModelBase CurrentViewModel => NavigationStore.CurrentViewModel;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// This field holds the binding fields for the banner functionality.
        /// </summary>
        public BannerMessageViewModel BannerMessageViewModel { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to submit the feedback provided by the user.
        /// </summary>
        public SubmitFeedbackCommand SubmitFeedbackCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Backing field for FeedbackMessage
        /// </summary>
        private string _feedbackMessage;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// FeedbackMessage bound to what the user enters for their feedback
        /// </summary>
        public string FeedbackMessage
        {
            get
            {
                return _feedbackMessage;
            }
            set
            {
                _feedbackMessage = value;
                OnPropertyChanged(nameof(FeedbackMessage));
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates the MainViewModel
        /// </summary>
        public MainViewModel()
        {
            // Setting the initial view shown
            this.NavigationStore.CurrentViewModel = new LandingViewModel();

            // Setting up Banner System
            var bannerStore = AppStore.Instance.BannerMessageStore;
            this.BannerMessageViewModel = new BannerMessageViewModel();
            this.SubmitFeedbackCommand = new SubmitFeedbackCommand(this.ClearFeedbackMessage);

            // Subscribing to the CurrentViewModelChanged event (ensures that the UI updates when view model changes)
            this.NavigationStore.CurrentViewModelChanged += this.OnCurrentViewModelChanged;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Updates the CurrentViewModel when a new view is being navigated to
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Clears the FeedbackMessage
        /// </summary>
        private void ClearFeedbackMessage()
        {
            this.FeedbackMessage = string.Empty;
        }
    }
}
//---------------------------------------EOF-------------------------------------------