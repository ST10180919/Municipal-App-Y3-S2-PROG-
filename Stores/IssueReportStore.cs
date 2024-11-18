using Municipal_App.Models;
using Municipal_App.Services.DatabaseServices;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;

namespace Municipal_App.Stores
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Stores all the issue reports submitted by users and provides methods to manage and retrieve them.
    /// A Binary Search Tree was used here to ensure really fast search times when searching
    /// (since the whole tree is searched whenever the user enters a character into the search bar)
    /// </summary>
    internal class IssueReportStore
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// A sorted dictionary that uses a Binary Search Tree to store issue reports.
        /// Proof that the SortedDictionary in C# uses a binary search tree: 
        /// https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2?view=netframework-4.8
        /// </summary>
        private SortedDictionary<string, ISSUE_REPORT> issueReportTree;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Dictionary of graph nodes representing relationships between issue reports and categories.
        /// </summary>
        private Dictionary<string, GraphNode> graphNodes;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// A max heap to track and prioritize issue reports based on timestamps.
        /// </summary>
        private MaxHeap<HeapItem> RequestsHeap;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The current search term used to filter issue reports.
        /// </summary>
        private string _searchTerm;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the search term used to filter issue reports.
        /// When set, triggers the <see cref="SearchTermChanged"/> action.
        /// </summary>
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; OnSearchTermChanged(); }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Action invoked when the <see cref="SearchTerm"/> changes.
        /// </summary>
        public Action SearchTermChanged { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Action invoked when the reports are loaded into the store.
        /// </summary>
        public Action ReportsLoaded { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="IssueReportStore"/> class.
        /// </summary>
        public IssueReportStore()
        {
            issueReportTree = new SortedDictionary<string, ISSUE_REPORT>();
            graphNodes = new Dictionary<string, GraphNode>();
            RequestsHeap = new MaxHeap<HeapItem>();

            this.InitializeReports();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Asynchronously initializes the store by loading issue reports from the database.
        /// </summary>
        private async void InitializeReports()
        {
            var initialReports = await ReportIssuesDataService.GetIssueReports();

            foreach (var report in initialReports)
            {
                this.AddIssueReport(report);
            }
            this.ReportsLoaded?.Invoke();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Adds a new issue report to the store.
        /// </summary>
        /// <param name="issueReport">The issue report to add.</param>
        public void AddIssueReport(ISSUE_REPORT issueReport)
        {
            // Add to sorted dictionary
            issueReportTree[issueReport.IDENTIFIER] = issueReport;

            // Add to Graph
            AddToGraph(issueReport);

            // Add to Heap using now as the timestamp
            RequestsHeap.Insert(new HeapItem {Timestamp = DateTime.Now, Data = issueReport });
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Adds the given issue report to the graph structure, establishing category relationships.
        /// </summary>
        /// <param name="issueReport">The issue report to add to the graph.</param>
        private void AddToGraph(ISSUE_REPORT issueReport)
        {
            // Ensure Category Node Exists
            if (!graphNodes.ContainsKey(issueReport.CATEGORY))
            {
                graphNodes[issueReport.CATEGORY] = new GraphNode(issueReport.CATEGORY, "Category");
            }

            // Create Service Request Node
            var requestNode = new GraphNode(issueReport.IDENTIFIER, "ServiceRequest");
            graphNodes[issueReport.IDENTIFIER] = requestNode;

            // Connect Service Request to Category
            requestNode.Edges["Category"] = new GraphEdge(graphNodes[issueReport.CATEGORY], "BelongsTo");

            // Connect Category to Service Request
            graphNodes[issueReport.CATEGORY].Edges[issueReport.IDENTIFIER] = new GraphEdge(requestNode, "HasRequest");
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Searches for issue reports within a specific category that match the current search term.
        /// </summary>
        /// <param name="categoryIdentifier">The identifier of the category to filter by.</param>
        /// <returns>A list of issue reports that match the search term and category.</returns>
        public List<ISSUE_REPORT> FilterAndSearch(string categoryIdentifier)
        {
            var results = new List<ISSUE_REPORT>();

            // Get the category node
            if (!graphNodes.TryGetValue(categoryIdentifier, out GraphNode categoryNode))
            {
                return results; // Category not found
            }

            // Perform BFS or DFS starting from the category node
            var visited = new HashSet<GraphNode>();
            var stack = new Stack<GraphNode>();
            stack.Push(categoryNode);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();

                if (!visited.Contains(currentNode))
                {
                    visited.Add(currentNode);

                    if (currentNode.Type == "ServiceRequest")
                    {
                        if (issueReportTree.TryGetValue(currentNode.Identifier, out ISSUE_REPORT report))
                        {
                            // Apply search term filter
                            if (IsMatch(report, this.SearchTerm) || SearchTerm == null || SearchTerm == string.Empty)
                            {
                                results.Add(report);
                            }
                        }
                    }

                    // Traverse edges
                    foreach (var edge in currentNode.Edges.Values)
                    {
                        if (edge.EdgeType == "HasRequest" || edge.EdgeType == "BelongsTo")
                        {
                            stack.Push(edge.TargetNode);
                        }
                    }
                }
            }

            return results;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Searches for issue reports that match the current <see cref="SearchTerm"/>.
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

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Determines whether an issue report matches the specified search term.
        /// </summary>
        private bool IsMatch(ISSUE_REPORT report, string searchTerm)
        {
            if (report == null || string.IsNullOrEmpty(searchTerm))
                return false;

            searchTerm = searchTerm.ToLower();

            // Check if the IDENTIFIER field contains the search term
            return (report.IDENTIFIER != null && report.IDENTIFIER.ToLower().Contains(searchTerm));
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Retrieves all issue reports stored in the store.
        /// </summary>
        public List<ISSUE_REPORT> GetAllIssueReports()
        {
            return new List<ISSUE_REPORT>(issueReportTree.Values);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the total number of issue reports in the store.
        /// </summary>
        public int GetNumberofReports()
        {
            return issueReportTree.Count;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the total number of issue reports in the store.
        /// </summary>
        public HeapItem GetMostRecentReport()
        {
            var heapItem =  RequestsHeap.Count > 0 ? RequestsHeap.Peek() : null;
            return heapItem;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Invokes the <see cref="SearchTermChanged"/> action when the search term changes.
        /// </summary>
        private void OnSearchTermChanged()
        {
            SearchTermChanged?.Invoke();
        }
    }
}
//---------------------------------------EOF-------------------------------------------