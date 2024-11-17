using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    public class ATTACHMENT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(100)]
        public string NAME_OF_FILE { get; set; }

        public byte[] FILE_BINARY { get; set; }

        // Foreign key
        public string ISSUE_REPORT_ID { get; set; }

        [ForeignKey("ISSUE_REPORT_ID")]
        public virtual ISSUE_REPORT ISSUE_REPORT { get; set; }
    }
}
