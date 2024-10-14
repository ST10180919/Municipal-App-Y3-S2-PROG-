using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Stores
{
    internal class AnnouncementsFilter
    {
        public Action OnFilterAnnouncements { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public DateType DateType { get; set; } = DateType.AnyTime;

        private SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>> SortedEventsHere;

        public AnnouncementsFilter(SortedDictionary<DateType, ObservableCollection<AnnouncementViewModel>> sortedEventsHere)
        {
            this.SortedEventsHere = sortedEventsHere;
        }

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
