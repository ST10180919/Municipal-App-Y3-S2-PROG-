using Municipal_App.Stores;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Municipal_App.Components
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// A user control for filtering events in the application.
    /// This component provides UI elements for selecting categories and date filters,
    /// and allows the user to apply these filters to the displayed events.
    /// </summary>
    public partial class FilterComponent : UserControl
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the instance of the EventsStore from the application state.
        /// This store is used for managing and filtering events.
        /// </summary>
        private EventsStore EventsStore => AppStore.Instance.EventsStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterComponent"/> class.
        /// Registers a callback to handle new event categories.
        /// </summary>
        public FilterComponent()
        {
            InitializeComponent();
            EventsStore.OnEventCategoryAdded += this.AddNewCategory;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the filter button. Toggles the visibility of the filter popup.
        /// When the popup is opened, it populates the categories and date comboboxes with appropriate filter options.
        /// </summary>
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = !FilterPopup.IsOpen;  // Toggle popup visibility

            // Populate categories combobox
            this.PopulateCategories();

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
            var existingDateType = EventsStore.FilterStore.DateType;
            DateComboBox.Items.Clear();

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
        /// Populates the CategoriesComboBox with available event categories.
        /// If a filter is already applied, it ensures that category is listed first.
        /// </summary>
        private void PopulateCategories()
        {
            var eventCategories = EventsStore.EventCategories.ToList();
            CategoriesComboBox.Items.Clear();

            // Checking if a filter is currently applied
            var existingCategoryText = EventsStore.FilterStore.CategoryText;
            if (existingCategoryText == string.Empty)
            {
                CategoriesComboBox.Items.Add("All Categories");
            }
            else
            {
                CategoriesComboBox.Items.Add(existingCategoryText);
                CategoriesComboBox.Items.Add("All Categories");
                // Remove it so it's not added twice
                eventCategories.Remove(existingCategoryText);
            }
            // Setting initial selection
            CategoriesComboBox.SelectedIndex = 0;

            foreach (var category in eventCategories)
            {
                CategoriesComboBox.Items.Add($"{category}");
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Adds a new category to the CategoriesComboBox if it doesn't already exist.
        /// This method is triggered when a new category is added to the EventsStore.
        /// </summary>
        /// <param name="category">The category to add.</param>
        private void AddNewCategory(string category)
        {
            var alreadyExists = false;
            foreach (var item in CategoriesComboBox.Items)
            {
                if (item as string == category)
                {
                    alreadyExists = true; break;
                }
            }

            if (!alreadyExists)
            {
                CategoriesComboBox.Items.Add(category);
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the apply filter button.
        /// Updates the category and date filters based on the user's selection,
        /// and triggers the event filtering process.
        /// </summary>
        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var dateText = DateComboBox.Text;
            var categoryText = CategoriesComboBox.Text;

            if (categoryText == "All Categories")
            {
                EventsStore.FilterStore.CategoryText = string.Empty;
            }
            else
            {
                EventsStore.FilterStore.CategoryText = categoryText;
                EventsStore.RecommendationService.AddTerm(Services.RecommendationTermType.Category, categoryText);
            }

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
                EventsStore.RecommendationService.AddTerm(Services.RecommendationTermType.Date, dateType.ToString());
            }
            EventsStore.FilterStore.DateType = dateType;

            EventsStore.FilterStore.OnFilterEvents?.Invoke();
        }
    }
}
//---------------------------------------EOF-------------------------------------------