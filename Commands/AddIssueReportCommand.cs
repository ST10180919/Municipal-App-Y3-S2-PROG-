using Municipal_App.Services;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Commands
{
    internal class AddIssueReportCommand : CommandBase
    {
        private BannerMessageStore _bannerMessageStore => AppStore.Instance.BannerMessageStore;

        private readonly ReportViewModel _reportToBeAdded;
        public AddIssueReportCommand(ReportViewModel report) 
        { 
            this._reportToBeAdded = report;
        }

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

            // Checking Solution
            var Solution = this._reportToBeAdded.Solution;
            if (Solution == string.Empty)
            {
                this._reportToBeAdded.AddError(nameof(Solution), "This Field cannot be empty");
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
