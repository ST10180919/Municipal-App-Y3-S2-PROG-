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

        public BannerMessageStore BannerMessageStore { get; private set; }

        public IssueReportStore IssueReportStore { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        private AppStore() 
        { 
            this.NavigationStore = new NavigationStore();
            this.BannerMessageStore = new BannerMessageStore();
            this.IssueReportStore = new IssueReportStore();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Property for getting the singleton instance
        /// </summary>
        public static AppStore Instance { get { return _instance; } }
    }
}
//---------------------------------------EOF-------------------------------------------