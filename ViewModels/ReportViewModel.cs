using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Municipal_App.ViewModels
{
	//---------------------------------------------------------------------------------
	/// <summary>
	/// Enum defining the option for category
	/// </summary>
	enum IssueCategory
	{
		Sanitation,
		Roads,
		Utilities
	}

    /* CREDITS
     * where I learnt INotifyDataErrorInfo https://www.youtube.com/watch?v=JZth6r2UXLU&t=3s
     */

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents an ISSUE_REPORT, but implements the INotifyPropertyChanged interface to 
    /// allow for more effective UI binding. Also stores all of the Info for 
    /// Any UI elements to bind to. Also implements INotifyDataErrorInfo to allow any
    /// validation errors to propogate to the UI.
    /// </summary>
    internal class ReportViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        //-----------------------------------------------------------------------------
		/// <summary>
		/// Backing field for the Location
		/// </summary>
        private string _location;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Location of the Report, exposed for the UI to bind to
		/// </summary>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Backing field for the Category
		/// </summary>
        private string _category;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Category of thier issue, chosen by the user, exposed for the UI to bind to
		/// </summary>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// List of categories for the UI to bind to (the comboBox)
		/// </summary>
        public List<string> CategoryList { get; set; } = new List<string>
		{
			"Sanitation",
			"Roads",
			"Utilities"
		};

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Backing field for Description
		/// </summary>
        private string _description;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Description of the user's issue, exposed for the UI to bind to
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Backing field for Solution
		/// </summary>
        private string _solution;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Solution, as proposed by the user
		/// </summary>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Backing field for Attachments
		/// </summary>
        private ObservableCollection<AttachmentViewModel> _attachments;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// File attachments relating to the user's issue report, exposed for the UI
		/// to bind to
		/// </summary>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Number of attachments added by the user, exposed for the UI to bind to
		/// </summary>
        public int AttachmentCount
		{
			get
			{
				return _attachments.Count;
			}
		}

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Creates an empty ReportViewModel
		/// </summary>
        public ReportViewModel()
        {
			this._location = string.Empty;
			this._category = string.Empty;
			this._description = string.Empty;
			this._solution = string.Empty;
			this._attachments = new ObservableCollection<AttachmentViewModel>();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a ReportViewModel from an ISSUE_REPORT
        /// </summary>
        /// <param name="report"> 
        /// The ISSUE_REPORT from which the ReportViewModel is created
        /// </param>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Converts this ReportViewModel to an ISSUE_REPORT
		/// </summary>
		/// <returns></returns>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Notifies the UI of changes in the Attachments and AttachmentCount fields
		/// </summary>
        public void NotifyUIofAttachmentsChange()
		{
            OnPropertyChanged(nameof(Attachments));
            OnPropertyChanged(nameof(AttachmentCount));
        }

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Dictionary used to store the errors associated with properties in this 
		/// viewModel
		/// </summary>
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Determines whether any of the properties in this viewModel have errors
		/// </summary>
        public bool HasErrors => this._propertyErrors.Any();

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Notifies UI when an error has changed
		/// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Invokes the ErrorsChanged event for a specific property
		/// </summary>
		/// <param name="propertyName"></param>
        private void OnErrorsChanged(string propertyName)
		{
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Adds an error for a property in this viewModel
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="errorMessage"></param>
        public void AddError(string propertyName, string errorMessage)
        {
            if (!this._propertyErrors.ContainsKey(propertyName))
            {
				this._propertyErrors.Add(propertyName, new List<string>());
            }

            this._propertyErrors[propertyName].Add(errorMessage);
            this.OnErrorsChanged(propertyName);
        }

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Clears all the errors for a single property in this viewModel
		/// </summary>
		/// <param name="propertyName"></param>
        public void ClearErrors(string propertyName)
        {
            if (this._propertyErrors.Remove(propertyName))
            {
                this.OnErrorsChanged(propertyName);
            }
        }

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Gets the errors for a property in this viewModel. Used by the UI to check
		/// for errors in bound properties.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
        public IEnumerable GetErrors(string propertyName)
        {
            _propertyErrors.TryGetValue(propertyName, out var errors);
            return errors;
        }
    }
}
//---------------------------------------EOF-------------------------------------------