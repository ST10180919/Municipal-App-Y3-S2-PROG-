using Municipal_App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

        public static async Task AddIssueReportAsync(ISSUE_REPORT report)
        {
            using (var context = new MunicipalDbContext())
            {
                context.IssueReports.AddOrUpdate(report);
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while saving changes:");
                    Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception:");
                        Console.WriteLine(ex.InnerException.ToString());
                    }
                    throw;
                }
            }
        }

        public static async Task<List<ISSUE_REPORT>> GetIssueReports()
        {
            using (var context = new MunicipalDbContext())
            {
                return await context.IssueReports.Include(r => r.ATTACHMENTS).ToListAsync();
            }
        }

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

                // Update the issue report
                report.STATUS_STRING = "In Progress";
                context.SaveChanges();

                // Delete the issue report
                context.IssueReports.Remove(report);
                context.SaveChanges();
            }
        }
    }
}
