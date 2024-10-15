using Municipal_App.Services;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;

namespace Municipal_App.Commands
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Command used to add Issue Reports from the user
    /// </summary>
    internal class AddIssueReportCommand : CommandBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Allows a confirmation banner to be set for user feedback
        /// </summary>
        private BannerMessageStore _bannerMessageStore => AppStore.Instance.BannerMessageStore;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// ViewModel of the report being added 
        /// </summary>
        private readonly ReportViewModel _reportToBeAdded;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new AddIssueReportCommand
        /// </summary>
        /// <param name="report"> 
        /// The ViewModel containing the report to be added
        /// </param>
        public AddIssueReportCommand(ReportViewModel report) 
        { 
            this._reportToBeAdded = report;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Code run when the command is executed
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            this.ValidateReport();

            if (!this._reportToBeAdded.HasErrors)
            {
                // Success
                this._bannerMessageStore.SetBanner("Report Successfully Submited!", BannerType.Confirmation);

                // Add to database
                AppStore.Instance.IssueReportStore.AddIssueReport(this._reportToBeAdded.convertToIssueReport());

                // Navigate back to landing
                var navigationService = new NavigationService(AppStore.Instance.NavigationStore, () => { return new LandingViewModel(); });
                navigationService.Navigate();
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Input validation for the report. Adds the errors to fields in the 
        /// ReportViewModel (which uses INotifyDataErrorInfo to notify UI)
        /// </summary>
        private void ValidateReport()
        {
            // Clearing previous errors
            this._reportToBeAdded.ClearErrors(nameof(this._reportToBeAdded.Location));
            this._reportToBeAdded.ClearErrors(nameof(this._reportToBeAdded.Category));
            this._reportToBeAdded.ClearErrors(nameof(this._reportToBeAdded.Solution));
            this._reportToBeAdded.ClearErrors(nameof(this._reportToBeAdded.Description));

            // Checking Location
            var Location = this._reportToBeAdded.Location;
            if (Location == string.Empty)
            {
                this._reportToBeAdded.AddError(nameof(this._reportToBeAdded.Location), "This Field cannot be empty");
            }

            // Checking Category
            var Category = this._reportToBeAdded.Category;
            if (Category == string.Empty)
            {
                this._reportToBeAdded.AddError(nameof(Category), "Please select a Category");
                if (Enum.TryParse<IssueCategory>(Category, false, out _))
                {
                    this._reportToBeAdded.AddError(nameof(Category), "Incorrect Category Chosen");
                }
            }

            // Checking Description
            var Description = this._reportToBeAdded.Description;
            if (Description == string.Empty)
            {
                this._reportToBeAdded.AddError(nameof(Description), "This Field cannot be empty");
            }
        }
    }
}
//---------------------------------------EOF-------------------------------------------