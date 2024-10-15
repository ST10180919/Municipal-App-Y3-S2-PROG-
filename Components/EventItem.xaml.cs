using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Municipal_App.Components
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// A user control for displaying individual event items. This control manages
    /// custom rendering and spacing adjustments for text elements, specifically for
    /// text using the Lato font.
    /// </summary>
    public partial class EventItem : UserControl
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="EventItem"/> class.
        /// </summary>
        public EventItem()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// This wierd method manually adjusts the letterspacing of a textblock,
        /// because aparently that's not built in in WPF
        /// </summary>
        /// <param name="text">The text string whose spacing is to be adjusted.
        /// </param>
        /// <param name="spacing">The amount of space to add between each character.
        /// </param>
        /// <param name="textBlock">The TextBlock where the text will be rendered with 
        /// spacing.</param>
        private void AdjustLatoLetterSpacing(string text, double spacing, TextBlock textBlock)
        {
            textBlock.Inlines.Clear();

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
                    space.FontSize = textBlock.FontSize + spacing;
                    textBlock.Inlines.Add(space);
                }
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Event handler for the Loaded event of the TextBlock. When the TextBlock 
        /// is loaded, this method is called to adjust its letter spacing using the Lato 
        /// font.
        /// </summary>
        /// <param name="sender">The TextBlock that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void LatoText_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            AdjustLatoLetterSpacing(textBlock.Text, 7, textBlock);
        }
    }
}
//---------------------------------------EOF-------------------------------------------