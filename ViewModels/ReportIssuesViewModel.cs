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
    internal class ReportIssuesViewModel : ViewModelBase
    {
        private string _feedbackLabel;
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

        public ICommand MainViewNavCommand { get; }

        public ReportViewModel IssueReport { get; set; }

        public AddIssueReportCommand AddIssueReportCommand { get; }

        public ChooseFileCommand ChooseFileCommand { get; }

        public RemoveAttachmentCommand RemoveAttachmentCommand { get; }
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

        public LandingViewModel CreateLandingViewModel()
        {
            return new LandingViewModel();
        }
    }
}
