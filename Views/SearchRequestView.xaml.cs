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

namespace Municipal_App.Views
{
    /// <summary>
    /// Interaction logic for SearchRequestView.xaml
    /// </summary>
    public partial class SearchRequestView : UserControl
    {
        public SearchRequestView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.CaretBrush = (Brush)FindResource("TextWhiteBrush");
        }
    }
}
