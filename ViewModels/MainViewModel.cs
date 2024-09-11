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
        public MainViewModel()
        {
            // Setting the initial view shown
            this.NavigationStore.CurrentViewModel = new LandingViewModel();

            // Subscribing to the CurrentViewModelChanged event (ensures that the UI updates when view model changes)
            this.NavigationStore.CurrentViewModelChanged += this.OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
