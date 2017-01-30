namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequestingUserToChatSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatSessions", "RequestingUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatSessions", "RequestingUser");
        }
    }
}
