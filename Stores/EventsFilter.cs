using Municipal_App.Services;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Municipal_App.Stores
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Handles filtering for events based on search text, category, and date. 
    /// This class manages the logic for determining whether an event should be displayed 
    /// based on the current search and filter criteria.
    /// </summary>
    internal class EventsFilter
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Action that is triggered when the filter is applied to the events.
        /// </summary>
        public Action OnFilterEvents { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the search text used to filter events by title. If the search text is empty, 
        /// all events are considered.
        /// </summary>
        public string SearchText { get; set; } = string.Empty;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the category text used to filter events by category. If the category text is empty, 
        /// all categories are considered.
        /// </summary>
        public string CategoryText { get; set; } = string.Empty;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the selected date filter type (AnyTime, Today, ThisWeek, or ThisMonth) 
        /// used to filter events based on the date.
        /// </summary>
        public DateType DateType { get; set; } = DateType.AnyTime;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Dictionary containing events sorted by date, which is used to evaluate the date filter.
        /// </summary>
        private SortedDictionary<DateType, ObservableCollection<MunicipalEventViewModel>> SortedEventsHere;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the EventsFilter class and assigns the provided sorted events dictionary.
        /// </summary>
        /// <param name="sortedEvents">Dictionary of events sorted by date.</param>
        public EventsFilter(SortedDictionary<DateType, ObservableCollection<MunicipalEventViewModel>> sortedEvents)
        {
            this.SortedEventsHere = sortedEvents;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Determines whether a given event should be accepted based on the 
        /// current search text, category, and date filters.
        /// </summary>
        /// <param name="item">The MunicipalEventViewModel being evaluated.</param>
        /// <returns>True if the event meets the filter criteria, false otherwise.</returns>
        public bool IsAccepted(MunicipalEventViewModel item)
        {
            bool searchTestPassed = true;
            bool categoryTestPassed = true;
            bool dateTestPassed = true;

            try
            {
                if (SearchText != string.Empty)
                {
                    searchTestPassed = item.Title.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (CategoryText != string.Empty)
                {
                    categoryTestPassed = item.Category == CategoryText;
                }

                if (DateType != DateType.AnyTime)
                {
                    dateTestPassed = SortedEventsHere[DateType].Contains(item);
                }
                
                return searchTestPassed && categoryTestPassed && dateTestPassed;
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