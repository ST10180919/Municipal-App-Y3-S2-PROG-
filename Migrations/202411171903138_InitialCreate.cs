namespace Municipal_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ATTACHMENT",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NAME_OF_FILE = c.String(maxLength: 100),
                        FILE_BINARY = c.Binary(),
                        ISSUE_REPORT_ID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ISSUE_REPORT", t => t.ISSUE_REPORT_ID, cascadeDelete: true)
                .Index(t => t.ISSUE_REPORT_ID);
            
            CreateTable(
                "dbo.ISSUE_REPORT",
                c => new
                    {
                        IDENTIFIER = c.String(nullable: false, maxLength: 128),
                        LOCATION = c.String(),
                        DESCRIPTION = c.String(),
                        CATEGORY = c.String(maxLength: 100),
                        SOLUTION = c.String(),
                        STATUS_STRING = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDENTIFIER);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ATTACHMENT", "ISSUE_REPORT_ID", "dbo.ISSUE_REPORT");
            DropIndex("dbo.ATTACHMENT", new[] { "ISSUE_REPORT_ID" });
            DropTable("dbo.ISSUE_REPORT");
            DropTable("dbo.ATTACHMENT");
        }
    }
}
