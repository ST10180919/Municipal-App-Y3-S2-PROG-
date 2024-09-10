

using Municipal_App.Services;

namespace Municipal_App.Commands
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Command Used to navigate between view models, of which the view model property
    /// that needs to be changed and what it needs to be changed to is provided by the
    /// navigation service.
    /// </summary>
    public class NavCommand : CommandBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the navigation Service used
        /// </summary>
        private readonly NavigationService _navigationService;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService"></param>
        public NavCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Executes the NavCommand - Navigates the current view to a different view 
        /// model using the navigaiton service.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
//---------------------------------------EOF-------------------------------------------