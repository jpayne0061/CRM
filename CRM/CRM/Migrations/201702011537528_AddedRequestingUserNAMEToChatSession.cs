namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequestingUserNAMEToChatSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatSessions", "RequesterName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatSessions", "RequesterName");
        }
    }
}
