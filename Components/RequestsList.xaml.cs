using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for RequestsList.xaml
    /// </summary>
    public partial class RequestsList : UserControl
    {
        public ObservableCollection<RequestStatus> requestList;
        public RequestsList()
        {
            InitializeComponent();

            requestList = new ObservableCollection<RequestStatus>
            {
                new RequestStatus {Identifier = "#REQ-995108", StatusString = "Pending", StatusColorBrush = new SolidColorBrush(new Color {R = 255, G = 221, B = 0, A = 255})}
            };

            MyList.ItemsSource = requestList;
        }
    }

    public class RequestStatus
    {
        public string Identifier { get; set; }

        public string StatusString { get; set; }

        public SolidColorBrush StatusColorBrush { get; set; }
    }
}
