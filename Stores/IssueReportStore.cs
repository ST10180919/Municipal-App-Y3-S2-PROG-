using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Stores
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// This class stores the instance of each issue report submitted by a user
    /// </summary>
    internal class IssueReportStore
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The master list of ISSUE_REPORTs
        /// </summary>
        public List<ISSUE_REPORT> IssueReportList { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new IssueReportStore
        /// </summary>
        public IssueReportStore()
        {
            this.IssueReportList = new List<ISSUE_REPORT>();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Adds an ISSUE_REPORT to the IssueReportList
        /// </summary>
        /// <param name="issueReport"> ISSUE_REPORT to be added </param>
        public void AddIssueReport(ISSUE_REPORT issueReport)
        {
            this.IssueReportList.Add(issueReport);
        }
    }
}
//---------------------------------------EOF-------------------------------------------