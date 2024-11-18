using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents an issue report containing details about a reported issue, including location, description, and status.
    /// </summary>
    public class ISSUE_REPORT
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Unique identifier for the issue report.
        /// </summary>
        [Key]
        public string IDENTIFIER { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Location of the reported issue.
        /// </summary>
        public string LOCATION { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Description of the reported issue.
        /// </summary>
        public string DESCRIPTION { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Category of the issue report (e.g., Roads, Sanitation, Utilities).
        /// </summary>
        [StringLength(100)]
        public string CATEGORY { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Proposed or applied solution for the issue.
        /// </summary>
        public string SOLUTION { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Status of the issue as a string (e.g., Pending, Resolved).
        /// </summary>
        [Required]
        public string STATUS_STRING { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Collection of attachments associated with the issue report.
        /// </summary>
        public virtual ICollection<ATTACHMENT> ATTACHMENTS { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Collection of attachments associated with the issue report.
        /// </summary>
        public ISSUE_REPORT()
        {
            ATTACHMENTS = new HashSet<ATTACHMENT>();
        }
    }
}
//---------------------------------------EOF-------------------------------------------