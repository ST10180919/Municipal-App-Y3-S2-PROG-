using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Municipal_App.ViewModels
{
    /* Credits : Most of this code here
     * https://www.youtube.com/watch?v=Kh4NFd5y3k0
     */
    //---------------------------------------------------------------------------------
    /// <summary>
    /// View model for the BannerMessage component to bind to
    /// </summary>
    public class BannerMessageViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The instance of the banner store used
        /// </summary>
        private readonly BannerMessageStore _bannerStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The message shown on the banner
        /// </summary>
        public string Message => _bannerStore.BannerMessage;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The type of banner to be shown
        /// </summary>
        public BannerType Type => _bannerStore.BannerType;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the banner has a message to show
        /// </summary>
        public bool HasMessage => _bannerStore.HasMessage;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bannerStore"></param>
        public BannerMessageViewModel()
        {
            _bannerStore = AppStore.Instance.BannerMessageStore;
            _bannerStore.BannerMessageChanged += OnBannerMessageChanged;
            _bannerStore.BannerTypeChanged += OnBannerTypeChanged;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Notifies UI that the type of the banner has changed
        /// </summary>
        private void OnBannerTypeChanged()
        {
            OnPropertyChanged(nameof(Type));
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Notifies UI that the name of the banner has changed
        /// </summary>
        private void OnBannerMessageChanged()
        {
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(HasMessage));
        }
    }
}
