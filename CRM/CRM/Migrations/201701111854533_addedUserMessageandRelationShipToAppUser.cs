namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserMessageandRelationShipToAppUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.MessageId })
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.MessageId);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMessages", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserMessages", "MessageId", "dbo.Messages");
            DropIndex("dbo.UserMessages", new[] { "MessageId" });
            DropIndex("dbo.UserMessages", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.UserMessages");
        }
    }
}
