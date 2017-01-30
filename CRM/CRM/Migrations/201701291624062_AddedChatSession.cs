namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedChatSession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        ReceiverId = c.String(),
                        SenderId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ChatMessages", "ChatSession_Id", c => c.Int());
            CreateIndex("dbo.ChatMessages", "ChatSession_Id");
            AddForeignKey("dbo.ChatMessages", "ChatSession_Id", "dbo.ChatSessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "ChatSession_Id", "dbo.ChatSessions");
            DropIndex("dbo.ChatMessages", new[] { "ChatSession_Id" });
            DropColumn("dbo.ChatMessages", "ChatSession_Id");
            DropTable("dbo.ChatSessions");
        }
    }
}
