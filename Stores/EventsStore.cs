using Municipal_App.Models;
using Municipal_App.Services;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Municipal_App.Stores
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Enum representing the different date types for filtering events.
    /// </summary>
    public enum DateType
    {
        AnyTime,
        Today,
        ThisWeek,
        ThisMonth
    }

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Stores the event information scraped from the internet, including functionality 
    /// to filter and sort events based on date, category, and other criteria.
    /// </summary>
    internal class EventsStore
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Indicates whether the queue has been initialized or not.
        /// True if initialized, false if not.
        /// </summary>
        public ObservableQueue<EventViewModel> EventsQueue { get; private set; } = new ObservableQueue<EventViewModel>();

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Indicates whether the Queue has been initialized or not
        /// True if initialized, false if not
        /// </summary>
        public bool IsQueueInitialized => EventsQueue.Count > 0;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// A sorted dictionary of events keyed by date type. This allows the user to 
        /// filter events based on date ranges like "Today," "This Week," or "This Month."
        /// </summary>
        public SortedDictionary<DateType, ObservableCollection<EventViewModel>> SortedEvents { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// A set containing all of the categories assigned to currently stored events.
        /// Used to filter events by category.
        /// </summary>
        public HashSet<string> EventCategories { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Action that gets triggered whenever a new event category is added.
        /// </summary>
        public Action<string> OnEventCategoryAdded { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Manages filtering events based on search text, category, and date criteria.
        /// </summary>
        public EventsFilter Filter { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Manages recommending events based on user interactions and search behavior.
        /// </summary>
        public RecommendationService RecommendationService { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of the EventsStore, initializing the sorted events 
        /// dictionary, event categories set, recommendation service, and filter store.
        /// </summary>
        public EventsStore()
        {
            this.SortedEvents = new SortedDictionary<DateType, ObservableCollection<EventViewModel>>();
            this.EventCategories = new HashSet<string>();
            this.RecommendationService = new RecommendationService();
            this.Filter = new EventsFilter(this.SortedEvents);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Updates the sorted events dictionary and event categories with a new event.
        /// This method determines the appropriate date type for the event and adds it
        /// to the corresponding category and date filter collections.
        /// </summary>
        /// <param name="municipalEvent">
        /// The event to be added to the store.
        /// </param>
        public void UpdateFields(EventViewModel municipalEvent)
        {
            if (DateTime.TryParse(municipalEvent.Date, out DateTime eventDate))
            {
                DateTime now = DateTime.Now;
                DateType dateType;

                // Determine which DateType the event falls into
                if (eventDate.Date == now.Date)
                {
                    dateType = DateType.Today;
                }
                else if (eventDate >= now && eventDate <= now.AddDays(7))
                {
                    dateType = DateType.ThisWeek;
                }
                else if (eventDate.Year == now.Year && eventDate.Month == now.Month)
                {
                    dateType = DateType.ThisMonth;
                }
                else
                {
                    dateType = DateType.AnyTime;
                }

                // Add the event to the SortedEvents dictionary based on the determined date type
                if (!SortedEvents.ContainsKey(dateType))
                {
                    SortedEvents[dateType] = new ObservableCollection<EventViewModel>();
                }

                SortedEvents[dateType].Add(municipalEvent);
            }

            // Add the event category to the EventCategories set
            if (!string.IsNullOrEmpty(municipalEvent.Category))
            {
                EventCategories.Add(municipalEvent.Category);
                this.OnEventCategoryAdded?.Invoke(municipalEvent.Category);
            }
        }
    }
}
//---------------------------------------EOF-------------------------------------------