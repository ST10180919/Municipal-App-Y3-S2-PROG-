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
    internal class MunicipalEventViewModel : ViewModelBase
    {
		private string _title;
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
