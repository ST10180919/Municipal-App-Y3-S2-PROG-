using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Municipal_App.ViewModels
{
    internal class AnnouncementViewModel : ViewModelBase
    {
        private string _title;
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The title of the event
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private BitmapImage _image;
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The image corresponding to the event
        /// </summary>
        public BitmapImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private string _date;
        //-----------------------------------------------------------------------------
        /// <summary>
        /// A string containing the date (or dates) for which the event takes place
        /// </summary>
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private string _description;
        //-----------------------------------------------------------------------------
        /// <summary>
        /// A string containing te description of the announcement
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private bool _isRecommended = false;
        public bool IsRecommended
        {
            get
            {
                return _isRecommended;
            }
            set
            {
                _isRecommended = value;
                OnPropertyChanged(nameof(IsRecommended));
            }
        }
    }
}
