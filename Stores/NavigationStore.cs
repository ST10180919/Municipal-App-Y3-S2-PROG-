using Municipal_App.ViewModels;
using System;

namespace Municipal_App.Stores
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// This class stores the current view model being shown for this application
    /// </summary>
    public class NavigationStore
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Current view model being shown to the user
        /// </summary>
        private ViewModelBase _currentViewModel;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Sets the _currentViewModel
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; OnCurrentViewModelChanged(); }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Raises the CurrentViewModelChanged event
        /// </summary>
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Event used to notify subscribers when the current view model has changed.
        /// </summary>
        public event Action CurrentViewModelChanged;
    }
}
//---------------------------------------EOF-------------------------------------------