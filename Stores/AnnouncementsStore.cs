using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Stores
{
    using global::Municipal_App.Models;
    using global::Municipal_App.Services;
    using global::Municipal_App.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Municipal_App.Stores
    {
        //---------------------------------------------------------------------------------
        /// <summary>
        /// Stores the announcement information scraped from the internet. This class manages the queue of announcements, 
        /// provides filtering and recommendation functionalities, and tracks announcements by date.
        /// </summary>
        internal class AnnouncementsStore
        {
            //-----------------------------------------------------------------------------
            /// <summary>
            /// Queue storing the announcements in the order they were scraped from the internet.
            /// </summary>
            public ObservableQueue<AnnouncementViewModel> AnnouncementsQueue { get; private set; } = new ObservableQueue<AnnouncementViewModel>();

            //-----------------------------------------------------------------------------
            /// <summary>
            /// Indicates whether the announcements queue has been initialized. 
            /// True if initialized and contains items, false if empty.
            /// </summary>
            public bool IsQueueInitialized => AnnouncementsQueue.Count > 0;

            //-----------------------------------------------------------------------------
            /// <summary>
            /// A sorted dictionary of announcements, keyed by date type (Today, ThisWeek, ThisMonth, AnyTime). 
            /// Used to allow users to filter the announcements by date.
            /// </summary>
            public SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>> SortedAnnouncements { get; private set; }

            //-----------------------------------------------------------------------------
            /// <summary>
            /// The filter store for announcements, responsible for applying search and date filters.
            /// </summary>
            public AnnouncementsFilter FilterStore { get; private set; }

            //-----------------------------------------------------------------------------
            /// <summary>
            /// The recommendation service responsible for determining which announcements to recommend based on user preferences.
            /// </summary>
            public RecommendationService RecommendationService { get; private set; }

            //-----------------------------------------------------------------------------
            /// <summary>
            /// Creates a new instance of the AnnouncementsStore class. This initializes the sorted dictionary, filter store, and recommendation service.
            /// </summary>
            public AnnouncementsStore()
            {
                this.SortedAnnouncements = new SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>>();
                this.FilterStore = new AnnouncementsFilter(this.SortedAnnouncements);
                this.RecommendationService = new RecommendationService();
            }

            //-----------------------------------------------------------------------------
            /// <summary>
            /// Updates the SortedAnnouncements dictionary by adding a new announcement based on its date. 
            /// The announcement is categorized by date (Today, ThisWeek, ThisMonth, or AnyTime).
            /// </summary>
            /// <param name="announcement">The AnnouncementViewModel to be added to the sorted dictionary.</param>
            public void UpdateSortedDictionary(AnnouncementViewModel announcement)
            {
                if (DateTime.TryParse(announcement.Date, out DateTime announcementDate))
                {
                    DateTime now = DateTime.Now;
                    DateType dateType;

                    // Determine which DateType the event falls into
                    if (announcementDate.Date == now.Date)
                    {
                        dateType = DateType.Today;
                    }
                    else if (announcementDate >= now && announcementDate <= now.AddDays(7))
                    {
                        dateType = DateType.ThisWeek;
                    }
                    else if (announcementDate.Year == now.Year && announcementDate.Month == now.Month)
                    {
                        dateType = DateType.ThisMonth;
                    }
                    else
                    {
                        dateType = DateType.AnyTime;
                    }

                    // Add the announcement to the SortedAnnouncements dictionary based on the determined date type
                    if (!SortedAnnouncements.ContainsKey(dateType))
                    {
                        SortedAnnouncements[dateType] = new ObservableCollection<AnnouncementViewModel>();
                    }

                    SortedAnnouncements[dateType].Add(announcement);
                }
            }
        }
    }
    //---------------------------------------EOF-------------------------------------------
}
