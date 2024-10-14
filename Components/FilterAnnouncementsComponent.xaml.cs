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
    /// <summary>
    /// Interaction logic for FilterAnnouncementsComponent.xaml
    /// </summary>
    public partial class FilterAnnouncementsComponent : UserControl
    {
        private AnnouncementsStore AnnouncementsStore => AppStore.Instance.AnnouncementsStore;

        public FilterAnnouncementsComponent()
        {
            InitializeComponent();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterPopup.IsOpen = !FilterPopup.IsOpen;  // Toggle popup visibility

            // Populate date combobox
            this.PopulateDates();
        }

        private void PopulateDates()
        {
            var existingDateType = AnnouncementsStore.FilterStore.DateType;
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

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var dateText = DateComboBox.Text;

            if (dateText.Equals("Any Time"))
            {
                AnnouncementsStore.FilterStore.DateType = DateType.AnyTime;
            }
            else if (dateText.Equals("Today"))
            {
                AnnouncementsStore.FilterStore.DateType = DateType.Today;
            }
            else if (dateText.Equals("This Week"))
            {
                AnnouncementsStore.FilterStore.DateType = DateType.ThisWeek;
            }
            else if (dateText.Equals("This Month"))
            {
                AnnouncementsStore.FilterStore.DateType = DateType.ThisMonth;
            }

            AnnouncementsStore.FilterStore.OnFilterAnnouncements?.Invoke();
        }
    }
}
