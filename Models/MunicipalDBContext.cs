using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            // Configure primary keys and relationships if necessary

            modelBuilder.Entity<ISSUE_REPORT>()
                .HasKey(e => e.IDENTIFIER);

            modelBuilder.Entity<ATTACHMENT>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<ATTACHMENT>()
                .HasRequired(a => a.ISSUE_REPORT)
                .WithMany(i => i.ATTACHMENTS)
                .HasForeignKey(a => a.ISSUE_REPORT_ID)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
