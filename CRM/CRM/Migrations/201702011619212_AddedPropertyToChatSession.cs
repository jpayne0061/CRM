namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPropertyToChatSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatSessions", "RecipientName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatSessions", "RecipientName");
        }
    }
}
