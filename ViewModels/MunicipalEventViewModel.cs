using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Municipal_App.ViewModels
{
	//---------------------------------------------------------------------------------
	/// <summary>
	/// Class representing a municipal event to be viewed by the user on the EventsView.
	/// 
	/// This class is a viewmodel and contains backing and exposed fields because
	/// it's intended for UI to bind directly to it, and for the UI to be updated
	/// whenever a field is set using OnPropertyChanged() from the ViewModelBase's 
	/// INotifyPropertyChanged.
	/// </summary>
    internal class MunicipalEventViewModel : ViewModelBase
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
		private string _category;
        //-----------------------------------------------------------------------------
		/// <summary>
		/// The category of the event
		/// </summary>
        public string Category
		{
			get { return _category; }
            set
			{
				_category = value;
				OnPropertyChanged(nameof(Category));
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

		private string _time;
        //-----------------------------------------------------------------------------
		/// <summary>
		/// A string containing the time the event takes place
		/// </summary>
        public string Time
		{
			get { return _time; }
            set
			{
				_time = value;
				OnPropertyChanged(nameof(Time));
			}
		}

		private string _venue;
        //-----------------------------------------------------------------------------
		/// <summary>
		/// The name of the location hosting the event
		/// </summary>
        public string Venue
		{
			get { return _venue; }
            set
			{
				_venue = value;
				OnPropertyChanged(nameof(Venue));
			}
		}

		private string _link;
        //-----------------------------------------------------------------------------
		/// <summary>
		/// A sring containing the URL to the event's official website
		/// </summary>
        public string Link
		{
			get { return _link; }
            set
			{
				_link = value;
				OnPropertyChanged(nameof(Link));
			}
		}
	}
}
//---------------------------------------EOF-------------------------------------------