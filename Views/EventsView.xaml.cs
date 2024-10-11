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
using static System.Net.Mime.MediaTypeNames;

namespace Municipal_App.Views
{
    /// <summary>
    /// Interaction logic for EventsView.xaml
    /// </summary>
    public partial class EventsView : UserControl
    {
        public EventsView()
        {
            InitializeComponent();
        }

        private void LatoText_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var text = textBlock.Text;
            var spacing = 7;
            textBlock.Inlines.Clear();  // Clear any existing text in the TextBlock

            // Loop through each character in the string
            for (int i = 0; i < text.Length; i++)
            {
                // Create a new Run for each character
                Run run = new Run(text[i].ToString());

                // Add the character to the TextBlock
                textBlock.Inlines.Add(run);

                // If it's not the last character, add a space Run to simulate spacing
                if (i < text.Length - 1)
                {
                    // Add spacing by adding a space character with adjusted FontSize or Width
                    Run space = new Run(" ");
                    space.FontSize = textBlock.FontSize + spacing;  // Increase space size
                    textBlock.Inlines.Add(space);
                }
            }
        }
    }
}
