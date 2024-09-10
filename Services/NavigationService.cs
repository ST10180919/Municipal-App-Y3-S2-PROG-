using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;

namespace Municipal_App.Services
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Used to navigate to a view model. Able to switch a viewmodel instance to a
    /// new view model.
    /// </summary>
    public class NavigationService
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// This navigation store instance holds the current view model being shown, 
        /// allowing the navigation service to change which view is being shown to the
        /// user.
        /// </summary>
        private readonly NavigationStore _navigationStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores a function used to create the viewmodel that the NavigationService
        /// will assign to the current view model. We use a function here to ensure that
        /// a new view model is created each time we switch view models (no memory leaks)
        /// and to allow the new view model to be created in a specific way.
        /// (required for this function to return the view model it creates)
        /// </summary>
        private readonly Func<ViewModelBase> _createViewModel;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationStore"></param>
        /// <param name="createViewModel"></param>
        public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> createViewModel)
        {
            this._navigationStore = navigationStore;
            this._createViewModel = createViewModel;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Navigates to the new view model by assigning the current view model to the 
        /// result of the _createViewModel fuction.
        /// </summary>
        public void Navigate()
        {
            this._navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
