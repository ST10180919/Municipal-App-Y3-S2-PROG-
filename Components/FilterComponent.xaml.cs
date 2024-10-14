using Municipal_App.Stores;
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
    /// <summary>
    /// Interaction logic for FilterComponent.xaml
    /// </summary>
    public partial class FilterComponent : UserControl
    {
        private EventsStore EventsStore => AppStore.Instance.EventsStore;
        public FilterComponent()
        {
            InitializeComponent();
            EventsStore.OnEventCategoryAdded += this.AddNewCategory;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = !FilterPopup.IsOpen;  // Toggle popup visibility

            // Populate categories combobox
            this.PopulateCategories();

            // Populate date combobox
            this.PopulateDates();
        }

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
            }

            if (dateText.Equals("Any Time"))
            {
                EventsStore.FilterStore.DateType = DateType.AnyTime;
            }
            else if (dateText.Equals("Today"))
            {
                EventsStore.FilterStore.DateType = DateType.Today;
            }
            else if (dateText.Equals("This Week"))
            {
                EventsStore.FilterStore.DateType = DateType.ThisWeek;
            }
            else if (dateText.Equals("This Month"))
            {
                EventsStore.FilterStore.DateType = DateType.ThisMonth;
            }

            EventsStore.FilterStore.OnFilterEvents?.Invoke();
        }
    }
}
