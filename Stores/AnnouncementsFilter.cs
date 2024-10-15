using Municipal_App.ViewModels;
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
    /// Handles filtering for announcements based on search text and date.
    /// Manages the logic for determining whether an announcement should be displayed based on 
    /// criteria like title search and date filtering. It also triggers an event when a filter is applied.
    /// </summary>
    internal class AnnouncementsFilter
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Action that is triggered when the filter is applied to the announcements.
        /// </summary>
        public Action OnFilterAnnouncements { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the search text used to filter announcements. If the search text is empty, 
        /// all announcements are considered.
        /// </summary>
        public string SearchText { get; set; } = string.Empty;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the selected date filter type (AnyTime, Today, ThisWeek, or ThisMonth) 
        /// used to filter announcements based on the date.
        /// </summary>
        public DateType DateType { get; set; } = DateType.AnyTime;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Dictionary containing announcements sorted by date, which is used to evaluate the date filter.
        /// </summary>
        private SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>> SortedEventsHere;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the AnnouncementsFilter class and assigns the provided sorted events dictionary.
        /// </summary>
        /// <param name="sortedEventsHere">Dictionary of announcements sorted by date.</param>
        public AnnouncementsFilter(SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>> sortedEventsHere)
        {
            this.SortedEventsHere = sortedEventsHere;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Determines whether a given announcement should be accepted based on the 
        /// current search text and date filters.
        /// </summary>
        /// <param name="item">The AnnouncementViewModel being evaluated.</param>
        /// <returns>True if the announcement meets the filter criteria, false otherwise.</returns>
        public bool IsAccepted(AnnouncementViewModel item)
        {
            bool searchTestPassed = true;
            bool dateTestPassed = true;

            try
            {
                if (SearchText != string.Empty)
                {
                    searchTestPassed = item.Title.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (DateType != DateType.AnyTime)
                {
                    dateTestPassed = SortedEventsHere[DateType].Contains(item);
                }

                return searchTestPassed && dateTestPassed;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
//---------------------------------------EOF-------------------------------------------