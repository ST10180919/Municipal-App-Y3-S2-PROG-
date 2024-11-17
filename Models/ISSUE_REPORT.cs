using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    public class ISSUE_REPORT
    {
        [Key]
        public string IDENTIFIER { get; set; }

        public string LOCATION { get; set; }

        public string DESCRIPTION { get; set; }

        [StringLength(100)]
        public string CATEGORY { get; set; }

        public string SOLUTION { get; set; }

        [Required]
        public string STATUS_STRING { get; set; }

        public virtual ICollection<ATTACHMENT> ATTACHMENTS { get; set; }

        public ISSUE_REPORT()
        {
            ATTACHMENTS = new HashSet<ATTACHMENT>();
        }
    }
}