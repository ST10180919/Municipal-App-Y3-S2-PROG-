using Municipal_App.Commands;
using Municipal_App.Services;
using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Municipal_App.ViewModels
{
    internal class IssuesViewModel : ViewModelBase
    {
        public ICommand MainViewNavCommand { get; }
        public IssuesViewModel()
        {
            var navigationStore = AppStore.Instance.NavigationStore;
            this.MainViewNavCommand = new NavCommand(new NavigationService(navigationStore, CreateLandingViewModel));
        }

        public LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel();
        }
    }
}
