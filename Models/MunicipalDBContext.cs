using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    public class MunicipalDbContext : DbContext
    {
        public MunicipalDbContext() : base("name=MunicipalDbContext")
        {
        }

        public virtual DbSet<ISSUE_REPORT> IssueReports { get; set; }
        public virtual DbSet<ATTACHMENT> Attachments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Map Attachment entity to ATTACHMENT table
            modelBuilder.Entity<ATTACHMENT>().ToTable("ATTACHMENT");

            // Map ISSUE_REPORT entity to ISSUE_REPORT table (if necessary)
            modelBuilder.Entity<ISSUE_REPORT>().ToTable("ISSUE_REPORT");

            // Configure relationships and other mappings
            modelBuilder.Entity<ATTACHMENT>()
                .HasRequired(a => a.ISSUE_REPORT)
                .WithMany(r => r.ATTACHMENTS)
                .HasForeignKey(a => a.ISSUE_REPORT_ID)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
