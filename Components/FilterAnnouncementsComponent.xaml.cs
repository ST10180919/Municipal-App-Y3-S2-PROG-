using Municipal_App.Stores;
using Municipal_App.Stores.Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Municipal_App.Components
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// A user control for filtering announcements in the application.
    /// This component provides UI elements for selecting filter criteria such as date,
    /// and it allows the user to apply filters to the displayed announcements.
    /// </summary>
    public partial class FilterAnnouncementsComponent : UserControl
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the instance of the AnnouncementsStore from the application state.
        /// This store is used for managing and filtering announcements.
        /// </summary>
        private AnnouncementsStore AnnouncementsStore => AppStore.Instance.AnnouncementsStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterAnnouncementsComponent"/> class.
        /// </summary>
        public FilterAnnouncementsComponent()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the filter button. Toggles the visibility of the filter popup.
        /// When the popup is opened, it populates the date combobox with the appropriate filter options.
        /// </summary>
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = !FilterPopup.IsOpen;  // Toggle popup visibility

            // Populate date combobox
            this.PopulateDates();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Populates the DateComboBox with date filter options based on the current selected date type.
        /// Ensures that the currently selected date type appears first in the list.
        /// </summary>
        private void PopulateDates()
        {
            var existingDateType = AnnouncementsStore.FilterStore.DateType;
            DateComboBox.Items.Clear();

            // True coding at it's finest
            if (existingDateType.Equals(DateType.AnyTime))
            {
                DateComboBox.Items.Add("Any Time");
                DateComboBox.Items.Add("Today");
                DateComboBox.Items.Add("This Week");
                DateComboBox.Items.Add("This Month");
            }
            else if (existingDateType.Equals(DateType.Today))
            {
                DateComboBox.Items.Add("Today");
                DateComboBox.Items.Add("Any Time");
                DateComboBox.Items.Add("This Week");
                DateComboBox.Items.Add("This Month");
            }
            else if (existingDateType.Equals(DateType.ThisWeek))
            {
                DateComboBox.Items.Add("This Week");
                DateComboBox.Items.Add("Any Time");
                DateComboBox.Items.Add("Today");
                DateComboBox.Items.Add("This Month");
            }
            else if (existingDateType.Equals(DateType.ThisMonth))
            {
                DateComboBox.Items.Add("This Month");
                DateComboBox.Items.Add("Any Time");
                DateComboBox.Items.Add("Today");
                DateComboBox.Items.Add("This Week");
            }
            DateComboBox.SelectedIndex = 0;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the apply filter button.
        /// Updates the date filter based on the selected value from the DateComboBox,
        /// and triggers the announcement filtering process.
        /// </summary>
        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var dateText = DateComboBox.Text;

            var dateType = DateType.AnyTime;
            if (dateText.Equals("Any Time"))
            {
                dateType = DateType.AnyTime;
            }
            else
            {
                if (dateText.Equals("Today"))
                {
                    dateType = DateType.Today;
                }
                else if (dateText.Equals("This Week"))
                {
                    dateType = DateType.ThisWeek;
                }
                else if (dateText.Equals("This Month"))
                {
                    dateType = DateType.ThisMonth;
                }
                // Add the term to the for recommendation consideration
                AnnouncementsStore.RecommendationService.AddTerm(Services.RecommendationTermType.Date, dateType.ToString());
            }
            AnnouncementsStore.FilterStore.DateType = dateType;

            AnnouncementsStore.FilterStore.OnFilterAnnouncements?.Invoke();
        }
    }
}
//---------------------------------------EOF-------------------------------------------