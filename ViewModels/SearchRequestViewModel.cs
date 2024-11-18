using Municipal_App.Commands;
using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// ViewModel for managing the search functionality and navigation for service requests.
    /// </summary>
    internal class SearchRequestViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate back to the LandingView.
        /// </summary>
        public ICommand LandingNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The text entered by the user for searching service requests.
        /// Updates the search term in the IssueReportStore when changed.
        /// </summary>
        private string _searchText;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the search text for filtering service requests.
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    AppStore.Instance.IssueReportStore.SearchTerm = _searchText;
                }
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequestViewModel"/> class.
        /// Sets up navigation to the LandingView.
        /// </summary>
        public SearchRequestViewModel()
        {
            // Setup navigation
            var appNavStore = AppStore.Instance.NavigationStore;
            this.LandingNavCommand = new NavCommand(new Services.NavigationService(appNavStore, CreateLandingViewModel));
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates and returns a new instance of the LandingViewModel.
        /// </summary>
        /// <returns>A new <see cref="LandingViewModel"/> instance.</returns>
        public LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel();
        }
    }
}
//---------------------------------------EOF-------------------------------------------