using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Stores
{
    using global::Municipal_App.Models;
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
        /// Stores the the announcement information scraped from the internet.
        /// </summary>
        internal class AnnouncementsStore
        {
            //-----------------------------------------------------------------------------
            /// <summary>
            /// Queue Storing the events in the order they were scraped from the internet.
            /// </summary>
            public ObservableQueue<AnnouncementViewModel> AnnouncementsQueue { get; private set; } = new ObservableQueue<AnnouncementViewModel>();

            //-----------------------------------------------------------------------------
            /// <summary>
            /// Indicates whether the Queue has been initialized or not
            /// True if initialized, false if not
            /// </summary>
            public bool IsQueueInitialized => AnnouncementsQueue.Count > 0;

            //-----------------------------------------------------------------------------
            /// <summary>
            /// A sorted Dictionary of Announcements keyed by date. Used to allow the user to 
            /// filter the announcements by date 
            /// </summary>
            public SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>> SortedAnnouncements { get; private set; }

            public AnnouncementsFilter FilterStore { get; private set; }

            //-----------------------------------------------------------------------------
            /// <summary>
            /// Creates a new instance of the AnnouncementsStore
            /// </summary>
            public AnnouncementsStore()
            {
                this.SortedAnnouncements = new SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>>();
                this.FilterStore = new AnnouncementsFilter(this.SortedAnnouncements);
            }

            //-----------------------------------------------------------------------------
            /// <summary>
            /// Updates the SortedAnnouncements adding the new announcement.
            /// </summary>
            /// <param name="announcement"> 
            /// AnnouncementViewModel to be added to the fields 
            /// </param>
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
