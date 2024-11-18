using Municipal_App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace Municipal_App.Services.DatabaseServices
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Provides data operations for managing issue reports within the municipal application.
    /// </summary>
    internal class ReportIssuesDataService
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Asynchronously adds or updates an issue report in the database.
        /// </summary>
        /// <param name="report">The <see cref="ISSUE_REPORT"/> to add or update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while saving changes.</exception>
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

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Asynchronously retrieves all issue reports from the database, including their attachments.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of <see cref="ISSUE_REPORT"/> objects.
        /// </returns>
        public static async Task<List<ISSUE_REPORT>> GetIssueReports()
        {
            using (var context = new MunicipalDbContext())
            {
                return await context.IssueReports.Include(r => r.ATTACHMENTS).ToListAsync();
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Tests the database operations by creating, retrieving, updating, and deleting an issue report.
        /// </summary>
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
//---------------------------------------EOF-------------------------------------------