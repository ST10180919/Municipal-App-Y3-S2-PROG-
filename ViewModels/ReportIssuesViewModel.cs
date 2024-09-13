using Municipal_App.Commands;
using Municipal_App.Services;
using Municipal_App.Services.DatabaseServices;
using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// ViewModel for the IssuesView
    /// </summary>
    internal class ReportIssuesViewModel : ViewModelBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Backing field for the FeedbackLabel
        /// </summary>
        private string _feedbackLabel;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Feedback Label for this ViewModel, exposed for the UI to bind to
        /// </summary>
        public string FeedbackLabel
        {
            get
            {
                return _feedbackLabel;
            }
            set
            {
                _feedbackLabel = value;
                OnPropertyChanged(nameof(FeedbackLabel));
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to navigate back to the MainViewModel
        /// </summary>
        public ICommand MainViewNavCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// ViewModel holding the inputs for the issue report from the user for this 
        /// page
        /// </summary>
        public ReportViewModel IssueReport { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to add the issue report created by the user
        /// </summary>
        public AddIssueReportCommand AddIssueReportCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to promt the user to choose files to attach to their issue
        /// report
        /// </summary>
        public ChooseFileCommand ChooseFileCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Command used to remove an attached file from the issue report
        /// </summary>
        public RemoveAttachmentCommand RemoveAttachmentCommand { get; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a ReportIssuesViewModel
        /// </summary>
        public ReportIssuesViewModel()
        {
            this.FeedbackLabel = "How can we improve our Issue Reporting Service?";
            this.IssueReport = new ReportViewModel();

            // Setting up back naviagtion
            var navigationStore = AppStore.Instance.NavigationStore;
            this.MainViewNavCommand = new NavCommand(new NavigationService(navigationStore, CreateLandingViewModel));

            // Initialising Commands
            this.RemoveAttachmentCommand = new RemoveAttachmentCommand(this.IssueReport.Attachments);
            this.ChooseFileCommand = new ChooseFileCommand(this.IssueReport);
            this.AddIssueReportCommand = new AddIssueReportCommand(this.IssueReport);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new LandingViewModel
        /// </summary>
        /// <returns></returns>
        public LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel();
        }
    }
}
//---------------------------------------EOF-------------------------------------------