namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedJoinRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JoinRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequesterId = c.String(maxLength: 128),
                        ManagerId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RequesterId)
                .Index(t => t.RequesterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JoinRequests", "RequesterId", "dbo.AspNetUsers");
            DropIndex("dbo.JoinRequests", new[] { "RequesterId" });
            DropTable("dbo.JoinRequests");
        }
    }
}
