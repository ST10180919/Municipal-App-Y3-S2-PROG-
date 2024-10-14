using Municipal_App.Stores.Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Stores
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Singleton class containing the different state stores used in the application
    /// </summary>
    internal class AppStore
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Instance of the AppStore Singleton
        /// </summary>
        private static AppStore _instance = new AppStore();

        //-----------------------------------------------------------------------------
        /// <summary>
        /// NavigationStore used to store the current page shown to the user
        /// </summary>
        public NavigationStore NavigationStore { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the current state of the banner shown to the user. Allows for the
        /// changing of the banner, which shows it to the user.
        /// </summary>
        public BannerMessageStore BannerMessageStore { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the reports submitted by users
        /// </summary>
        public IssueReportStore IssueReportStore { get; private set; }

        public EventsStore EventsStore { get; private set; }

        public AnnouncementsStore AnnouncementsStore { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates the AppStore
        /// </summary>
        private AppStore() 
        { 
            this.NavigationStore = new NavigationStore();
            this.BannerMessageStore = new BannerMessageStore();
            this.IssueReportStore = new IssueReportStore();
            this.EventsStore = new EventsStore();
            this.AnnouncementsStore = new AnnouncementsStore();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Property for getting the singleton instance
        /// </summary>
        public static AppStore Instance { get { return _instance; } }
    }
}
//---------------------------------------EOF-------------------------------------------