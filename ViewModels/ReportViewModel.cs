using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.ViewModels
{
	enum IssueCategory
	{
		Sanitation,
		Roads,
		Utilities
	}

    internal class ReportViewModel : ViewModelBase, INotifyDataErrorInfo
    {
		private string _location;
		public string Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
				OnPropertyChanged(nameof(Location));
			}
		}

		private string _category;
		public string Category
		{
			get
			{
				return _category;
			}
			set
			{
				_category = value;
				OnPropertyChanged(nameof(Category));
			}
		}

        public List<string> CategoryList { get; set; } = new List<string>
		{
			"Sanitation",
			"Roads",
			"Utilities"
		};

        private string _description;
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

		private string _solution;
		public string Solution
		{
			get
			{
				return _solution;
			}
			set
			{
				_solution = value;
				OnPropertyChanged(nameof(Solution));
			}
		}

		private ObservableCollection<AttachmentViewModel> _attachments;
		public ObservableCollection<AttachmentViewModel> Attachments
		{
			get
			{
				return _attachments;
			}
			set
			{
				_attachments = value;
				OnPropertyChanged(nameof(Attachments));
			}
		}

		public int AttachmentCount
		{
			get
			{
				return _attachments.Count;
			}
		}

		public ReportViewModel()
        {
			this._location = string.Empty;
			this._category = string.Empty;
			this._description = string.Empty;
			this._solution = string.Empty;
			this._attachments = new ObservableCollection<AttachmentViewModel>();
        }

        public ReportViewModel(ISSUE_REPORT report)
        {
			this._location = report.LOCATION;
			this._category = report.CATEGORY;
			this._description = report.DESCRIPTION;
			this._solution = report.DESCRIPTION;

			this._attachments = new ObservableCollection<AttachmentViewModel>();
			foreach (var attachment in report.ATTACHMENTs)
			{
				this._attachments.Add(new AttachmentViewModel(attachment));
			}
        }

		public ISSUE_REPORT convertToIssueReport()
		{
			var attachments = new List<ATTACHMENT>();
			foreach (var attachment in this.Attachments)
			{
				attachments.Add(attachment.ConvertToAttachment());
			}

			var convertedObj = new ISSUE_REPORT()
			{
				LOCATION = this._location,
				CATEGORY = this._category,
				DESCRIPTION = this._description,
				SOLUTION = this._solution,
				ATTACHMENTs = attachments
			};
			return convertedObj;
		}

		public void NotifyUIofAttachmentsChange()
		{
            OnPropertyChanged(nameof(Attachments));
            OnPropertyChanged(nameof(AttachmentCount));
        }

		// Error Handling
		private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

		public bool HasErrors => this._propertyErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		private void OnErrorsChanged(string propertyName)
		{
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

		//private string _locationErrors;
		//public string LocationErrors
		//{
		//	get
		//	{
		//		return _locationErrors;
		//	}
		//	set
		//	{
		//		_locationErrors = value;
		//		OnPropertyChanged(nameof(LocationErrors));
		//	}
		//}

		public void AddError(string propertyName, string errorMessage)
        {
            if (!this._propertyErrors.ContainsKey(propertyName))
            {
				//if (propertyName == "Location")
				//{
				//	this.LocationErrors += errorMessage;
				//}
				this._propertyErrors.Add(propertyName, new List<string>());
            }

            this._propertyErrors[propertyName].Add(errorMessage);
            this.OnErrorsChanged(propertyName);
        }

        public void ClearErrors(string propertyName)
        {
            if (this._propertyErrors.Remove(propertyName))
            {
                this.OnErrorsChanged(propertyName);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            _propertyErrors.TryGetValue(propertyName, out var errors);
            return errors;
        }
    }
}
