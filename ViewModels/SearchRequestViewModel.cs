﻿using Municipal_App.Commands;
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
    internal class SearchRequestViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate back to the LandingView
        /// </summary>
        public ICommand LandingNavCommand { get; }

        private string _searchText;
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

        public SearchRequestViewModel()
        {
            // Setup navigation
            var appNavStore = AppStore.Instance.NavigationStore;
            this.LandingNavCommand = new NavCommand(new Services.NavigationService(appNavStore, CreateLandingViewModel));
        }

        public LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel();
        }
    }
}
