using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents an attachment associated with an issue report.
    /// </summary>
    public class ATTACHMENT
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Unique identifier for the attachment.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Name of the file associated with the attachment.
        /// </summary>
        [StringLength(100)]
        public string NAME_OF_FILE { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Binary data of the attached file.
        /// </summary>
        public byte[] FILE_BINARY { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Foreign key for the associated issue report.
        /// </summary>
        public string ISSUE_REPORT_ID { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Navigation property for the associated issue report.
        /// </summary>
        [ForeignKey("ISSUE_REPORT_ID")]
        public virtual ISSUE_REPORT ISSUE_REPORT { get; set; }
    }
}
//---------------------------------------EOF-------------------------------------------