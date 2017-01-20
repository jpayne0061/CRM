namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserNotifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sender = c.String(),
                        CustomerName = c.String(),
                        Body = c.String(),
                        RecipientId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId)
                .Index(t => t.RecipientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "RecipientId", "dbo.AspNetUsers");
            DropIndex("dbo.UserNotifications", new[] { "RecipientId" });
            DropTable("dbo.UserNotifications");
        }
    }
}
