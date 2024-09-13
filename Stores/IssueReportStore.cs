using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Stores
{
    internal class IssueReportStore
    {
        public List<ISSUE_REPORT> IssueReportList { get; private set; }

        public IssueReportStore()
        {
            this.IssueReportList = new List<ISSUE_REPORT>();
        }

        public void AddIssueReport(ISSUE_REPORT issueReport)
        {
            this.IssueReportList.Add(issueReport);
        }
    }
}
