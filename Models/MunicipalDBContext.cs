using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents the Entity Framework DbContext for managing the municipal application's database entities.
    /// </summary>
    public class MunicipalDbContext : DbContext
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="MunicipalDbContext"/> class with the specified connection string.
        /// </summary>
        public MunicipalDbContext() : base("name=MunicipalDbContext")
        {
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the DbSet of issue reports.
        /// </summary>
        public virtual DbSet<ISSUE_REPORT> IssueReports { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the DbSet of attachments.
        /// </summary>
        public virtual DbSet<ATTACHMENT> Attachments { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Configures the model relationships and mappings for the database.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the database schema.</param>
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
//---------------------------------------EOF-------------------------------------------