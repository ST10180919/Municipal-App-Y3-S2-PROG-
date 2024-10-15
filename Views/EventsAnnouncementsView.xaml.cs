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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Municipal_App.Views
{
    /// <summary>
    /// Interaction logic for EventsAnnouncementsViewModel.xaml
    /// </summary>
    public partial class EventsAnnouncemetnsView : UserControl
    {
        public EventsAnnouncemetnsView()
        {
            InitializeComponent();
        }

        private void ContentSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null && comboBox.SelectedItem is ComboBoxItem)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem;

                switch (selectedItem.Content)
                {
                    case "Events Only":
                        AnnouncementsSection.Visibility = Visibility.Collapsed;
                        EventsSection.Visibility = Visibility.Visible;
                        break;

                    case "Announcements Only":
                        EventsSection.Visibility = Visibility.Collapsed;
                        AnnouncementsSection.Visibility = Visibility.Visible;
                        break;

                    case "Events and Announcements":
                        EventsSection.Visibility = Visibility.Visible;
                        AnnouncementsSection.Visibility = Visibility.Visible;
                        break;

                    default:
                        break;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.CaretBrush = (Brush)FindResource("TextWhiteBrush");
        }
    }
}
