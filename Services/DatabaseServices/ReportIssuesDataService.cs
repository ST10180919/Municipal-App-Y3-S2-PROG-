using Municipal_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //private MunicipalDatabaseEntities Entity;

        //public async Task AddIssueReportAsync(ISSUE_REPORT report)
        //{
        //    using (this.Entity = new MunicipalDatabaseEntities())
        //    {
        //        this.Entity.ISSUE_REPORT.Add(report);
        //        await this.Entity.SaveChangesAsync();
        //    }
        //}

        //public void AddIssueReport(ISSUE_REPORT report)
        //{
        //    using (this.Entity = new MunicipalDatabaseEntities())
        //    {
        //        Console.WriteLine(this.Entity.Database.Connection.ConnectionString);
        //        this.Entity.Database.Log = Console.WriteLine;
        //        this.Entity.ISSUE_REPORT.Add(report);
        //        var entry = this.Entity.Entry(report);
        //        Console.WriteLine(entry.State);

        //        int affectedRows = this.Entity.SaveChanges();
        //        Console.WriteLine($"Affected Rows: {affectedRows}");
        //    }
        //}

        //public List<ISSUE_REPORT> GetIssueReports()
        //{
        //    using (this.Entity = new MunicipalDatabaseEntities())
        //    {
        //        Console.WriteLine(this.Entity.Database.Connection.ConnectionString);
        //        return this.Entity.ISSUE_REPORT.ToList();
        //    }
        //}

        public static void TestDatabase()
        {
            using (var context = new MunicipalDbContext())
            {
                // Create a new issue report
                var issueReport = new ISSUE_REPORT
                {
                    IDENTIFIER = Guid.NewGuid().ToString(),
                    LOCATION = "123 Main St",
                    DESCRIPTION = "Pothole in the road",
                    CATEGORY = "Roads",
                    SOLUTION = null,
                    STATUS_STRING = "Open"
                };

                context.IssueReports.Add(issueReport);
                context.SaveChanges();

                // Retrieve the issue report
                var report = context.IssueReports.FirstOrDefault(i => i.IDENTIFIER == issueReport.IDENTIFIER);
                Console.WriteLine($"Issue Report: {report.DESCRIPTION} at {report.LOCATION}");

                //// Update the issue report
                //report.STATUS_STRING = "In Progress";
                //context.SaveChanges();

                //// Delete the issue report
                //context.IssueReports.Remove(report);
                //context.SaveChanges();
            }
        }
    }
}
