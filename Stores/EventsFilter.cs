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

    internal class EventsFilter
    {
        public Action OnFilterEvents { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public string CategoryText { get; set; } = string.Empty;
        public DateType DateType { get; set; } = DateType.AnyTime;

        private SortedDictionary<DateType, ObservableCollection<MunicipalEventViewModel>> SortedEventsHere;

        public EventsFilter(SortedDictionary<DateType, ObservableCollection<MunicipalEventViewModel>> sortedEvents)
        {
            this.SortedEventsHere = sortedEvents;
        }

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
