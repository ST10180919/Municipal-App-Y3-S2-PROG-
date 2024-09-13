using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Municipal_App.Stores
{
    /* Credits: Basic Idea here
     * https://www.youtube.com/watch?v=Kh4NFd5y3k0
     */
    //---------------------------------------------------------------------------------
    /// <summary>
    /// The different types of banners that can be used.
    /// </summary>
    public enum BannerType
    {
        Error,
        Confirmation,
        Validation
    }

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Stores the instance of a banner to be shown to the user. This banner is set to 
    /// disapear after a certain amount of time.
    /// </summary>
    public class BannerMessageStore
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The text content of the banner
        /// </summary>
        private string _bannerMessage;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Property used to get and set the _bannerMessage
        /// </summary>
        public string BannerMessage
        {
            get { return _bannerMessage; }
            set { _bannerMessage = value; BannerMessageChanged?.Invoke(); }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The type of banner being shown
        /// </summary>
        private BannerType _bannerType;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets the _bannerType
        /// </summary>
        public BannerType BannerType
        {
            get { return _bannerType; }
            set { _bannerType = value; BannerTypeChanged?.Invoke(); }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The timer used to make the banner disapear after a certain amount of time
        /// </summary>
        private readonly Timer _timer;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores a boolean value which determines whether the banner currently has
        /// a message or not
        /// </summary>
        public bool HasMessage => !string.IsNullOrEmpty(_bannerMessage);

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Event used to notify subscribers that the banner message has changed
        /// </summary>
        public event Action BannerMessageChanged;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Event used to notify subscribers that the banner type has changed
        /// </summary>
        public event Action BannerTypeChanged;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        public BannerMessageStore()
        {
            _timer = new Timer(3000);

            // Determines what happens after the timer has elapsed
            _timer.Elapsed += OnTimerElapsed;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// This code executes when the timer elapses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BannerMessage = string.Empty;
            _timer.Stop();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Sets the BannerMessage and BannerType to the given values and starts the 
        /// timer
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bannerType"></param>
        public void SetBanner(string message, BannerType bannerType)
        {
            BannerMessage = message;
            BannerType = bannerType;

            // Starting the timer
            _timer.Start();
        }
    }
}
//---------------------------------------EOF-------------------------------------------