using Municipal_App.Models;
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
        public SortedDictionary<DateTime, ObservableCollection<MunicipalEventViewModel>> SortedEvents { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// A set containing all of the Categories assigned to currently stored events.
        /// </summary>
        public HashSet<string> EventCategories { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// A set containing all of the Locations assigned to currently stored events.
        /// </summary>
        public HashSet<string> EventLocations { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of the EventsStore
        /// </summary>
        public EventsStore()
        {
            this.SortedEvents = new SortedDictionary<DateTime, ObservableCollection<MunicipalEventViewModel>>();
            this.EventCategories = new HashSet<string>();
            this.EventLocations = new HashSet<string>();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Updates the SortedEvents, EventCategories, and EventLocations fields by
        /// adding the new municipalEvent.
        /// </summary>
        /// <param name="municipalEvent"> 
        /// MunicipalEventViewModel to be added to the fields 
        /// </param>
        public void UpdateFields(MunicipalEventViewModel municipalEvent)
        {
            if (DateTime.TryParse(municipalEvent.Date, out DateTime eventDate))
            {
                // Adding the event to the SortedDictionary, keyed by date
                if (!SortedEvents.ContainsKey(eventDate))
                {
                    SortedEvents[eventDate] = new ObservableCollection<MunicipalEventViewModel>();
                }

                // Add the event to the list of events for that date
                SortedEvents[eventDate].Add(municipalEvent);
            }

            // Add the event category to the EventCategories set
            if (!string.IsNullOrEmpty(municipalEvent.Category))
            {
                EventCategories.Add(municipalEvent.Category);
            }

            // Add the venue to the EventLocations set
            if (!string.IsNullOrEmpty(municipalEvent.Venue))
            {
                EventLocations.Add(municipalEvent.Venue);
            }
        }
    }
}
//---------------------------------------EOF-------------------------------------------