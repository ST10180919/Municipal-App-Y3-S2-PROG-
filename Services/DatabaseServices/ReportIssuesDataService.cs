using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Services.DatabaseServices
{
    /*
     * NOTE: None of this code is implemented in the app as database functionality
     * wasn't necessary for this part. The database and ADO.NET entity data model 
     * included in this project are used for the classes they create and nothing
     * more (yet).
     */

    internal class ReportIssuesDataService
    {
        private MunicipalDatabaseEntities Entity;

        public async Task AddIssueReportAsync(ISSUE_REPORT report)
        {
            using (this.Entity = new MunicipalDatabaseEntities())
            {
                this.Entity.ISSUE_REPORT.Add(report);
                await this.Entity.SaveChangesAsync();
            }
        }

        public void AddIssueReport(ISSUE_REPORT report)
        {
            using (this.Entity = new MunicipalDatabaseEntities())
            {
                Console.WriteLine(this.Entity.Database.Connection.ConnectionString);
                this.Entity.Database.Log = Console.WriteLine;
                this.Entity.ISSUE_REPORT.Add(report);
                var entry = this.Entity.Entry(report);
                Console.WriteLine(entry.State);

                int affectedRows = this.Entity.SaveChanges();
                Console.WriteLine($"Affected Rows: {affectedRows}");
            }
        }

        public List<ISSUE_REPORT> GetIssueReports()
        {
            using (this.Entity = new MunicipalDatabaseEntities())
            {
                Console.WriteLine(this.Entity.Database.Connection.ConnectionString);
                return this.Entity.ISSUE_REPORT.ToList();
            }
        }
    }
}
