namespace Municipal_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAttachmentMapping : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ATTACHMENTs", newName: "ATTACHMENT");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ATTACHMENT", newName: "ATTACHMENTs");
        }
    }
}
