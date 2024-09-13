using Municipal_App.Commands;
using Municipal_App.Stores;
using Municipal_App.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Municipal_App.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private NavigationStore NavigationStore => AppStore.Instance.NavigationStore;
        public ViewModelBase CurrentViewModel => NavigationStore.CurrentViewModel;
        public BannerMessageViewModel CurrentBannerMessageVM { get; }

        public SubmitFeedbackCommand SubmitFeedbackCommand { get; }

        private string _feedbackMessage;
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

        public MainViewModel()
        {
            // Setting the initial view shown
            this.NavigationStore.CurrentViewModel = new LandingViewModel();

            // Setting up Banner System
            var bannerStore = AppStore.Instance.BannerMessageStore;
            this.CurrentBannerMessageVM = new BannerMessageViewModel();
            this.SubmitFeedbackCommand = new SubmitFeedbackCommand(this.ClearFeedbackMessage);

            // Subscribing to the CurrentViewModelChanged event (ensures that the UI updates when view model changes)
            this.NavigationStore.CurrentViewModelChanged += this.OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void ClearFeedbackMessage()
        {
            this.FeedbackMessage = string.Empty;
        }
    }
}
