using System.ComponentModel;

namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Base view model class to be extended by each other view model.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Exposing the PropertyChanged event from INofifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Raises the PropertyChanged event for the property with the given propertyName
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//---------------------------------------EOF-------------------------------------------