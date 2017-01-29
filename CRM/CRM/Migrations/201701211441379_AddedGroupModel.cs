namespace CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGroupModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ManagerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ManagerId)
                .Index(t => t.ManagerId);
            
            AddColumn("dbo.AspNetUsers", "Group_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Group_Id1", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            CreateIndex("dbo.AspNetUsers", "Group_Id1");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id1", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Group_Id1", "dbo.Groups");
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Groups", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Groups", new[] { "ManagerId" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id1" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            DropColumn("dbo.AspNetUsers", "Group_Id1");
            DropColumn("dbo.AspNetUsers", "Group_Id");
            DropTable("dbo.Groups");
        }
    }
}
