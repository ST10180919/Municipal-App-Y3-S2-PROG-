using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;

namespace Municipal_App.Stores
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Stores all the issue reports submitted by users and provides methods to manage and retrieve them.
    /// A Red-Black tree was used here to ensure really fast search times when searching
    /// (since the whole tree is searched whenever the user enters a character into the search bar)
    /// </summary>
    internal class IssueReportStore
    {
        /// <summary>
        /// A sorted dictionary that uses a Red-Black Tree to store issue reports.
        /// </summary>
        private SortedDictionary<string, ISSUE_REPORT> issueReportTree;

        /// <summary>
        /// The current search term used to filter issue reports.
        /// </summary>
        private string _searchTerm;

        /// <summary>
        /// Gets or sets the search term used to filter issue reports.
        /// When set, triggers the <see cref="SearchTermChanged"/> action.
        /// </summary>
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; OnSearchTermChanged(); }
        }

        /// <summary>
        /// Action invoked when the <see cref="SearchTerm"/> changes.
        /// </summary>
        public Action SearchTermChanged { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueReportStore"/> class.
        /// </summary>
        public IssueReportStore()
        {
            issueReportTree = new SortedDictionary<string, ISSUE_REPORT>();
        }

        /// <summary>
        /// Adds a new issue report to the store.
        /// The issue reports are stored in a SortedDictionary, which uses a Red-Black Tree.
        /// This tree structure ensures that the issue reports are kept in sorted order based on their identifiers,
        /// allowing for efficient search and retrieval operations, which is beneficial for managing a large number of reports.
        /// </summary>
        /// <param name="issueReport">The issue report to add.</param>
        public void AddIssueReport(ISSUE_REPORT issueReport)
        {
            issueReportTree[issueReport.IDENTIFIER] = issueReport;
        }

        /// <summary>
        /// Searches for issue reports that match the current <see cref="SearchTerm"/>.
        /// If the search term is null or empty, all issue reports are returned.
        /// </summary>
        /// <returns>A list of issue reports that match the search term.</returns>
        public List<ISSUE_REPORT> Search()
        {
            List<ISSUE_REPORT> results = new List<ISSUE_REPORT>();

            if (string.IsNullOrEmpty(SearchTerm))
            {
                // If SearchTerm is null or empty, return all reports
                return GetAllIssueReports();
            }

            foreach (var report in issueReportTree.Values)
            {
                if (IsMatch(report, SearchTerm))
                {
                    results.Add(report);
                }
            }

            return results;
        }

        /// <summary>
        /// Determines whether the specified issue report matches the given search term.
        /// </summary>
        /// <param name="report">The issue report to check.</param>
        /// <param name="searchTerm">The search term to match against.</param>
        /// <returns><c>true</c> if the report matches the search term; otherwise, <c>false</c>.</returns>
        private bool IsMatch(ISSUE_REPORT report, string searchTerm)
        {
            if (report == null || string.IsNullOrEmpty(searchTerm))
                return false;

            searchTerm = searchTerm.ToLower();

            // Check if the IDENTIFIER field contains the search term
            return (report.IDENTIFIER != null && report.IDENTIFIER.ToLower().Contains(searchTerm));
        }

        /// <summary>
        /// Retrieves all issue reports stored in the store.
        /// The reports are returned in sorted order based on their identifiers due to the underlying Red-Black Tree structure.
        /// This efficient retrieval is helpful for displaying reports to the user in an organized manner.
        /// </summary>
        /// <returns>A list of all issue reports.</returns>
        public List<ISSUE_REPORT> GetAllIssueReports()
        {
            return new List<ISSUE_REPORT>(issueReportTree.Values);
        }

        /// <summary>
        /// Gets the total number of issue reports stored.
        /// </summary>
        /// <returns>The number of issue reports.</returns>
        public int GetNumberofReports()
        {
            return issueReportTree.Count;
        }

        /// <summary>
        /// Invokes the <see cref="SearchTermChanged"/> action when the <see cref="SearchTerm"/> changes.
        /// </summary>
        private void OnSearchTermChanged()
        {
            SearchTermChanged?.Invoke();
        }
    }
}
//---------------------------------------EOF-------------------------------------------