using Municipal_App.Models;
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
    public enum DateType
    {
        AnyTime,
        Today,
        ThisWeek,
        ThisMonth
    }

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Stores the the event information scraped from the internet.
    /// </summary>
    internal class EventsStore
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Queue Storing the events in the order they were scraped from the internet.
        /// </summary>
        public ObservableQueue<MunicipalEventViewModel> EventsQueue { get; private set; } = new ObservableQueue<MunicipalEventViewModel>();

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Indicates whether the Queue has been initialized or not
        /// True if initialized, false if not
        /// </summary>
        public bool IsQueueInitialized => EventsQueue.Count > 0;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// A sorted Dictionary of Events keyed by date. Used to allow the user to 
        /// filter the events by date 
        /// </summary>
        public SortedDictionary<DateType, ObservableCollection<MunicipalEventViewModel>> SortedEvents { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// A set containing all of the Categories assigned to currently stored events.
        /// </summary>
        public HashSet<string> EventCategories { get; private set; }

        public Action<string> OnEventCategoryAdded { get; set; }

        public EventsFilter FilterStore { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of the EventsStore
        /// </summary>
        public EventsStore()
        {
            this.SortedEvents = new SortedDictionary<DateType, ObservableCollection<MunicipalEventViewModel>>();
            this.EventCategories = new HashSet<string>();
            this.FilterStore = new EventsFilter(this.SortedEvents);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Updates the SortedEvents, EventCategories
        /// </summary>
        /// <param name="municipalEvent"> 
        /// MunicipalEventViewModel to be added to the fields 
        /// </param>
        public void UpdateFields(MunicipalEventViewModel municipalEvent)
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
                    SortedEvents[dateType] = new ObservableCollection<MunicipalEventViewModel>();
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